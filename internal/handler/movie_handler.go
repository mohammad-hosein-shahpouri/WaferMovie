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

type MovieHandler struct {
	svc service.MovieService
}

func NewMovieHandler(svc service.MovieService) *MovieHandler {
	return &MovieHandler{svc: svc}
}

func (h *MovieHandler) GetMovieById(c *gin.Context) {
	loc := middleware.GetI18n(c)
	idStr := c.Param("id")
	id, err := uuid.Parse(idStr)
	if err != nil {
		response.JSON(c, response.Fail(response.StatusNotFound, loc.Shared("NotFound")))
		return
	}

	movie, err := h.svc.GetById(c.Request.Context(), id)
	if err != nil {
		if errors.Is(err, service.ErrMovieNotFound) {
			msg := loc.Validation("{0} is not found", loc.Property("movie"))
			response.JSON(c, response.Fail(response.StatusNotFound, msg))
			return
		}
		response.JSON(c, response.Fail(response.StatusError, loc.Shared("Error")))
		return
	}

	response.JSON(c, response.Success(movie))
}

func (h *MovieHandler) CreateMovie(c *gin.Context) {
	loc := middleware.GetI18n(c)
	var req dto.CreateMovieRequest
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

func (h *MovieHandler) UpdateMovie(c *gin.Context) {
	loc := middleware.GetI18n(c)
	idStr := c.Param("id")
	id, err := uuid.Parse(idStr)
	if err != nil {
		response.JSON(c, response.Fail(response.StatusNotFound, loc.Shared("NotFound")))
		return
	}

	var req dto.UpdateMovieRequest
	if err := c.ShouldBindJSON(&req); err != nil {
		response.JSON(c, response.Fail(response.StatusInvalidData, loc.Shared("InvalidData")))
		return
	}

	updatedId, err := h.svc.Update(c.Request.Context(), id, req)
	if err != nil {
		if errors.Is(err, service.ErrMovieNotFound) {
			response.JSON(c, response.Fail(response.StatusNotFound, loc.Shared("NotFound")))
			return
		}
		response.JSON(c, response.Fail(response.StatusError, loc.Shared("Error")))
		return
	}

	response.JSON(c, response.Success(updatedId))
}

func (h *MovieHandler) DeleteMovie(c *gin.Context) {
	loc := middleware.GetI18n(c)
	idStr := c.Param("id")
	id, err := uuid.Parse(idStr)
	if err != nil {
		response.JSON(c, response.Fail(response.StatusNotFound, loc.Shared("NotFound")))
		return
	}

	if err := h.svc.Delete(c.Request.Context(), id); err != nil {
		if errors.Is(err, service.ErrMovieNotFound) {
			response.JSON(c, response.Fail(response.StatusNotFound, loc.Shared("NotFound")))
			return
		}
		response.JSON(c, response.Fail(response.StatusError, loc.Shared("Error")))
		return
	}

	response.JSON(c, response.SuccessEmpty())
}

func (h *MovieHandler) CreateRate(c *gin.Context) {
	loc := middleware.GetI18n(c)
	userId, ok := middleware.GetCurrentUserId(c)
	if !ok {
		response.JSON(c, response.Fail(response.StatusUnauthorized, loc.Shared("Unauthorized")))
		return
	}

	idStr := c.Param("id")
	movieId, err := uuid.Parse(idStr)
	if err != nil {
		response.JSON(c, response.Fail(response.StatusNotFound, loc.Shared("NotFound")))
		return
	}

	var req dto.CreateMovieRateRequest
	if err := c.ShouldBindJSON(&req); err != nil {
		response.JSON(c, response.Fail(response.StatusInvalidData, loc.Shared("InvalidData")))
		return
	}

	if err := h.svc.Rate(c.Request.Context(), movieId, userId, req.Score); err != nil {
		if errors.Is(err, service.ErrMovieNotFound) {
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
