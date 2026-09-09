package token

import (
	"testing"
	"time"

	"github.com/google/uuid"
	"github.com/stretchr/testify/assert"
	"github.com/stretchr/testify/require"
)

func TestTokenService(t *testing.T) {
	svc := New(Config{
		SecretKey: "super-secret-key-that-is-long-enough-for-hs256",
		Issuer:    "WaferMovie",
		Audience:  "WaferMovieClient",
		ExpiresIn: time.Hour,
	})

	userId := uuid.New()
	userName := "john_doe"
	email := "john@example.com"
	securityStamp := "sec-stamp"
	birthDate := time.Date(1995, 5, 20, 0, 0, 0, 0, time.UTC)

	// Generate token
	tokenStr, err := svc.Generate(userId, userName, email, securityStamp, &birthDate)
	require.NoError(t, err)
	assert.NotEmpty(t, tokenStr)

	// Validate token
	claims, err := svc.Validate(tokenStr)
	require.NoError(t, err)
	assert.Equal(t, userId, claims.UserId)
	assert.Equal(t, userName, claims.UserName)
	assert.Equal(t, email, claims.Email)
	assert.Equal(t, securityStamp, claims.SecurityStamp)
	assert.Equal(t, "1995-05-20", claims.BirthDate)

	// Validate invalid token
	_, err = svc.Validate("invalid.jwt.token")
	assert.Error(t, err)
}
