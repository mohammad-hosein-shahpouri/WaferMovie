package app

import (
	"net/http"

	"wafer-movie/internal/handler"
	"wafer-movie/internal/middleware"
	"wafer-movie/internal/repository/redis"
	"wafer-movie/internal/service"
	"wafer-movie/pkg/hasher"
	"wafer-movie/pkg/token"

	"github.com/gin-gonic/gin"
	"gorm.io/gorm"
)

type App struct {
	Engine       *gin.Engine
	DB           *gorm.DB
	Cache        redis.CacheService
	TokenService *token.TokenService
}

func SetupRouter(
	db *gorm.DB,
	cache redis.CacheService,
	tokenService *token.TokenService,
	passwordHasher hasher.Hasher,
) *gin.Engine {
	// Initialize services
	accountSvc := service.NewAccountService(db, cache, tokenService, passwordHasher)
	userSvc := service.NewUserService(db, passwordHasher)
	movieSvc := service.NewMovieService(db, cache)
	serieSvc := service.NewSerieService(db, cache)
	groupSvc := service.NewGroupService(db)
	searchSvc := service.NewSearchService(db)

	// Initialize handlers
	accountHandler := handler.NewAccountHandler(accountSvc)
	userHandler := handler.NewUserHandler(userSvc)
	movieHandler := handler.NewMovieHandler(movieSvc)
	serieHandler := handler.NewSerieHandler(serieSvc)
	groupHandler := handler.NewGroupHandler(groupSvc)
	searchHandler := handler.NewSearchHandler(searchSvc)
	healthHandler := handler.NewHealthHandler(db, cache)
	docsHandler := handler.NewDocsHandler()

	app := gin.New()
	app.Use(middleware.Recovery())
	app.Use(middleware.CORS())
	app.Use(middleware.Localization())

	// Health check probe
	app.GET("/ready", healthHandler.Ready)

	// API Documentation (Swagger & Scalar)
	app.GET("/docs/openapi.json", docsHandler.OpenAPISpec)
	app.GET("/swagger", docsHandler.SwaggerUI)
	app.GET("/swagger/*any", docsHandler.SwaggerUI)
	app.GET("/scalar", docsHandler.ScalarUI)
	app.GET("/docs", func(c *gin.Context) {
		c.Redirect(http.StatusMovedPermanently, "/scalar")
	})

	// API v1
	v1 := app.Group("/api/v1")
	{
		// Accounts
		accounts := v1.Group("/Accounts")
		{
			accounts.POST("/Login", accountHandler.Login)
			accounts.GET("", middleware.Auth(tokenService), accountHandler.GetCurrentUser)
			accounts.GET("/Sessions", middleware.Auth(tokenService), accountHandler.GetActiveSessions)
			accounts.DELETE("/Sessions/:id", middleware.Auth(tokenService), accountHandler.RevokeActiveSession)
		}

		// Users
		users := v1.Group("/Users", middleware.Auth(tokenService))
		{
			users.POST("", userHandler.CreateUser)
			users.PUT("/:id", userHandler.UpdateUser)
			users.DELETE("/:id", userHandler.DeleteUser)
		}

		// Movies
		movies := v1.Group("/Movies")
		{
			movies.GET("/:id", movieHandler.GetMovieById)
			movies.POST("", movieHandler.CreateMovie)
			movies.PUT("/:id", movieHandler.UpdateMovie)
			movies.DELETE("/:id", movieHandler.DeleteMovie)
			movies.POST("/:id/Rate", middleware.Auth(tokenService), movieHandler.CreateRate)
		}

		// Series
		series := v1.Group("/Series")
		{
			series.GET("/:id", serieHandler.FindById)
			series.POST("", serieHandler.CreateSerie)
			series.PUT("/:id", serieHandler.UpdateSerie)
			series.DELETE("/:id", serieHandler.DeleteSerie)
			series.POST("/:id/Rate", middleware.Auth(tokenService), serieHandler.CreateRate)
		}

		// Groups
		groups := v1.Group("/Groups")
		{
			groups.GET("/:id", groupHandler.GetGroupById)
			groups.POST("", groupHandler.CreateGroup)
			groups.PUT("/:id", groupHandler.UpdateGroup)
			groups.DELETE("/:id", groupHandler.DeleteGroup)
		}

		// Search
		search := v1.Group("/Search")
		{
			search.POST("", searchHandler.GetAllPaginated)
			search.POST("/Movies", searchHandler.GetMoviesPaginated)
			search.POST("/Series", searchHandler.GetSeriesPaginated)
		}
	}

	return app
}
