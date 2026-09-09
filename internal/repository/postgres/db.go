package postgres

import (
	"context"
	"time"

	"wafer-movie/internal/config"
	"wafer-movie/internal/domain/entity"

	"gorm.io/driver/postgres"
	"gorm.io/gorm"
	"gorm.io/gorm/logger"
)

func New(cfg config.PostgresConfig) (*gorm.DB, error) {
	logLevel := logger.Silent
	db, err := gorm.Open(postgres.Open(cfg.DSN()), &gorm.Config{
		Logger: logger.Default.LogMode(logLevel),
	})
	if err != nil {
		return nil, err
	}

	sqlDB, err := db.DB()
	if err != nil {
		return nil, err
	}

	sqlDB.SetMaxIdleConns(10)
	sqlDB.SetMaxOpenConns(100)
	sqlDB.SetConnMaxLifetime(time.Hour)

	return db, nil
}

func AutoMigrate(db *gorm.DB) error {
	return db.AutoMigrate(
		&entity.User{},
		&entity.Role{},
		&entity.UserRole{},
		&entity.UserSession{},
		&entity.Movie{},
		&entity.MovieRate{},
		&entity.MovieDownloadLink{},
		&entity.Serie{},
		&entity.Season{},
		&entity.Episode{},
		&entity.SerieDownloadLink{},
		&entity.SerieRate{},
		&entity.Group{},
		&entity.MovieGroup{},
		&entity.SerieGroup{},
		&entity.Genre{},
	)
}

func Ping(ctx context.Context, db *gorm.DB) error {
	sqlDB, err := db.DB()
	if err != nil {
		return err
	}
	return sqlDB.PingContext(ctx)
}
