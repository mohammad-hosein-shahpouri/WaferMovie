package dto

import (
	"time"

	"wafer-movie/internal/domain/enum"

	"github.com/google/uuid"
)

// Accounts DTOs
type LoginRequest struct {
	Email        string `json:"email" binding:"required,email"`
	Password     string `json:"password" binding:"required"`
	IsPersistent bool   `json:"isPersistent"`
}

type LoginResponse struct {
	Id             uuid.UUID `json:"id"`
	Name           string    `json:"name"`
	UserName       string    `json:"userName"`
	Email          string    `json:"email"`
	AccountBalance int       `json:"accountBalance"`
	Token          string    `json:"token"`
}

type GetCurrentUserResponse struct {
	Id             uuid.UUID `json:"id"`
	Name           string    `json:"name"`
	IpAddress      string    `json:"ipAddress,omitempty"`
	UserName       string    `json:"userName"`
	Email          string    `json:"email"`
	AccountBalance int       `json:"accountBalance"`
}

type UserSessionResponse struct {
	Id           uuid.UUID        `json:"id"`
	UserId       uuid.UUID        `json:"userId"`
	CreationDate time.Time        `json:"creationDate"`
	IsActive     bool             `json:"isActive"`
	IpAddress    string           `json:"ipAddress"`
	DeviceKind   enum.DeviceKind  `json:"deviceKind"`
	DeviceOs     enum.DeviceOs    `json:"deviceOs"`
	DeviceAgent  enum.DeviceAgent `json:"deviceAgent"`
}

// Users DTOs
type CreateUserRequest struct {
	Name                 string `json:"name" binding:"required,max=30"`
	Email                string `json:"email" binding:"required,email"`
	PhoneNumber          string `json:"phoneNumber" binding:"required"`
	UserName             string `json:"userName" binding:"required"`
	Password             string `json:"password" binding:"required"`
	PasswordConfirmation string `json:"passwordConfirmation" binding:"required"`
}

type UpdateUserRequest struct {
	Name        string `json:"name" binding:"required"`
	Email       string `json:"email" binding:"required,email"`
	PhoneNumber string `json:"phoneNumber" binding:"required"`
	UserName    string `json:"userName" binding:"required"`
}

type GetUserResponse struct {
	Id             uuid.UUID   `json:"id"`
	Name           string      `json:"name"`
	Email          string      `json:"email"`
	PhoneNumber    string      `json:"phoneNumber"`
	UserName       string      `json:"userName"`
	Gender         enum.Gender `json:"gender"`
	AccountBalance int         `json:"accountBalance"`
}

// Movies DTOs
type CreateMovieRequest struct {
	Title          string                   `json:"title" binding:"required,max=100"`
	Description    string                   `json:"description" binding:"max=500"`
	Unavailable    bool                     `json:"unavailable"`
	Length         int                      `json:"length"`
	IsFree         bool                     `json:"isFree"`
	OutYear        int                      `json:"outYear"`
	IMDB           string                   `json:"imdb" binding:"required,max=20"`
	AgeRestriction enum.MovieAgeRestriction `json:"ageRestriction"`
}

type UpdateMovieRequest struct {
	Title          string                   `json:"title" binding:"required,max=100"`
	Description    string                   `json:"description" binding:"max=500"`
	Unavailable    bool                     `json:"unavailable"`
	Length         int                      `json:"length"`
	IsFree         bool                     `json:"isFree"`
	OutYear        int                      `json:"outYear"`
	AgeRestriction enum.MovieAgeRestriction `json:"ageRestriction"`
}

type GetMovieByIdResponse struct {
	Id             uuid.UUID `json:"id"`
	IMDB           string    `json:"imdb"`
	Title          string    `json:"title"`
	Description    string    `json:"description"`
	AverageScore   float64   `json:"averageScore"`
	Length         int       `json:"length"`
	IsFree         bool      `json:"isFree"`
	OutYear        int       `json:"outYear"`
	AgeRestriction string    `json:"ageRestriction"`
}

type CreateMovieRateRequest struct {
	Score uint8 `json:"score" binding:"required,min=1,max=10"`
}

// Series DTOs
type CreateSerieRequest struct {
	Title           string                   `json:"title" binding:"required,max=100"`
	Description     string                   `json:"description" binding:"max=500"`
	IMDB            string                   `json:"imdb" binding:"required,max=20"`
	AgeRestriction  enum.SerieAgeRestriction `json:"ageRestriction"`
	Unavailable     bool                     `json:"unavailable"`
	Length          int                      `json:"length"`
	IsFree          bool                     `json:"isFree"`
	FirstSeasonYear int                      `json:"firstSeasonYear"`
	LastSeasonYear  *int                     `json:"lastSeasonYear,omitempty"`
}

type UpdateSerieRequest struct {
	Title           string                   `json:"title" binding:"required,max=100"`
	Description     string                   `json:"description" binding:"max=500"`
	AgeRestriction  enum.SerieAgeRestriction `json:"ageRestriction"`
	Unavailable     bool                     `json:"unavailable"`
	Length          int                      `json:"length"`
	IsFree          bool                     `json:"isFree"`
	FirstSeasonYear int                      `json:"firstSeasonYear"`
	LastSeasonYear  *int                     `json:"lastSeasonYear,omitempty"`
}

type GetSerieByIdResponse struct {
	Id              uuid.UUID `json:"id"`
	IMDB            string    `json:"imdb"`
	Title           string    `json:"title"`
	Description     string    `json:"description"`
	Length          int       `json:"length"`
	IsFree          bool      `json:"isFree"`
	StreamNetwork   *string   `json:"streamNetwork,omitempty"`
	AverageScore    float64   `json:"averageScore"`
	FirstSeasonYear int       `json:"firstSeasonYear"`
	LastSeasonYear  *int      `json:"lastSeasonYear,omitempty"`
}

type CreateSerieRateRequest struct {
	Score uint8 `json:"score" binding:"required,min=1,max=10"`
}

// Groups DTOs
type CreateGroupRequest struct {
	Name              string  `json:"name" binding:"required,max=100"`
	Description       *string `json:"description" binding:"omitempty,max=500"`
	ImageUrl          *string `json:"imageUrl,omitempty"`
	ImageThumbnailUrl *string `json:"imageThumbnailUrl,omitempty"`
	IsPublic          bool    `json:"isPublic"`
}

type UpdateGroupRequest struct {
	Name              string  `json:"name" binding:"required,max=100"`
	Description       *string `json:"description" binding:"omitempty,max=500"`
	ImageUrl          *string `json:"imageUrl,omitempty"`
	ImageThumbnailUrl *string `json:"imageThumbnailUrl,omitempty"`
	IsPublic          bool    `json:"isPublic"`
}

type GetGroupByIdResponse struct {
	Id                uuid.UUID `json:"id"`
	Name              string    `json:"name"`
	Description       *string   `json:"description,omitempty"`
	ImageUrl          *string   `json:"imageUrl,omitempty"`
	ImageThumbnailUrl *string   `json:"imageThumbnailUrl,omitempty"`
}

// Search DTOs
type GetMoviesPaginatedResponse struct {
	Id             uuid.UUID `json:"id"`
	IMDB           string    `json:"imdb"`
	Title          string    `json:"title"`
	Description    string    `json:"description"`
	IsFree         bool      `json:"isFree"`
	OutYear        int       `json:"outYear"`
	AgeRestriction string    `json:"ageRestriction"`
}
