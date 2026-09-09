package service

import (
	"context"
	"errors"
	"strings"

	"wafer-movie/internal/domain/dto"
	"wafer-movie/internal/domain/entity"
	"wafer-movie/internal/domain/enum"
	"wafer-movie/pkg/hasher"

	"github.com/google/uuid"
	"gorm.io/gorm"
)

var (
	ErrPasswordMismatch  = errors.New("passwords do not match")
	ErrPasswordTooWeak   = errors.New("password must contain at least one letter, one number and be at least 8 characters")
	ErrDuplicateEmail    = errors.New("email already in use")
	ErrDuplicateUserName = errors.New("username already in use")
)

func isPasswordComplex(p string) bool {
	if len(p) < 8 {
		return false
	}
	hasLetter := false
	hasDigit := false
	for _, ch := range p {
		switch {
		case (ch >= 'a' && ch <= 'z') || (ch >= 'A' && ch <= 'Z'):
			hasLetter = true
		case ch >= '0' && ch <= '9':
			hasDigit = true
		case strings.ContainsRune("!@#$%^&*-", ch):
			// allowed symbol
		default:
			// disallowed character
			return false
		}
	}
	return hasLetter && hasDigit
}

type UserService interface {
	Create(ctx context.Context, req dto.CreateUserRequest) (uuid.UUID, error)
	Update(ctx context.Context, id uuid.UUID, req dto.UpdateUserRequest) (uuid.UUID, error)
	Delete(ctx context.Context, id uuid.UUID) error
	GetById(ctx context.Context, id uuid.UUID) (*dto.GetUserResponse, error)
}

type userService struct {
	db     *gorm.DB
	hasher hasher.Hasher
}

func NewUserService(db *gorm.DB, hasher hasher.Hasher) UserService {
	return &userService{
		db:     db,
		hasher: hasher,
	}
}

func (s *userService) Create(ctx context.Context, req dto.CreateUserRequest) (uuid.UUID, error) {
	if req.Password != req.PasswordConfirmation {
		return uuid.Nil, ErrPasswordMismatch
	}

	if !isPasswordComplex(req.Password) {
		return uuid.Nil, ErrPasswordTooWeak
	}

	normEmail := strings.ToUpper(strings.TrimSpace(req.Email))
	normUserName := strings.ToUpper(strings.TrimSpace(req.UserName))

	// Check duplicates
	var count int64
	if err := s.db.WithContext(ctx).Model(&entity.User{}).Where(`"NormalizedEmail" = ?`, normEmail).Count(&count).Error; err != nil {
		return uuid.Nil, err
	}
	if count > 0 {
		return uuid.Nil, ErrDuplicateEmail
	}

	if err := s.db.WithContext(ctx).Model(&entity.User{}).Where(`"NormalizedUserName" = ?`, normUserName).Count(&count).Error; err != nil {
		return uuid.Nil, err
	}
	if count > 0 {
		return uuid.Nil, ErrDuplicateUserName
	}

	hash, err := s.hasher.HashPassword(req.Password)
	if err != nil {
		return uuid.Nil, err
	}

	user := entity.User{
		Name:                 req.Name,
		Email:                req.Email,
		NormalizedEmail:      normEmail,
		UserName:             req.UserName,
		NormalizedUserName:   normUserName,
		PhoneNumber:          req.PhoneNumber,
		PasswordHash:         hash,
		SecurityStamp:        uuid.New().String(),
		ConcurrencyStamp:     uuid.New().String(),
		Gender:               enum.GenderPreferNotToSay,
		AccountBalance:       0,
		EmailConfirmed:       false,
		PhoneNumberConfirmed: false,
	}

	if err := s.db.WithContext(ctx).Create(&user).Error; err != nil {
		return uuid.Nil, err
	}

	return user.Id, nil
}

func (s *userService) Update(ctx context.Context, id uuid.UUID, req dto.UpdateUserRequest) (uuid.UUID, error) {
	var user entity.User
	if err := s.db.WithContext(ctx).Where(`"Id" = ?`, id).First(&user).Error; err != nil {
		if errors.Is(err, gorm.ErrRecordNotFound) {
			return uuid.Nil, ErrUserNotFound
		}
		return uuid.Nil, err
	}

	user.Name = req.Name
	user.Email = req.Email
	user.NormalizedEmail = strings.ToUpper(strings.TrimSpace(req.Email))
	user.UserName = req.UserName
	user.NormalizedUserName = strings.ToUpper(strings.TrimSpace(req.UserName))
	user.PhoneNumber = req.PhoneNumber

	if err := s.db.WithContext(ctx).Save(&user).Error; err != nil {
		return uuid.Nil, err
	}

	return user.Id, nil
}

func (s *userService) Delete(ctx context.Context, id uuid.UUID) error {
	res := s.db.WithContext(ctx).Where(`"Id" = ?`, id).Delete(&entity.User{})
	if res.Error != nil {
		return res.Error
	}
	if res.RowsAffected == 0 {
		return ErrUserNotFound
	}
	return nil
}

func (s *userService) GetById(ctx context.Context, id uuid.UUID) (*dto.GetUserResponse, error) {
	var user entity.User
	if err := s.db.WithContext(ctx).Where(`"Id" = ?`, id).First(&user).Error; err != nil {
		if errors.Is(err, gorm.ErrRecordNotFound) {
			return nil, ErrUserNotFound
		}
		return nil, err
	}

	return &dto.GetUserResponse{
		Id:             user.Id,
		Name:           user.Name,
		Email:          user.Email,
		PhoneNumber:    user.PhoneNumber,
		UserName:       user.UserName,
		Gender:         user.Gender,
		AccountBalance: user.AccountBalance,
	}, nil
}
