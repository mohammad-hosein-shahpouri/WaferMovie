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

type UserHandler struct {
	svc service.UserService
}

func NewUserHandler(svc service.UserService) *UserHandler {
	return &UserHandler{svc: svc}
}

func (h *UserHandler) CreateUser(c *gin.Context) {
	loc := middleware.GetI18n(c)
	var req dto.CreateUserRequest
	if err := c.ShouldBindJSON(&req); err != nil {
		response.JSON(c, response.Fail(response.StatusInvalidData, loc.Shared("InvalidData")))
		return
	}

	newId, err := h.svc.Create(c.Request.Context(), req)
	if err != nil {
		if errors.Is(err, service.ErrPasswordMismatch) {
			msg := loc.Validation("{0} must match {1}", loc.Property("Password"), loc.Property("PasswordConfirmation"))
			response.JSON(c, response.Fail(response.StatusInvalidData, msg))
			return
		}
		if errors.Is(err, service.ErrPasswordTooWeak) {
			msg := loc.Validation("{0} must contain at least one letter and one number", loc.Property("Password"))
			response.JSON(c, response.Fail(response.StatusInvalidData, msg))
			return
		}
		if errors.Is(err, service.ErrDuplicateEmail) {
			msg := loc.Validation("{0} already exists", loc.Property("Email"))
			response.JSON(c, response.Fail(response.StatusInvalidData, msg))
			return
		}
		if errors.Is(err, service.ErrDuplicateUserName) {
			msg := loc.Validation("{0} already exists", loc.Property("UserName"))
			response.JSON(c, response.Fail(response.StatusInvalidData, msg))
			return
		}
		response.JSON(c, response.Fail(response.StatusError, loc.Shared("Error")))
		return
	}

	response.JSON(c, response.Success(newId))
}

func (h *UserHandler) UpdateUser(c *gin.Context) {
	loc := middleware.GetI18n(c)
	idStr := c.Param("id")
	id, err := uuid.Parse(idStr)
	if err != nil {
		response.JSON(c, response.Fail(response.StatusNotFound, loc.Shared("NotFound")))
		return
	}

	var req dto.UpdateUserRequest
	if err := c.ShouldBindJSON(&req); err != nil {
		response.JSON(c, response.Fail(response.StatusInvalidData, loc.Shared("InvalidData")))
		return
	}

	updatedId, err := h.svc.Update(c.Request.Context(), id, req)
	if err != nil {
		if errors.Is(err, service.ErrUserNotFound) {
			response.JSON(c, response.Fail(response.StatusNotFound, loc.Shared("NotFound")))
			return
		}
		response.JSON(c, response.Fail(response.StatusError, loc.Shared("Error")))
		return
	}

	response.JSON(c, response.Success(updatedId))
}

func (h *UserHandler) DeleteUser(c *gin.Context) {
	loc := middleware.GetI18n(c)
	idStr := c.Param("id")
	id, err := uuid.Parse(idStr)
	if err != nil {
		response.JSON(c, response.Fail(response.StatusNotFound, loc.Shared("NotFound")))
		return
	}

	if err := h.svc.Delete(c.Request.Context(), id); err != nil {
		if errors.Is(err, service.ErrUserNotFound) {
			response.JSON(c, response.Fail(response.StatusNotFound, loc.Shared("NotFound")))
			return
		}
		response.JSON(c, response.Fail(response.StatusError, loc.Shared("Error")))
		return
	}

	response.JSON(c, response.SuccessEmpty())
}
