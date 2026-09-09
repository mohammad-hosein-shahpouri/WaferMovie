package middleware

import (
	"net/http"
	"strings"

	"wafer-movie/pkg/i18n"
	"wafer-movie/pkg/response"
	"wafer-movie/pkg/token"

	"github.com/gin-gonic/gin"
	"github.com/google/uuid"
)

const (
	CtxKeyI18n     = "i18n"
	CtxKeyUserId   = "userId"
	CtxKeyUserName = "userName"
	CtxKeyEmail    = "email"
)

func Localization() gin.HandlerFunc {
	return func(c *gin.Context) {
		lang := c.GetHeader("Accept-Language")
		if lang == "" {
			lang = "en-US"
		}
		loc := i18n.New(lang)
		c.Set(CtxKeyI18n, loc)
		c.Header("Content-Language", string(loc.Language()))
		c.Next()
	}
}

func GetI18n(c *gin.Context) *i18n.I18n {
	if val, ok := c.Get(CtxKeyI18n); ok {
		if loc, ok := val.(*i18n.I18n); ok {
			return loc
		}
	}
	return i18n.New("en-US")
}

func Auth(tokenService *token.TokenService) gin.HandlerFunc {
	return func(c *gin.Context) {
		loc := GetI18n(c)
		authHeader := c.GetHeader("Authorization")
		if authHeader == "" {
			response.JSON(c, response.Fail(response.StatusUnauthorized, loc.Shared("Unauthorized")))
			c.Abort()
			return
		}

		parts := strings.SplitN(authHeader, " ", 2)
		if len(parts) != 2 || !strings.EqualFold(parts[0], "Bearer") {
			response.JSON(c, response.Fail(response.StatusUnauthorized, loc.Shared("Unauthorized")))
			c.Abort()
			return
		}

		tokenString := strings.TrimSpace(parts[1])
		claims, err := tokenService.Validate(tokenString)
		if err != nil {
			response.JSON(c, response.Fail(response.StatusUnauthorized, loc.Shared("Unauthorized")))
			c.Abort()
			return
		}

		c.Set(CtxKeyUserId, claims.UserId)
		c.Set(CtxKeyUserName, claims.UserName)
		c.Set(CtxKeyEmail, claims.Email)
		c.Next()
	}
}

func GetCurrentUserId(c *gin.Context) (uuid.UUID, bool) {
	val, ok := c.Get(CtxKeyUserId)
	if !ok {
		return uuid.Nil, false
	}
	id, ok := val.(uuid.UUID)
	return id, ok
}

func Recovery() gin.HandlerFunc {
	return func(c *gin.Context) {
		defer func() {
			if r := recover(); r != nil {
				loc := GetI18n(c)
				response.JSON(c, response.Fail(response.StatusError, loc.Shared("Error")))
				c.Abort()
			}
		}()
		c.Next()
	}
}

func CORS() gin.HandlerFunc {
	return func(c *gin.Context) {
		c.Header("Access-Control-Allow-Origin", "*")
		c.Header("Access-Control-Allow-Methods", "GET, POST, PUT, DELETE, OPTIONS")
		c.Header("Access-Control-Allow-Headers", "Origin, Content-Type, Accept, Authorization, Accept-Language")

		if c.Request.Method == http.MethodOptions {
			c.AbortWithStatus(http.StatusNoContent)
			return
		}

		c.Next()
	}
}
