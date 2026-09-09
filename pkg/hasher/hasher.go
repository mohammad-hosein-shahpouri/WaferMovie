package hasher

import (
	"crypto/rand"
	"crypto/sha1"
	"crypto/sha256"
	"crypto/sha512"
	"crypto/subtle"
	"encoding/base64"
	"encoding/binary"
	"hash"
	"strings"

	"golang.org/x/crypto/bcrypt"
	"golang.org/x/crypto/pbkdf2"
)

type Hasher interface {
	HashPassword(password string) (string, error)
	VerifyPassword(hashedPassword, password string) bool
}

type defaultHasher struct {
	cost int
}

func New() Hasher {
	return &defaultHasher{cost: bcrypt.DefaultCost}
}

func (h *defaultHasher) HashPassword(password string) (string, error) {
	bytes, err := bcrypt.GenerateFromPassword([]byte(password), h.cost)
	if err != nil {
		return "", err
	}
	return string(bytes), nil
}

func (h *defaultHasher) VerifyPassword(hashedPassword, password string) bool {
	if strings.HasPrefix(hashedPassword, "$2a$") ||
		strings.HasPrefix(hashedPassword, "$2b$") ||
		strings.HasPrefix(hashedPassword, "$2y$") {
		err := bcrypt.CompareHashAndPassword([]byte(hashedPassword), []byte(password))
		return err == nil
	}

	// Try ASP.NET Identity V3 PBKDF2
	if verifyIdentityV3(hashedPassword, password) {
		return true
	}

	return false
}

// verifyIdentityV3 verifies passwords hashed using ASP.NET Core Identity V3 format
// Header format:
// byte 0: 0x01 (version 3)
// byte 1-4: prf (0=SHA1, 1=SHA256, 2=SHA512)
// byte 5-8: iter count (big-endian uint32)
// byte 9-12: salt size (big-endian uint32)
// byte 13..: salt
// followed by subkey
func verifyIdentityV3(hashedPassword, password string) bool {
	decoded, err := base64.StdEncoding.DecodeString(hashedPassword)
	if err != nil || len(decoded) < 13 {
		return false
	}

	if decoded[0] != 0x01 {
		return false
	}

	prfNum := binary.BigEndian.Uint32(decoded[1:5])
	iterCount := int(binary.BigEndian.Uint32(decoded[5:9]))
	saltLength := int(binary.BigEndian.Uint32(decoded[9:13]))

	if len(decoded) < 13+saltLength {
		return false
	}

	salt := decoded[13 : 13+saltLength]
	expectedSubkey := decoded[13+saltLength:]

	var prfFunc func() hash.Hash
	switch prfNum {
	case 0:
		prfFunc = sha1.New
	case 1:
		prfFunc = sha256.New
	case 2:
		prfFunc = sha512.New
	default:
		return false
	}

	actualSubkey := pbkdf2.Key([]byte(password), salt, iterCount, len(expectedSubkey), prfFunc)
	return subtle.ConstantTimeCompare(actualSubkey, expectedSubkey) == 1
}

// HashIdentityV3 generates an ASP.NET Identity V3 format hash (used for test compatibility)
func HashIdentityV3(password string, iterations int) (string, error) {
	if iterations <= 0 {
		iterations = 10000
	}
	salt := make([]byte, 16)
	if _, err := rand.Read(salt); err != nil {
		return "", err
	}

	subkey := pbkdf2.Key([]byte(password), salt, iterations, 32, sha256.New)

	output := make([]byte, 13+len(salt)+len(subkey))
	output[0] = 0x01
	binary.BigEndian.PutUint32(output[1:5], 1) // HMAC-SHA256
	binary.BigEndian.PutUint32(output[5:9], uint32(iterations))
	binary.BigEndian.PutUint32(output[9:13], uint32(len(salt)))
	copy(output[13:], salt)
	copy(output[13+len(salt):], subkey)

	return base64.StdEncoding.EncodeToString(output), nil
}
