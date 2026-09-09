package redis

import (
	"context"
	"encoding/json"
	"fmt"
	"time"

	"wafer-movie/internal/config"

	"github.com/google/uuid"
	redis "github.com/redis/go-redis/v9"
)

type CacheService interface {
	Get(ctx context.Context, key string, dest any) (bool, error)
	Set(ctx context.Context, key string, value any, expiration time.Duration) error
	Delete(ctx context.Context, keys ...string) error
	Ping(ctx context.Context) error

	GetUserKey(userId uuid.UUID) string
	GetMovieKey(movieId uuid.UUID) string
	GetSerieKey(serieId uuid.UUID) string
}

type redisCache struct {
	client *redis.Client
}

func New(cfg config.RedisConfig) CacheService {
	client := redis.NewClient(&redis.Options{
		Addr:     cfg.Addr,
		Password: cfg.Password,
		DB:       cfg.DB,
	})
	return &redisCache{client: client}
}

func (r *redisCache) GetUserKey(userId uuid.UUID) string {
	return fmt.Sprintf("WaferMovie:Users:%s", userId)
}

func (r *redisCache) GetMovieKey(movieId uuid.UUID) string {
	return fmt.Sprintf("WaferMovie:Movies:%s", movieId)
}

func (r *redisCache) GetSerieKey(serieId uuid.UUID) string {
	return fmt.Sprintf("WaferMovie:Series:%s", serieId)
}

func (r *redisCache) Get(ctx context.Context, key string, dest any) (bool, error) {
	val, err := r.client.Get(ctx, key).Result()
	if err == redis.Nil {
		return false, nil
	}
	if err != nil {
		return false, err
	}
	if err := json.Unmarshal([]byte(val), dest); err != nil {
		return false, err
	}
	return true, nil
}

func (r *redisCache) Set(ctx context.Context, key string, value any, expiration time.Duration) error {
	bytes, err := json.Marshal(value)
	if err != nil {
		return err
	}
	return r.client.Set(ctx, key, bytes, expiration).Err()
}

func (r *redisCache) Delete(ctx context.Context, keys ...string) error {
	return r.client.Del(ctx, keys...).Err()
}

func (r *redisCache) Ping(ctx context.Context) error {
	return r.client.Ping(ctx).Err()
}

// MemoryCache is a fallback/mock cache for testing or when Redis is disabled
type MemoryCache struct {
	store map[string]string
}

func NewMemoryCache() CacheService {
	return &MemoryCache{store: make(map[string]string)}
}

func (m *MemoryCache) GetUserKey(userId uuid.UUID) string {
	return fmt.Sprintf("WaferMovie:Users:%s", userId)
}

func (m *MemoryCache) GetMovieKey(movieId uuid.UUID) string {
	return fmt.Sprintf("WaferMovie:Movies:%s", movieId)
}

func (m *MemoryCache) GetSerieKey(serieId uuid.UUID) string {
	return fmt.Sprintf("WaferMovie:Series:%s", serieId)
}

func (m *MemoryCache) Get(ctx context.Context, key string, dest any) (bool, error) {
	val, ok := m.store[key]
	if !ok {
		return false, nil
	}
	if err := json.Unmarshal([]byte(val), dest); err != nil {
		return false, err
	}
	return true, nil
}

func (m *MemoryCache) Set(ctx context.Context, key string, value any, expiration time.Duration) error {
	bytes, err := json.Marshal(value)
	if err != nil {
		return err
	}
	m.store[key] = string(bytes)
	return nil
}

func (m *MemoryCache) Delete(ctx context.Context, keys ...string) error {
	for _, k := range keys {
		delete(m.store, k)
	}
	return nil
}

func (m *MemoryCache) Ping(ctx context.Context) error {
	return nil
}
