package token

import (
	"errors"
	"fmt"
	"time"

	"github.com/golang-jwt/jwt/v5"
	"github.com/google/uuid"
)

var (
	ErrInvalidToken = errors.New("invalid or expired token")
	ErrInvalidClaim = errors.New("invalid token claims")
)

type Config struct {
	SecretKey string
	Issuer    string
	Audience  string
	ExpiresIn time.Duration
}

type UserClaims struct {
	UserId        uuid.UUID `json:"nameid"`
	UserName      string    `json:"name"`
	Email         string    `json:"email"`
	SecurityStamp string    `json:"security_stamp"`
	BirthDate     string    `json:"birth_date,omitempty"`
	jwt.RegisteredClaims
}

type TokenService struct {
	cfg Config
}

func New(cfg Config) *TokenService {
	if cfg.ExpiresIn == 0 {
		cfg.ExpiresIn = 31 * 24 * time.Hour
	}
	return &TokenService{cfg: cfg}
}

func (s *TokenService) Generate(userId uuid.UUID, userName, email, securityStamp string, birthDate *time.Time) (string, error) {
	now := time.Now().UTC()
	jti, err := uuid.NewV7()
	if err != nil {
		jti = uuid.New()
	}

	bDateStr := ""
	if birthDate != nil {
		bDateStr = birthDate.Format("2006-01-02")
	}

	claims := UserClaims{
		UserId:        userId,
		UserName:      userName,
		Email:         email,
		SecurityStamp: securityStamp,
		BirthDate:     bDateStr,
		RegisteredClaims: jwt.RegisteredClaims{
			Subject:   userId.String(),
			Issuer:    s.cfg.Issuer,
			Audience:  jwt.ClaimStrings{s.cfg.Audience},
			IssuedAt:  jwt.NewNumericDate(now),
			NotBefore: jwt.NewNumericDate(now),
			ExpiresAt: jwt.NewNumericDate(now.Add(s.cfg.ExpiresIn)),
			ID:        jti.String(),
		},
	}

	token := jwt.NewWithClaims(jwt.SigningMethodHS256, claims)
	return token.SignedString([]byte(s.cfg.SecretKey))
}

func (s *TokenService) Validate(tokenString string) (*UserClaims, error) {
	token, err := jwt.ParseWithClaims(tokenString, &UserClaims{}, func(t *jwt.Token) (any, error) {
		if _, ok := t.Method.(*jwt.SigningMethodHMAC); !ok {
			return nil, fmt.Errorf("unexpected signing method: %v", t.Header["alg"])
		}
		return []byte(s.cfg.SecretKey), nil
	})

	if err != nil {
		return nil, ErrInvalidToken
	}

	claims, ok := token.Claims.(*UserClaims)
	if !ok || !token.Valid {
		return nil, ErrInvalidToken
	}

	if claims.UserId == uuid.Nil {
		parsedId, err := uuid.Parse(claims.Subject)
		if err == nil {
			claims.UserId = parsedId
		}
	}

	return claims, nil
}
