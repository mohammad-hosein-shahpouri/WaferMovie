package main

import (
	"context"
	"fmt"
	"log"
	"net/http"
	"os"
	"os/signal"
	"syscall"
	"time"

	"wafer-movie/internal/app"
	"wafer-movie/internal/config"
	"wafer-movie/internal/repository/postgres"
	"wafer-movie/internal/repository/redis"
	"wafer-movie/pkg/hasher"
	"wafer-movie/pkg/token"
)

func main() {
	cfg := config.Load()
	log.Printf("Starting WaferMovie API in %s mode...", cfg.Server.Environment)

	// Database
	db, err := postgres.New(cfg.Postgres)
	if err != nil {
		log.Fatalf("Failed to connect to PostgreSQL: %v", err)
	}

	// Auto-migrate tables
	if err := postgres.AutoMigrate(db); err != nil {
		log.Fatalf("Failed to auto-migrate database: %v", err)
	}
	log.Println("PostgreSQL connected and migrated successfully.")

	// Redis Cache
	cache := redis.New(cfg.Redis)

	// Token service & password hasher
	tokenService := token.New(token.Config{
		SecretKey: cfg.JWT.SecretKey,
		Issuer:    cfg.JWT.Issuer,
		Audience:  cfg.JWT.Audience,
		ExpiresIn: cfg.JWT.ExpiresIn,
	})
	passwordHasher := hasher.New()

	// Setup Gin Router
	router := app.SetupRouter(db, cache, tokenService, passwordHasher)

	server := &http.Server{
		Addr:         fmt.Sprintf(":%s", cfg.Server.Port),
		Handler:      router,
		ReadTimeout:  15 * time.Second,
		WriteTimeout: 15 * time.Second,
	}

	go func() {
		log.Printf("WaferMovie API listening on port %s", cfg.Server.Port)
		if err := server.ListenAndServe(); err != nil && err != http.ErrServerClosed {
			log.Fatalf("HTTP server error: %v", err)
		}
	}()

	// Graceful shutdown
	quit := make(chan os.Signal, 1)
	signal.Notify(quit, syscall.SIGINT, syscall.SIGTERM)
	<-quit
	log.Println("Shutting down server...")

	ctx, cancel := context.WithTimeout(context.Background(), 5*time.Second)
	defer cancel()

	if err := server.Shutdown(ctx); err != nil {
		log.Fatalf("Server forced to shutdown: %v", err)
	}

	log.Println("Server exiting")
}
