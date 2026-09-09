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

type SerieHandler struct {
	svc service.SerieService
}

func NewSerieHandler(svc service.SerieService) *SerieHandler {
	return &SerieHandler{svc: svc}
}

func (h *SerieHandler) FindById(c *gin.Context) {
	loc := middleware.GetI18n(c)
	idStr := c.Param("id")
	id, err := uuid.Parse(idStr)
	if err != nil {
		response.JSON(c, response.Fail(response.StatusNotFound, loc.Shared("NotFound")))
		return
	}

	serie, err := h.svc.GetById(c.Request.Context(), id)
	if err != nil {
		if errors.Is(err, service.ErrSerieNotFound) {
			msg := loc.Validation("No {0} found with this {1}", loc.Property("serie"), loc.Property("Id"))
			response.JSON(c, response.Fail(response.StatusNotFound, msg))
			return
		}
		response.JSON(c, response.Fail(response.StatusError, loc.Shared("Error")))
		return
	}

	response.JSON(c, response.Success(serie))
}

func (h *SerieHandler) CreateSerie(c *gin.Context) {
	loc := middleware.GetI18n(c)
	var req dto.CreateSerieRequest
	if err := c.ShouldBindJSON(&req); err != nil {
		response.JSON(c, response.Fail(response.StatusInvalidData, loc.Shared("InvalidData")))
		return
	}

	newId, err := h.svc.Create(c.Request.Context(), req)
	if err != nil {
		if errors.Is(err, service.ErrDuplicateIMDB) {
			msg := loc.Validation("{0} already exists", loc.Property("IMDB"))
			response.JSON(c, response.Fail(response.StatusInvalidData, msg))
			return
		}
		response.JSON(c, response.Fail(response.StatusError, loc.Shared("Error")))
		return
	}

	response.JSON(c, response.Success(newId))
}

func (h *SerieHandler) UpdateSerie(c *gin.Context) {
	loc := middleware.GetI18n(c)
	idStr := c.Param("id")
	id, err := uuid.Parse(idStr)
	if err != nil {
		response.JSON(c, response.Fail(response.StatusNotFound, loc.Shared("NotFound")))
		return
	}

	var req dto.UpdateSerieRequest
	if err := c.ShouldBindJSON(&req); err != nil {
		response.JSON(c, response.Fail(response.StatusInvalidData, loc.Shared("InvalidData")))
		return
	}

	updatedId, err := h.svc.Update(c.Request.Context(), id, req)
	if err != nil {
		if errors.Is(err, service.ErrSerieNotFound) {
			response.JSON(c, response.Fail(response.StatusNotFound, loc.Shared("NotFound")))
			return
		}
		response.JSON(c, response.Fail(response.StatusError, loc.Shared("Error")))
		return
	}

	response.JSON(c, response.Success(updatedId))
}

func (h *SerieHandler) DeleteSerie(c *gin.Context) {
	loc := middleware.GetI18n(c)
	idStr := c.Param("id")
	id, err := uuid.Parse(idStr)
	if err != nil {
		response.JSON(c, response.Fail(response.StatusNotFound, loc.Shared("NotFound")))
		return
	}

	if err := h.svc.Delete(c.Request.Context(), id); err != nil {
		if errors.Is(err, service.ErrSerieNotFound) {
			response.JSON(c, response.Fail(response.StatusNotFound, loc.Shared("NotFound")))
			return
		}
		response.JSON(c, response.Fail(response.StatusError, loc.Shared("Error")))
		return
	}

	response.JSON(c, response.SuccessEmpty())
}

func (h *SerieHandler) CreateRate(c *gin.Context) {
	loc := middleware.GetI18n(c)
	userId, ok := middleware.GetCurrentUserId(c)
	if !ok {
		response.JSON(c, response.Fail(response.StatusUnauthorized, loc.Shared("Unauthorized")))
		return
	}

	idStr := c.Param("id")
	serieId, err := uuid.Parse(idStr)
	if err != nil {
		response.JSON(c, response.Fail(response.StatusNotFound, loc.Shared("NotFound")))
		return
	}

	var req dto.CreateSerieRateRequest
	if err := c.ShouldBindJSON(&req); err != nil {
		response.JSON(c, response.Fail(response.StatusInvalidData, loc.Shared("InvalidData")))
		return
	}

	if err := h.svc.Rate(c.Request.Context(), serieId, userId, req.Score); err != nil {
		if errors.Is(err, service.ErrSerieNotFound) {
			response.JSON(c, response.Fail(response.StatusNotFound, loc.Shared("NotFound")))
			return
		}
		if errors.Is(err, service.ErrInvalidRating) {
			response.JSON(c, response.Fail(response.StatusInvalidData, loc.Shared("InvalidData")))
			return
		}
		response.JSON(c, response.Fail(response.StatusError, loc.Shared("Error")))
		return
	}

	response.JSON(c, response.SuccessEmpty())
}
