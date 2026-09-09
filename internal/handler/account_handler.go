package handler

import (
	"errors"

	"wafer-movie/internal/domain/dto"
	"wafer-movie/internal/middleware"
	"wafer-movie/internal/service"
	"wafer-movie/pkg/response"

	"github.com/gin-gonic/gin"
	"github.com/google/uuid"
)

type AccountHandler struct {
	svc service.AccountService
}

func NewAccountHandler(svc service.AccountService) *AccountHandler {
	return &AccountHandler{svc: svc}
}

func (h *AccountHandler) Login(c *gin.Context) {
	loc := middleware.GetI18n(c)
	var req dto.LoginRequest
	if err := c.ShouldBindJSON(&req); err != nil {
		response.JSON(c, response.Fail(response.StatusInvalidData, loc.Shared("InvalidData")))
		return
	}

	res, err := h.svc.Login(c.Request.Context(), req, c.ClientIP())
	if err != nil {
		if errors.Is(err, service.ErrUserNotFound) || errors.Is(err, service.ErrInvalidPassword) {
			response.JSON(c, response.Fail(response.StatusFailure, loc.Shared("Failure")))
			return
		}
		response.JSON(c, response.Fail(response.StatusError, loc.Shared("Error")))
		return
	}

	response.JSON(c, response.Success(res))
}

func (h *AccountHandler) GetCurrentUser(c *gin.Context) {
	loc := middleware.GetI18n(c)
	userId, ok := middleware.GetCurrentUserId(c)
	if !ok {
		response.JSON(c, response.Fail(response.StatusUnauthorized, loc.Shared("Unauthorized")))
		return
	}

	res, err := h.svc.GetCurrentUser(c.Request.Context(), userId, c.ClientIP())
	if err != nil {
		if errors.Is(err, service.ErrUserNotFound) {
			response.JSON(c, response.Fail(response.StatusNotFound, loc.Shared("NotFound")))
			return
		}
		response.JSON(c, response.Fail(response.StatusError, loc.Shared("Error")))
		return
	}

	response.JSON(c, response.Success(res))
}

func (h *AccountHandler) GetActiveSessions(c *gin.Context) {
	loc := middleware.GetI18n(c)
	userId, ok := middleware.GetCurrentUserId(c)
	if !ok {
		response.JSON(c, response.Fail(response.StatusUnauthorized, loc.Shared("Unauthorized")))
		return
	}

	sessions, err := h.svc.GetActiveSessions(c.Request.Context(), userId)
	if err != nil {
		response.JSON(c, response.Fail(response.StatusError, loc.Shared("Error")))
		return
	}

	response.JSON(c, response.Success(sessions))
}

func (h *AccountHandler) RevokeActiveSession(c *gin.Context) {
	loc := middleware.GetI18n(c)
	userId, ok := middleware.GetCurrentUserId(c)
	if !ok {
		response.JSON(c, response.Fail(response.StatusUnauthorized, loc.Shared("Unauthorized")))
		return
	}

	idStr := c.Param("id")
	sessionId, err := uuid.Parse(idStr)
	if err != nil {
		response.JSON(c, response.Fail(response.StatusNotFound, loc.Shared("NotFound")))
		return
	}

	if err := h.svc.RevokeSession(c.Request.Context(), userId, sessionId); err != nil {
		if errors.Is(err, service.ErrSessionNotFound) {
			response.JSON(c, response.Fail(response.StatusNotFound, loc.Shared("NotFound")))
			return
		}
		response.JSON(c, response.Fail(response.StatusError, loc.Shared("Error")))
		return
	}

	response.JSON(c, response.SuccessEmpty())
}
