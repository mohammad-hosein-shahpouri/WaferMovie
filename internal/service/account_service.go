package service

import (
	"context"
	"errors"
	"strings"
	"time"

	"wafer-movie/internal/domain/dto"
	"wafer-movie/internal/domain/entity"
	"wafer-movie/internal/domain/enum"
	"wafer-movie/internal/repository/redis"
	"wafer-movie/pkg/hasher"
	"wafer-movie/pkg/token"

	"github.com/google/uuid"
	"gorm.io/gorm"
)

var (
	ErrUserNotFound    = errors.New("user not found")
	ErrInvalidPassword = errors.New("invalid password")
	ErrSessionNotFound = errors.New("session not found")
)

type AccountService interface {
	Login(ctx context.Context, req dto.LoginRequest, remoteIP string) (*dto.LoginResponse, error)
	GetCurrentUser(ctx context.Context, userId uuid.UUID, remoteIP string) (*dto.GetCurrentUserResponse, error)
	GetActiveSessions(ctx context.Context, userId uuid.UUID) ([]dto.UserSessionResponse, error)
	RevokeSession(ctx context.Context, userId, sessionId uuid.UUID) error
}

type accountService struct {
	db           *gorm.DB
	cache        redis.CacheService
	tokenService *token.TokenService
	hasher       hasher.Hasher
}

func NewAccountService(db *gorm.DB, cache redis.CacheService, tokenService *token.TokenService, hasher hasher.Hasher) AccountService {
	return &accountService{
		db:           db,
		cache:        cache,
		tokenService: tokenService,
		hasher:       hasher,
	}
}

func (s *accountService) Login(ctx context.Context, req dto.LoginRequest, remoteIP string) (*dto.LoginResponse, error) {
	var user entity.User
	normalizedEmail := strings.ToUpper(strings.TrimSpace(req.Email))
	if err := s.db.WithContext(ctx).Where(`"NormalizedEmail" = ?`, normalizedEmail).First(&user).Error; err != nil {
		if errors.Is(err, gorm.ErrRecordNotFound) {
			return nil, ErrUserNotFound
		}
		return nil, err
	}

	if !s.hasher.VerifyPassword(user.PasswordHash, req.Password) {
		return nil, ErrInvalidPassword
	}

	// Create session
	session := entity.UserSession{
		UserId:       user.Id,
		CreationDate: time.Now().UTC(),
		IsActive:     true,
		IpAddress:    remoteIP,
		DeviceKind:   enum.DeviceKindUnknown,
		DeviceOs:     enum.DeviceOsUnknown,
		DeviceAgent:  enum.DeviceAgentNativeApplication,
	}
	_ = s.db.WithContext(ctx).Create(&session).Error

	jwtToken, err := s.tokenService.Generate(user.Id, user.NormalizedUserName, user.Email, user.SecurityStamp, user.BirthDate)
	if err != nil {
		return nil, err
	}

	return &dto.LoginResponse{
		Id:             user.Id,
		Name:           user.Name,
		UserName:       user.UserName,
		Email:          user.Email,
		AccountBalance: user.AccountBalance,
		Token:          jwtToken,
	}, nil
}

func (s *accountService) GetCurrentUser(ctx context.Context, userId uuid.UUID, remoteIP string) (*dto.GetCurrentUserResponse, error) {
	cacheKey := s.cache.GetUserKey(userId)
	var cached dto.GetCurrentUserResponse
	found, _ := s.cache.Get(ctx, cacheKey, &cached)
	if found {
		cached.IpAddress = remoteIP
		return &cached, nil
	}

	var user entity.User
	if err := s.db.WithContext(ctx).Where(`"Id" = ?`, userId).First(&user).Error; err != nil {
		if errors.Is(err, gorm.ErrRecordNotFound) {
			return nil, ErrUserNotFound
		}
		return nil, err
	}

	res := dto.GetCurrentUserResponse{
		Id:             user.Id,
		Name:           user.Name,
		UserName:       user.UserName,
		Email:          user.Email,
		AccountBalance: user.AccountBalance,
		IpAddress:      remoteIP,
	}

	_ = s.cache.Set(ctx, cacheKey, res, 24*time.Hour)
	return &res, nil
}

func (s *accountService) GetActiveSessions(ctx context.Context, userId uuid.UUID) ([]dto.UserSessionResponse, error) {
	var sessions []entity.UserSession
	if err := s.db.WithContext(ctx).Where(`"UserId" = ? AND "IsActive" = ?`, userId, true).Find(&sessions).Error; err != nil {
		return nil, err
	}

	result := make([]dto.UserSessionResponse, len(sessions))
	for i, sess := range sessions {
		result[i] = dto.UserSessionResponse{
			Id:           sess.Id,
			UserId:       sess.UserId,
			CreationDate: sess.CreationDate,
			IsActive:     sess.IsActive,
			IpAddress:    sess.IpAddress,
			DeviceKind:   sess.DeviceKind,
			DeviceOs:     sess.DeviceOs,
			DeviceAgent:  sess.DeviceAgent,
		}
	}
	return result, nil
}

func (s *accountService) RevokeSession(ctx context.Context, userId, sessionId uuid.UUID) error {
	res := s.db.WithContext(ctx).Model(&entity.UserSession{}).
		Where(`"Id" = ? AND "UserId" = ?`, sessionId, userId).
		Update("IsActive", false)
	if res.Error != nil {
		return res.Error
	}
	if res.RowsAffected == 0 {
		return ErrSessionNotFound
	}
	return nil
}
