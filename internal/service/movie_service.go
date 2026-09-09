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
	ErrMovieNotFound = errors.New("movie not found")
	ErrDuplicateIMDB = errors.New("duplicate IMDB")
	ErrInvalidRating = errors.New("score must be between 1 and 10")
)

type MovieService interface {
	GetById(ctx context.Context, id uuid.UUID) (*dto.GetMovieByIdResponse, error)
	Create(ctx context.Context, req dto.CreateMovieRequest) (uuid.UUID, error)
	Update(ctx context.Context, id uuid.UUID, req dto.UpdateMovieRequest) (uuid.UUID, error)
	Delete(ctx context.Context, id uuid.UUID) error
	Rate(ctx context.Context, movieId, userId uuid.UUID, score uint8) error
}

type movieService struct {
	db    *gorm.DB
	cache redis.CacheService
}

func NewMovieService(db *gorm.DB, cache redis.CacheService) MovieService {
	return &movieService{
		db:    db,
		cache: cache,
	}
}

func (s *movieService) GetById(ctx context.Context, id uuid.UUID) (*dto.GetMovieByIdResponse, error) {
	cacheKey := s.cache.GetMovieKey(id)
	var cached dto.GetMovieByIdResponse
	found, _ := s.cache.Get(ctx, cacheKey, &cached)
	if found {
		return &cached, nil
	}

	var movie entity.Movie
	if err := s.db.WithContext(ctx).Preload("Rates").Where(`"Id" = ?`, id).First(&movie).Error; err != nil {
		if errors.Is(err, gorm.ErrRecordNotFound) {
			return nil, ErrMovieNotFound
		}
		return nil, err
	}

	var avgScore float64
	if len(movie.Rates) > 0 {
		var total float64
		for _, r := range movie.Rates {
			total += float64(r.Score)
		}
		avgScore = total / float64(len(movie.Rates))
	}

	res := dto.GetMovieByIdResponse{
		Id:             movie.Id,
		IMDB:           movie.IMDB,
		Title:          movie.Title,
		Description:    movie.Description,
		AverageScore:   avgScore,
		Length:         movie.Length,
		IsFree:         movie.IsFree,
		OutYear:        movie.OutYear,
		AgeRestriction: movie.AgeRestriction.String(),
	}

	_ = s.cache.Set(ctx, cacheKey, res, 24*time.Hour)
	return &res, nil
}

func (s *movieService) Create(ctx context.Context, req dto.CreateMovieRequest) (uuid.UUID, error) {
	var count int64
	if err := s.db.WithContext(ctx).Model(&entity.Movie{}).Where(`"IMDB" = ?`, req.IMDB).Count(&count).Error; err != nil {
		return uuid.Nil, err
	}
	if count > 0 {
		return uuid.Nil, ErrDuplicateIMDB
	}

	movie := entity.Movie{
		Title:          req.Title,
		Description:    req.Description,
		Unavailable:    req.Unavailable,
		Length:         req.Length,
		IsFree:         req.IsFree,
		OutYear:        req.OutYear,
		IMDB:           req.IMDB,
		AgeRestriction: req.AgeRestriction,
	}

	if err := s.db.WithContext(ctx).Create(&movie).Error; err != nil {
		return uuid.Nil, err
	}

	_ = s.cache.Delete(ctx, "WaferMovie:Movies:All")
	return movie.Id, nil
}

func (s *movieService) Update(ctx context.Context, id uuid.UUID, req dto.UpdateMovieRequest) (uuid.UUID, error) {
	var movie entity.Movie
	if err := s.db.WithContext(ctx).Where(`"Id" = ?`, id).First(&movie).Error; err != nil {
		if errors.Is(err, gorm.ErrRecordNotFound) {
			return uuid.Nil, ErrMovieNotFound
		}
		return uuid.Nil, err
	}

	movie.Title = req.Title
	movie.Description = req.Description
	movie.Unavailable = req.Unavailable
	movie.Length = req.Length
	movie.IsFree = req.IsFree
	movie.OutYear = req.OutYear
	movie.AgeRestriction = req.AgeRestriction

	if err := s.db.WithContext(ctx).Save(&movie).Error; err != nil {
		return uuid.Nil, err
	}

	_ = s.cache.Delete(ctx, "WaferMovie:Movies:All", s.cache.GetMovieKey(id))
	return movie.Id, nil
}

func (s *movieService) Delete(ctx context.Context, id uuid.UUID) error {
	res := s.db.WithContext(ctx).Where(`"Id" = ?`, id).Delete(&entity.Movie{})
	if res.Error != nil {
		return res.Error
	}
	if res.RowsAffected == 0 {
		return ErrMovieNotFound
	}

	_ = s.cache.Delete(ctx, "WaferMovie:Movies:All", s.cache.GetMovieKey(id))
	return nil
}

func (s *movieService) Rate(ctx context.Context, movieId, userId uuid.UUID, score uint8) error {
	if score < 1 || score > 10 {
		return ErrInvalidRating
	}

	var count int64
	if err := s.db.WithContext(ctx).Model(&entity.Movie{}).Where(`"Id" = ?`, movieId).Count(&count).Error; err != nil {
		return err
	}
	if count == 0 {
		return ErrMovieNotFound
	}

	var existing entity.MovieRate
	err := s.db.WithContext(ctx).Where(`"MovieId" = ? AND "UserId" = ?`, movieId, userId).First(&existing).Error
	if err != nil {
		if errors.Is(err, gorm.ErrRecordNotFound) {
			rate := entity.MovieRate{
				MovieId: movieId,
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

	_ = s.cache.Delete(ctx, s.cache.GetMovieKey(movieId))
	return nil
}
