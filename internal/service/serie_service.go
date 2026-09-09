package service

import (
	"context"
	"errors"
	"time"

	"wafer-movie/internal/domain/dto"
	"wafer-movie/internal/domain/entity"
	"wafer-movie/internal/repository/redis"

	"github.com/google/uuid"
	"gorm.io/gorm"
)

var (
	ErrSerieNotFound = errors.New("serie not found")
)

type SerieService interface {
	GetById(ctx context.Context, id uuid.UUID) (*dto.GetSerieByIdResponse, error)
	Create(ctx context.Context, req dto.CreateSerieRequest) (uuid.UUID, error)
	Update(ctx context.Context, id uuid.UUID, req dto.UpdateSerieRequest) (uuid.UUID, error)
	Delete(ctx context.Context, id uuid.UUID) error
	Rate(ctx context.Context, serieId, userId uuid.UUID, score uint8) error
}

type serieService struct {
	db    *gorm.DB
	cache redis.CacheService
}

func NewSerieService(db *gorm.DB, cache redis.CacheService) SerieService {
	return &serieService{
		db:    db,
		cache: cache,
	}
}

func (s *serieService) GetById(ctx context.Context, id uuid.UUID) (*dto.GetSerieByIdResponse, error) {
	cacheKey := s.cache.GetSerieKey(id)
	var cached dto.GetSerieByIdResponse
	found, _ := s.cache.Get(ctx, cacheKey, &cached)
	if found {
		return &cached, nil
	}

	var serie entity.Serie
	if err := s.db.WithContext(ctx).Preload("Rates").Where(`"Id" = ?`, id).First(&serie).Error; err != nil {
		if errors.Is(err, gorm.ErrRecordNotFound) {
			return nil, ErrSerieNotFound
		}
		return nil, err
	}

	var avgScore float64
	if len(serie.Rates) > 0 {
		var total float64
		for _, r := range serie.Rates {
			total += float64(r.Score)
		}
		avgScore = total / float64(len(serie.Rates))
	}

	res := dto.GetSerieByIdResponse{
		Id:              serie.Id,
		IMDB:            serie.IMDB,
		Title:           serie.Title,
		Description:     serie.Description,
		Length:          serie.Length,
		IsFree:          serie.IsFree,
		StreamNetwork:   serie.StreamNetwork,
		AverageScore:    avgScore,
		FirstSeasonYear: serie.FirstSeasonYear,
		LastSeasonYear:  serie.LastSeasonYear,
	}

	_ = s.cache.Set(ctx, cacheKey, res, 24*time.Hour)
	return &res, nil
}

func (s *serieService) Create(ctx context.Context, req dto.CreateSerieRequest) (uuid.UUID, error) {
	var count int64
	if err := s.db.WithContext(ctx).Model(&entity.Serie{}).Where(`"IMDB" = ?`, req.IMDB).Count(&count).Error; err != nil {
		return uuid.Nil, err
	}
	if count > 0 {
		return uuid.Nil, ErrDuplicateIMDB
	}

	serie := entity.Serie{
		Title:           req.Title,
		Description:     req.Description,
		IMDB:            req.IMDB,
		AgeRestriction:  req.AgeRestriction,
		Unavailable:     req.Unavailable,
		Length:          req.Length,
		IsFree:          req.IsFree,
		FirstSeasonYear: req.FirstSeasonYear,
		LastSeasonYear:  req.LastSeasonYear,
	}

	if err := s.db.WithContext(ctx).Create(&serie).Error; err != nil {
		return uuid.Nil, err
	}

	_ = s.cache.Delete(ctx, "WaferMovie:Series:All")
	return serie.Id, nil
}

func (s *serieService) Update(ctx context.Context, id uuid.UUID, req dto.UpdateSerieRequest) (uuid.UUID, error) {
	var serie entity.Serie
	if err := s.db.WithContext(ctx).Where(`"Id" = ?`, id).First(&serie).Error; err != nil {
		if errors.Is(err, gorm.ErrRecordNotFound) {
			return uuid.Nil, ErrSerieNotFound
		}
		return uuid.Nil, err
	}

	serie.Title = req.Title
	serie.Description = req.Description
	serie.AgeRestriction = req.AgeRestriction
	serie.Unavailable = req.Unavailable
	serie.Length = req.Length
	serie.IsFree = req.IsFree
	serie.FirstSeasonYear = req.FirstSeasonYear
	serie.LastSeasonYear = req.LastSeasonYear

	if err := s.db.WithContext(ctx).Save(&serie).Error; err != nil {
		return uuid.Nil, err
	}

	_ = s.cache.Delete(ctx, "WaferMovie:Series:All", s.cache.GetSerieKey(id))
	return serie.Id, nil
}

func (s *serieService) Delete(ctx context.Context, id uuid.UUID) error {
	res := s.db.WithContext(ctx).Where(`"Id" = ?`, id).Delete(&entity.Serie{})
	if res.Error != nil {
		return res.Error
	}
	if res.RowsAffected == 0 {
		return ErrSerieNotFound
	}

	_ = s.cache.Delete(ctx, "WaferMovie:Series:All", s.cache.GetSerieKey(id))
	return nil
}

func (s *serieService) Rate(ctx context.Context, serieId, userId uuid.UUID, score uint8) error {
	if score < 1 || score > 10 {
		return ErrInvalidRating
	}

	var count int64
	if err := s.db.WithContext(ctx).Model(&entity.Serie{}).Where(`"Id" = ?`, serieId).Count(&count).Error; err != nil {
		return err
	}
	if count == 0 {
		return ErrSerieNotFound
	}

	var existing entity.SerieRate
	err := s.db.WithContext(ctx).Where(`"SerieId" = ? AND "UserId" = ?`, serieId, userId).First(&existing).Error
	if err != nil {
		if errors.Is(err, gorm.ErrRecordNotFound) {
			rate := entity.SerieRate{
				SerieId: serieId,
				UserId:  userId,
				Score:   score,
			}
			if err := s.db.WithContext(ctx).Create(&rate).Error; err != nil {
				return err
			}
		} else {
			return err
		}
	} else {
		existing.Score = score
		if err := s.db.WithContext(ctx).Save(&existing).Error; err != nil {
			return err
		}
	}

	_ = s.cache.Delete(ctx, s.cache.GetSerieKey(serieId))
	return nil
}
