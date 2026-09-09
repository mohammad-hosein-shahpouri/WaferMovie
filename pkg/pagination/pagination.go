package pagination

import (
	"fmt"
	"math"
	"strings"

	"wafer-movie/internal/domain/enum"

	"gorm.io/gorm"
)

type SortModel struct {
	ColumnName string `json:"columnName"`
	Ascending  bool   `json:"ascending"`
}

type SearchModelRequest struct {
	Key      string                  `json:"key"`
	Operator enum.ComparisonOperator `json:"operator"`
	Value    string                  `json:"value"`
}

type PaginationInput struct {
	SearchObjects []SearchModelRequest `json:"searchObjects"`
	Order         SortModel            `json:"order"`
	PageNumber    int                  `json:"pageNumber"`
	PageSize      int                  `json:"pageSize"`
}

type SearchOperation struct {
	Name  string                  `json:"name"`
	Value enum.ComparisonOperator `json:"value"`
}

type SearchItem struct {
	Name  string `json:"name"`
	Value int    `json:"value"`
}

type SearchModelResponse struct {
	Operator    *enum.ComparisonOperator `json:"operator,omitempty"`
	Operations  []SearchOperation        `json:"operations"`
	Value       *string                  `json:"value,omitempty"`
	SearchItems []SearchItem             `json:"searchItems,omitempty"`
}

type PaginationColumn struct {
	DisplayName        string               `json:"displayName"`
	Key                string               `json:"key"`
	ClassName          string               `json:"className"`
	PlaceHolder        string               `json:"placeHolder"`
	Prefix             string               `json:"prefix"`
	SpaceAfterPrefix   bool                 `json:"spaceAfterPrefix"`
	Postfix            string               `json:"postfix"`
	SpaceBeforePostfix bool                 `json:"spaceBeforePostfix"`
	Copy               bool                 `json:"copy"`
	Hidden             bool                 `json:"hidden"`
	Sortable           bool                 `json:"sortable"`
	DataType           enum.DataType        `json:"dataType"`
	SearchModel        *SearchModelResponse `json:"searchModel,omitempty"`
}

type PaginationOutput[T any] struct {
	Items      []T                `json:"items"`
	PageNumber int                `json:"pageNumber"`
	PageSize   int                `json:"pageSize"`
	TotalPages int                `json:"totalPages"`
	TotalCount int64              `json:"totalCount"`
	Columns    []PaginationColumn `json:"columns"`
	Order      SortModel          `json:"order"`
}

// ColumnMapping defines how JSON keys map to DB column names and types
type ColumnMapping struct {
	DBColumn   string
	DataType   enum.DataType
	Searchable bool
	Sortable   bool
	Hidden     bool
}

func ApplyFilter(db *gorm.DB, searchObjects []SearchModelRequest, allowedColumns map[string]ColumnMapping) *gorm.DB {
	for _, obj := range searchObjects {
		mapping, ok := allowedColumns[strings.ToLower(obj.Key)]
		if !ok || !mapping.Searchable {
			continue
		}

		col := mapping.DBColumn
		val := obj.Value

		switch obj.Operator {
		case enum.EqualTo:
			db = db.Where(fmt.Sprintf("%s = ?", col), val)
		case enum.NotEqualTo:
			db = db.Where(fmt.Sprintf("%s <> ?", col), val)
		case enum.GreaterThan:
			db = db.Where(fmt.Sprintf("%s > ?", col), val)
		case enum.LessThan:
			db = db.Where(fmt.Sprintf("%s < ?", col), val)
		case enum.GreaterThanOrEqual:
			db = db.Where(fmt.Sprintf("%s >= ?", col), val)
		case enum.LessThanOrEqual:
			db = db.Where(fmt.Sprintf("%s <= ?", col), val)
		case enum.Contains:
			db = db.Where(fmt.Sprintf("LOWER(%s) LIKE LOWER(?)", col), "%"+val+"%")
		case enum.NotContains:
			db = db.Where(fmt.Sprintf("LOWER(%s) NOT LIKE LOWER(?)", col), "%"+val+"%")
		case enum.StartsWith:
			db = db.Where(fmt.Sprintf("LOWER(%s) LIKE LOWER(?)", col), val+"%")
		case enum.EndsWith:
			db = db.Where(fmt.Sprintf("LOWER(%s) LIKE LOWER(?)", col), "%"+val)
		}
	}
	return db
}

func Paginate[T any](db *gorm.DB, input PaginationInput, allowedColumns map[string]ColumnMapping, columns []PaginationColumn) (*PaginationOutput[T], error) {
	pageNumber := input.PageNumber
	if pageNumber <= 0 {
		pageNumber = 1
	}
	pageSize := input.PageSize
	if pageSize <= 0 {
		pageSize = 10
	}

	query := ApplyFilter(db, input.SearchObjects, allowedColumns)

	var totalCount int64
	if err := query.Count(&totalCount).Error; err != nil {
		return nil, err
	}

	// Ordering
	orderCol := `"Id"`
	if input.Order.ColumnName != "" {
		if mapping, ok := allowedColumns[strings.ToLower(input.Order.ColumnName)]; ok {
			orderCol = mapping.DBColumn
		}
	}
	direction := "DESC"
	if input.Order.Ascending {
		direction = "ASC"
	}
	query = query.Order(fmt.Sprintf("%s %s", orderCol, direction))

	offset := (pageNumber - 1) * pageSize
	var items []T
	if err := query.Offset(offset).Limit(pageSize).Find(&items).Error; err != nil {
		return nil, err
	}

	totalPages := 0
	if totalCount > 0 {
		totalPages = int(math.Ceil(float64(totalCount) / float64(pageSize)))
	}

	order := input.Order
	if order.ColumnName == "" {
		order.ColumnName = "Id"
		order.Ascending = false
	}

	return &PaginationOutput[T]{
		Items:      items,
		PageNumber: pageNumber,
		PageSize:   pageSize,
		TotalPages: totalPages,
		TotalCount: totalCount,
		Columns:    columns,
		Order:      order,
	}, nil
}
