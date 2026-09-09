package handler

import (
	"wafer-movie/internal/middleware"
	"wafer-movie/internal/service"
	"wafer-movie/pkg/pagination"
	"wafer-movie/pkg/response"

	"github.com/gin-gonic/gin"
)

type SearchHandler struct {
	svc service.SearchService
}

func NewSearchHandler(svc service.SearchService) *SearchHandler {
	return &SearchHandler{svc: svc}
}

func (h *SearchHandler) GetAllPaginated(c *gin.Context) {
	h.GetMoviesPaginated(c)
}

func (h *SearchHandler) GetMoviesPaginated(c *gin.Context) {
	loc := middleware.GetI18n(c)
	var input pagination.PaginationInput
	if err := c.ShouldBindJSON(&input); err != nil {
		response.JSON(c, response.Fail(response.StatusInvalidData, loc.Shared("InvalidData")))
		return
	}

	res, err := h.svc.SearchMovies(c.Request.Context(), input)
	if err != nil {
		response.JSON(c, response.Fail(response.StatusError, loc.Shared("Error")))
		return
	}

	response.JSON(c, response.Success(res))
}

func (h *SearchHandler) GetSeriesPaginated(c *gin.Context) {
	loc := middleware.GetI18n(c)
	var input pagination.PaginationInput
	if err := c.ShouldBindJSON(&input); err != nil {
		response.JSON(c, response.Fail(response.StatusInvalidData, loc.Shared("InvalidData")))
		return
	}

	res, err := h.svc.SearchSeries(c.Request.Context(), input)
	if err != nil {
		response.JSON(c, response.Fail(response.StatusError, loc.Shared("Error")))
		return
	}

	response.JSON(c, response.Success(res))
}
