package service

import (
	"context"

	"wafer-movie/internal/domain/dto"
	"wafer-movie/internal/domain/entity"
	"wafer-movie/internal/domain/enum"
	"wafer-movie/pkg/pagination"

	"gorm.io/gorm"
)

type SearchService interface {
	SearchMovies(ctx context.Context, input pagination.PaginationInput) (*pagination.PaginationOutput[dto.GetMoviesPaginatedResponse], error)
	SearchSeries(ctx context.Context, input pagination.PaginationInput) (*pagination.PaginationOutput[dto.GetMoviesPaginatedResponse], error)
}

type searchService struct {
	db *gorm.DB
}

func NewSearchService(db *gorm.DB) SearchService {
	return &searchService{db: db}
}

var movieColumns = []pagination.PaginationColumn{
	{
		DisplayName: "Id",
		Key:         "id",
		Hidden:      true,
		Sortable:    true,
		DataType:    enum.DataTypeGuid,
	},
	{
		DisplayName: "IMDB",
		Key:         "imdb",
		Hidden:      false,
		Sortable:    true,
		DataType:    enum.DataTypeString,
		SearchModel: &pagination.SearchModelResponse{
			Operations: []pagination.SearchOperation{
				{Name: "EqualTo", Value: enum.EqualTo},
				{Name: "Contains", Value: enum.Contains},
				{Name: "StartsWith", Value: enum.StartsWith},
			},
		},
	},
	{
		DisplayName: "Title",
		Key:         "title",
		Hidden:      false,
		Sortable:    true,
		DataType:    enum.DataTypeString,
		SearchModel: &pagination.SearchModelResponse{
			Operations: []pagination.SearchOperation{
				{Name: "EqualTo", Value: enum.EqualTo},
				{Name: "Contains", Value: enum.Contains},
				{Name: "StartsWith", Value: enum.StartsWith},
			},
		},
	},
	{
		DisplayName: "Description",
		Key:         "description",
		Hidden:      false,
		Sortable:    false,
		DataType:    enum.DataTypeString,
	},
	{
		DisplayName: "IsFree",
		Key:         "isFree",
		Hidden:      false,
		Sortable:    true,
		DataType:    enum.DataTypeBoolean,
		SearchModel: &pagination.SearchModelResponse{
			Operations: []pagination.SearchOperation{
				{Name: "EqualTo", Value: enum.EqualTo},
			},
		},
	},
	{
		DisplayName: "OutYear",
		Key:         "outYear",
		Hidden:      false,
		Sortable:    true,
		DataType:    enum.DataTypeNumber,
		SearchModel: &pagination.SearchModelResponse{
			Operations: []pagination.SearchOperation{
				{Name: "EqualTo", Value: enum.EqualTo},
				{Name: "GreaterThan", Value: enum.GreaterThan},
				{Name: "LessThan", Value: enum.LessThan},
			},
		},
	},
	{
		DisplayName: "AgeRestriction",
		Key:         "ageRestriction",
		Hidden:      false,
		Sortable:    true,
		DataType:    enum.DataTypeEnum,
		SearchModel: &pagination.SearchModelResponse{
			Operations: []pagination.SearchOperation{
				{Name: "EqualTo", Value: enum.EqualTo},
			},
			SearchItems: []pagination.SearchItem{
				{Name: "G", Value: int(enum.MovieAgeG)},
				{Name: "PG", Value: int(enum.MovieAgePG)},
				{Name: "PG-13", Value: int(enum.MovieAgePG13)},
				{Name: "R", Value: int(enum.MovieAgeR)},
				{Name: "NC-17", Value: int(enum.MovieAgeNC17)},
			},
		},
	},
}

