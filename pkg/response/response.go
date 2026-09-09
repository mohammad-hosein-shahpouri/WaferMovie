package response

import (
	"net/http"

	"github.com/gin-gonic/gin"
)

type Status int

const (
	StatusSuccess      Status = 200
	StatusFailure      Status = 400
	StatusUnauthorized Status = 401
	StatusForbidden    Status = 403
	StatusNotFound     Status = 404
	StatusInvalidData  Status = 406
	StatusError        Status = 500
)

func (s Status) String() string {
	switch s {
	case StatusSuccess:
		return "Success"
	case StatusFailure:
		return "Failure"
	case StatusUnauthorized:
		return "Unauthorized"
	case StatusForbidden:
		return "Forbidden"
	case StatusNotFound:
		return "NotFound"
	case StatusInvalidData:
		return "InvalidData"
	case StatusError:
		return "Error"
	default:
		return "Unknown"
	}
}

type ApiResponse[T any] struct {
	Succeeded bool     `json:"succeeded"`
	Status    Status   `json:"status"`
	Messages  []string `json:"messages"`
	Data      T        `json:"data,omitempty"`
}

func Success[T any](data T, messages ...string) ApiResponse[T] {
	if len(messages) == 0 {
		messages = []string{StatusSuccess.String()}
	}
	return ApiResponse[T]{
		Succeeded: true,
		Status:    StatusSuccess,
		Messages:  messages,
		Data:      data,
	}
}

func SuccessEmpty(messages ...string) ApiResponse[any] {
	if len(messages) == 0 {
		messages = []string{StatusSuccess.String()}
	}
	return ApiResponse[any]{
		Succeeded: true,
		Status:    StatusSuccess,
		Messages:  messages,
	}
}

func Fail(status Status, messages ...string) ApiResponse[any] {
	if len(messages) == 0 {
		messages = []string{status.String()}
	}
	return ApiResponse[any]{
		Succeeded: false,
		Status:    status,
		Messages:  messages,
	}
}

func FailWithData[T any](status Status, data T, messages ...string) ApiResponse[T] {
	if len(messages) == 0 {
		messages = []string{status.String()}
	}
	return ApiResponse[T]{
		Succeeded: false,
		Status:    status,
		Messages:  messages,
		Data:      data,
	}
}

// JSON sends the ApiResponse with its HTTP status code matching res.Status
func JSON[T any](c *gin.Context, res ApiResponse[T]) {
	httpStatus := int(res.Status)
	if httpStatus < 100 || httpStatus > 599 {
		httpStatus = http.StatusOK
	}
	c.JSON(httpStatus, res)
}
