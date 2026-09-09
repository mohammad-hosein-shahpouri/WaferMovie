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

type GroupHandler struct {
	svc service.GroupService
}

func NewGroupHandler(svc service.GroupService) *GroupHandler {
	return &GroupHandler{svc: svc}
}

func (h *GroupHandler) GetGroupById(c *gin.Context) {
	loc := middleware.GetI18n(c)
	idStr := c.Param("id")
	id, err := uuid.Parse(idStr)
	if err != nil {
		response.JSON(c, response.Fail(response.StatusNotFound, loc.Shared("NotFound")))
		return
	}

	group, err := h.svc.GetById(c.Request.Context(), id)
	if err != nil {
		if errors.Is(err, service.ErrGroupNotFound) {
			response.JSON(c, response.Fail(response.StatusNotFound, loc.Shared("NotFound")))
			return
		}
		response.JSON(c, response.Fail(response.StatusError, loc.Shared("Error")))
		return
	}

	response.JSON(c, response.Success(group))
}

func (h *GroupHandler) CreateGroup(c *gin.Context) {
	loc := middleware.GetI18n(c)
	var req dto.CreateGroupRequest
	if err := c.ShouldBindJSON(&req); err != nil {
		response.JSON(c, response.Fail(response.StatusInvalidData, loc.Shared("InvalidData")))
		return
	}

	newId, err := h.svc.Create(c.Request.Context(), req)
	if err != nil {
		response.JSON(c, response.Fail(response.StatusError, loc.Shared("Error")))
		return
	}

	response.JSON(c, response.Success(newId))
}

func (h *GroupHandler) UpdateGroup(c *gin.Context) {
	loc := middleware.GetI18n(c)
	idStr := c.Param("id")
	id, err := uuid.Parse(idStr)
	if err != nil {
		response.JSON(c, response.Fail(response.StatusNotFound, loc.Shared("NotFound")))
		return
	}

	var req dto.UpdateGroupRequest
	if err := c.ShouldBindJSON(&req); err != nil {
		response.JSON(c, response.Fail(response.StatusInvalidData, loc.Shared("InvalidData")))
		return
	}

	updatedId, err := h.svc.Update(c.Request.Context(), id, req)
	if err != nil {
		if errors.Is(err, service.ErrGroupNotFound) {
			response.JSON(c, response.Fail(response.StatusNotFound, loc.Shared("NotFound")))
			return
		}
		response.JSON(c, response.Fail(response.StatusError, loc.Shared("Error")))
		return
	}

	response.JSON(c, response.Success(updatedId))
}

func (h *GroupHandler) DeleteGroup(c *gin.Context) {
	loc := middleware.GetI18n(c)
	idStr := c.Param("id")
	id, err := uuid.Parse(idStr)
	if err != nil {
		response.JSON(c, response.Fail(response.StatusNotFound, loc.Shared("NotFound")))
		return
	}

	if err := h.svc.Delete(c.Request.Context(), id); err != nil {
		if errors.Is(err, service.ErrGroupNotFound) {
			response.JSON(c, response.Fail(response.StatusNotFound, loc.Shared("NotFound")))
			return
		}
		response.JSON(c, response.Fail(response.StatusError, loc.Shared("Error")))
		return
	}

	response.JSON(c, response.SuccessEmpty())
}