var movieColumnMapping = map[string]pagination.ColumnMapping{
	"id":             {DBColumn: `"Id"`, DataType: enum.DataTypeGuid, Searchable: false, Sortable: true, Hidden: true},
	"imdb":           {DBColumn: `"IMDB"`, DataType: enum.DataTypeString, Searchable: true, Sortable: true},
	"title":          {DBColumn: `"Title"`, DataType: enum.DataTypeString, Searchable: true, Sortable: true},
	"description":    {DBColumn: `"Description"`, DataType: enum.DataTypeString, Searchable: false, Sortable: false},
	"isfree":         {DBColumn: `"IsFree"`, DataType: enum.DataTypeBoolean, Searchable: true, Sortable: true},
	"outyear":        {DBColumn: `"OutYear"`, DataType: enum.DataTypeNumber, Searchable: true, Sortable: true},
	"agerestriction": {DBColumn: `"AgeRestriction"`, DataType: enum.DataTypeEnum, Searchable: true, Sortable: true},
}

func (s *searchService) SearchMovies(ctx context.Context, input pagination.PaginationInput) (*pagination.PaginationOutput[dto.GetMoviesPaginatedResponse], error) {
	type movieResult struct {
		entity.Movie
	}

	res, err := pagination.Paginate[movieResult](s.db.WithContext(ctx).Model(&entity.Movie{}), input, movieColumnMapping, movieColumns)
	if err != nil {
		return nil, err
	}

	items := make([]dto.GetMoviesPaginatedResponse, len(res.Items))
	for i, m := range res.Items {
		items[i] = dto.GetMoviesPaginatedResponse{
			Id:             m.Id,
			IMDB:           m.IMDB,
			Title:          m.Title,
			Description:    m.Description,
			IsFree:         m.IsFree,
			OutYear:        m.OutYear,
			AgeRestriction: m.AgeRestriction.String(),
		}
	}

	return &pagination.PaginationOutput[dto.GetMoviesPaginatedResponse]{
		Items:      items,
		PageNumber: res.PageNumber,
		PageSize:   res.PageSize,
		TotalPages: res.TotalPages,
		TotalCount: res.TotalCount,
		Columns:    res.Columns,
		Order:      res.Order,
	}, nil
}

func (s *searchService) SearchSeries(ctx context.Context, input pagination.PaginationInput) (*pagination.PaginationOutput[dto.GetMoviesPaginatedResponse], error) {
	type serieResult struct {
		entity.Serie
	}

	serieColumnMapping := map[string]pagination.ColumnMapping{
		"id":             {DBColumn: `"Id"`, DataType: enum.DataTypeGuid, Searchable: false, Sortable: true, Hidden: true},
		"imdb":           {DBColumn: `"IMDB"`, DataType: enum.DataTypeString, Searchable: true, Sortable: true},
		"title":          {DBColumn: `"Title"`, DataType: enum.DataTypeString, Searchable: true, Sortable: true},
		"description":    {DBColumn: `"Description"`, DataType: enum.DataTypeString, Searchable: false, Sortable: false},
		"isfree":         {DBColumn: `"IsFree"`, DataType: enum.DataTypeBoolean, Searchable: true, Sortable: true},
		"outyear":        {DBColumn: `"FirstSeasonYear"`, DataType: enum.DataTypeNumber, Searchable: true, Sortable: true},
		"agerestriction": {DBColumn: `"AgeRestriction"`, DataType: enum.DataTypeEnum, Searchable: true, Sortable: true},
	}

	res, err := pagination.Paginate[serieResult](s.db.WithContext(ctx).Model(&entity.Serie{}), input, serieColumnMapping, movieColumns)
	if err != nil {
		return nil, err
	}

	items := make([]dto.GetMoviesPaginatedResponse, len(res.Items))
	for i, sr := range res.Items {
		items[i] = dto.GetMoviesPaginatedResponse{
			Id:             sr.Id,
			IMDB:           sr.IMDB,
			Title:          sr.Title,
			Description:    sr.Description,
			IsFree:         sr.IsFree,
			OutYear:        sr.FirstSeasonYear,
			AgeRestriction: sr.AgeRestriction.String(),
		}
	}

	return &pagination.PaginationOutput[dto.GetMoviesPaginatedResponse]{
		Items:      items,
		PageNumber: res.PageNumber,
		PageSize:   res.PageSize,
		TotalPages: res.TotalPages,
		TotalCount: res.TotalCount,
		Columns:    res.Columns,
		Order:      res.Order,
	}, nil
}
