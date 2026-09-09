package service

import (
	"context"
	"errors"

	"wafer-movie/internal/domain/dto"
	"wafer-movie/internal/domain/entity"

	"github.com/google/uuid"
	"gorm.io/gorm"
)

var (
	ErrGroupNotFound = errors.New("group not found")
)

type GroupService interface {
	GetById(ctx context.Context, id uuid.UUID) (*dto.GetGroupByIdResponse, error)
	Create(ctx context.Context, req dto.CreateGroupRequest) (uuid.UUID, error)
	Update(ctx context.Context, id uuid.UUID, req dto.UpdateGroupRequest) (uuid.UUID, error)
	Delete(ctx context.Context, id uuid.UUID) error
}

type groupService struct {
	db *gorm.DB
}

func NewGroupService(db *gorm.DB) GroupService {
	return &groupService{db: db}
}

func (s *groupService) GetById(ctx context.Context, id uuid.UUID) (*dto.GetGroupByIdResponse, error) {
	var group entity.Group
	if err := s.db.WithContext(ctx).Where(`"Id" = ? AND "IsDeleted" = ?`, id, false).First(&group).Error; err != nil {
		if errors.Is(err, gorm.ErrRecordNotFound) {
			return nil, ErrGroupNotFound
		}
		return nil, err
	}

	return &dto.GetGroupByIdResponse{
		Id:                group.Id,
		Name:              group.Name,
		Description:       &group.Description,
		ImageUrl:          group.ImageUrl,
		ImageThumbnailUrl: group.ImageThumbnailUrl,
	}, nil
}

func (s *groupService) Create(ctx context.Context, req dto.CreateGroupRequest) (uuid.UUID, error) {
	desc := ""
	if req.Description != nil {
		desc = *req.Description
	}

	group := entity.Group{
		Name:              req.Name,
		Description:       desc,
		ImageUrl:          req.ImageUrl,
		ImageThumbnailUrl: req.ImageThumbnailUrl,
		IsPublic:          req.IsPublic,
		IsDeleted:         false,
	}

	if err := s.db.WithContext(ctx).Create(&group).Error; err != nil {
		return uuid.Nil, err
	}

	return group.Id, nil
}

func (s *groupService) Update(ctx context.Context, id uuid.UUID, req dto.UpdateGroupRequest) (uuid.UUID, error) {
	var group entity.Group
	if err := s.db.WithContext(ctx).Where(`"Id" = ? AND "IsDeleted" = ?`, id, false).First(&group).Error; err != nil {
		if errors.Is(err, gorm.ErrRecordNotFound) {
			return uuid.Nil, ErrGroupNotFound
		}
		return uuid.Nil, err
	}

	group.Name = req.Name
	if req.Description != nil {
		group.Description = *req.Description
	}
	group.ImageUrl = req.ImageUrl
	group.ImageThumbnailUrl = req.ImageThumbnailUrl
	group.IsPublic = req.IsPublic

	if err := s.db.WithContext(ctx).Save(&group).Error; err != nil {
		return uuid.Nil, err
	}

	return group.Id, nil
}

func (s *groupService) Delete(ctx context.Context, id uuid.UUID) error {
	res := s.db.WithContext(ctx).Model(&entity.Group{}).
		Where(`"Id" = ? AND "IsDeleted" = ?`, id, false).
		Update("IsDeleted", true)
	if res.Error != nil {
		return res.Error
	}
	if res.RowsAffected == 0 {
		return ErrGroupNotFound
	}
	return nil
}
