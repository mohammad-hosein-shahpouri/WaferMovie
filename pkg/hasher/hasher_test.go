package hasher

import (
	"testing"

	"github.com/stretchr/testify/assert"
	"github.com/stretchr/testify/require"
)

func TestHasher(t *testing.T) {
	h := New()

	password := "SecretP@ssword123!"

	// Test bcrypt hashing and verification
	hashed, err := h.HashPassword(password)
	require.NoError(t, err)
	assert.NotEmpty(t, hashed)

	assert.True(t, h.VerifyPassword(hashed, password))
	assert.False(t, h.VerifyPassword(hashed, "WrongPassword!"))

	// Test ASP.NET Identity V3 format compatibility
	v3Hash, err := HashIdentityV3(password, 10000)
	require.NoError(t, err)
	assert.NotEmpty(t, v3Hash)

	assert.True(t, h.VerifyPassword(v3Hash, password))
	assert.False(t, h.VerifyPassword(v3Hash, "WrongPassword!"))
}
