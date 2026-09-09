package handler

import (
	"context"
	"net/http"
	"time"

	"wafer-movie/internal/repository/postgres"
	"wafer-movie/internal/repository/redis"

	"github.com/gin-gonic/gin"
	"gorm.io/gorm"
)

type HealthHandler struct {
	db    *gorm.DB
	cache redis.CacheService
}

func NewHealthHandler(db *gorm.DB, cache redis.CacheService) *HealthHandler {
	return &HealthHandler{db: db, cache: cache}
}

func (h *HealthHandler) Ready(c *gin.Context) {
	ctx, cancel := context.WithTimeout(c.Request.Context(), 3*time.Second)
	defer cancel()

	pgStatus := "Healthy"
	if err := postgres.Ping(ctx, h.db); err != nil {
		pgStatus = "Unhealthy"
	}

	redisStatus := "Healthy"
	if err := h.cache.Ping(ctx); err != nil {
		redisStatus = "Unhealthy"
	}

	status := http.StatusOK
	overall := "Healthy"
	if pgStatus != "Healthy" || redisStatus != "Healthy" {
		status = http.StatusServiceUnavailable
		overall = "Unhealthy"
	}

	c.JSON(status, gin.H{
		"status": overall,
		"entries": gin.H{
			"PostgresHealthCheck": gin.H{
				"status":      pgStatus,
				"description": "Postgres is up and running.",
			},
			"RedisHealthCheck": gin.H{
				"status":      redisStatus,
				"description": "Redis is up and running.",
			},
		},
	})
}
