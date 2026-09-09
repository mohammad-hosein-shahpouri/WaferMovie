package config

import (
	"fmt"
	"os"
	"strconv"
	"strings"
	"time"
)

type Config struct {
	Server   ServerConfig
	Postgres PostgresConfig
	Redis    RedisConfig
	JWT      JWTConfig
}

type ServerConfig struct {
	Port        string
	Environment string
}

type PostgresConfig struct {
	Host     string
	Port     string
	User     string
	Password string
	DBName   string
	SSLMode  string
}

func (p PostgresConfig) DSN() string {
	return fmt.Sprintf("host=%s user=%s password=%s dbname=%s port=%s sslmode=%s TimeZone=Asia/Tehran",
		p.Host, p.User, p.Password, p.DBName, p.Port, p.SSLMode)
}

type RedisConfig struct {
	Addr     string
	Password string
	DB       int
}

type JWTConfig struct {
	SecretKey string
	Issuer    string
	Audience  string
	ExpiresIn time.Duration
}

func getEnv(key, defaultVal string) string {
	if val := os.Getenv(key); val != "" {
		return val
	}
	return defaultVal
}

func Load() *Config {
	port := getEnv("PORT", "8080")
	env := getEnv("ENVIRONMENT", "development")

	pgHost := getEnv("POSTGRES_HOST", "127.0.0.1")
	pgPort := getEnv("POSTGRES_PORT", "5432")
	pgUser := getEnv("POSTGRES_USER", "postgres")
	pgPass := getEnv("POSTGRES_PASSWORD", "postgres")
	pgDB := getEnv("POSTGRES_DB", "WaferMovie.App")
	pgSSL := getEnv("POSTGRES_SSLMODE", "disable")

	// Parse connection string if provided directly
	if connStr := os.Getenv("POSTGRES_CONNECTION_STRING"); connStr != "" {
		// Can parse key-values
		parts := strings.Split(connStr, ";")
		for _, part := range parts {
			kv := strings.SplitN(strings.TrimSpace(part), "=", 2)
			if len(kv) == 2 {
				k, v := strings.ToLower(kv[0]), kv[1]
				switch k {
				case "host":
					pgHost = v
				case "port":
					pgPort = v
				case "database":
					pgDB = v
				case "username":
					pgUser = v
				case "password":
					pgPass = v
				}
			}
		}
	}

	redisAddr := getEnv("REDIS_ADDR", "127.0.0.1:6379")
	redisPassword := getEnv("REDIS_PASSWORD", "")
	redisDB := 0
	if dbStr := os.Getenv("REDIS_DB"); dbStr != "" {
		if val, err := strconv.Atoi(dbStr); err == nil {
			redisDB = val
		}
	}

	jwtSecret := getEnv("JWT_SECRET", "103Tx^YqUB9WwWnHT^94")
	jwtIssuer := getEnv("JWT_ISSUER", "LocalHost")
	jwtAudience := getEnv("JWT_AUDIENCE", "LocalHost")

	return &Config{
		Server: ServerConfig{
			Port:        port,
			Environment: env,
		},
		Postgres: PostgresConfig{
			Host:     pgHost,
			Port:     pgPort,
			User:     pgUser,
			Password: pgPass,
			DBName:   pgDB,
			SSLMode:  pgSSL,
		},
		Redis: RedisConfig{
			Addr:     redisAddr,
			Password: redisPassword,
			DB:       redisDB,
		},
		JWT: JWTConfig{
			SecretKey: jwtSecret,
			Issuer:    jwtIssuer,
			Audience:  jwtAudience,
			ExpiresIn: 31 * 24 * time.Hour,
		},
	}
}
