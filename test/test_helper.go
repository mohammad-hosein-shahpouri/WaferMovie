package test

import (
	"testing"
	"time"

	"wafer-movie/internal/app"
	"wafer-movie/internal/domain/entity"
	"wafer-movie/internal/repository/redis"
	"wafer-movie/pkg/hasher"
	"wafer-movie/pkg/token"

	"github.com/gin-gonic/gin"
	"github.com/glebarez/sqlite"
	"github.com/stretchr/testify/require"
	"gorm.io/gorm"
	"gorm.io/gorm/logger"
)

type TestFixture struct {
	Engine       *gin.Engine
	DB           *gorm.DB
	Cache        redis.CacheService
	TokenService *token.TokenService
	Hasher       hasher.Hasher
}

func SetupTestFixture(t *testing.T) *TestFixture {
	gin.SetMode(gin.TestMode)

	db, err := gorm.Open(sqlite.Open("file::memory:?cache=shared"), &gorm.Config{
		Logger: logger.Default.LogMode(logger.Silent),
	})
	require.NoError(t, err)

	// Migrate tables
	err = db.AutoMigrate(
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
	require.NoError(t, err)

	cache := redis.NewMemoryCache()
	tokenService := token.New(token.Config{
		SecretKey: "test-secret-key-12345678901234567890",
		Issuer:    "TestIssuer",
		Audience:  "TestAudience",
		ExpiresIn: 24 * time.Hour,
	})
	passwordHasher := hasher.New()

	router := app.SetupRouter(db, cache, tokenService, passwordHasher)

	return &TestFixture{
		Engine:       router,
		DB:           db,
		Cache:        cache,
		TokenService: tokenService,
		Hasher:       passwordHasher,
	}
}
