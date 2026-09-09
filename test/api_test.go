package test

import (
	"bytes"
	"encoding/json"
	"fmt"
	"net/http"
	"net/http/httptest"
	"testing"

	"wafer-movie/internal/domain/dto"
	"wafer-movie/internal/domain/enum"
	"wafer-movie/pkg/pagination"
	"wafer-movie/pkg/response"

	"github.com/google/uuid"
	"github.com/stretchr/testify/assert"
	"github.com/stretchr/testify/require"
)

func performRequest(r http.Handler, method, path string, body any, headers map[string]string) *httptest.ResponseRecorder {
	var reqBody []byte
	if body != nil {
		reqBody, _ = json.Marshal(body)
	}
	req, _ := http.NewRequest(method, path, bytes.NewBuffer(reqBody))
	req.Header.Set("Content-Type", "application/json")
	for k, v := range headers {
		req.Header.Set(k, v)
	}
	w := httptest.NewRecorder()
	r.ServeHTTP(w, req)
	return w
}

func TestHealthCheck(t *testing.T) {
	fixture := SetupTestFixture(t)
	w := performRequest(fixture.Engine, "GET", "/ready", nil, nil)
	assert.Equal(t, http.StatusOK, w.Code)
}

func TestUserAndAccountFlow(t *testing.T) {
	fixture := SetupTestFixture(t)

	// 1. Password mismatch validation
	badUserReq := dto.CreateUserRequest{
		Name:                 "Test User",
		Email:                "test@example.com",
		PhoneNumber:          "1234567890",
		UserName:             "testuser",
		Password:             "StrongP@ss1",
		PasswordConfirmation: "DifferentP@ss1",
	}
	w := performRequest(fixture.Engine, "POST", "/api/v1/Users", badUserReq, nil)
	// Should fail with 401 Unauthorized because /api/v1/Users requires auth!
	assert.Equal(t, http.StatusUnauthorized, w.Code)

	// Create an admin token to call /api/v1/Users
	adminId := uuid.New()
	adminToken, err := fixture.TokenService.Generate(adminId, "admin", "admin@example.com", "stamp", nil)
	require.NoError(t, err)

	authHeader := map[string]string{"Authorization": "Bearer " + adminToken}

	// 2. Weak password
	weakPassReq := badUserReq
	weakPassReq.Password = "weak"
	weakPassReq.PasswordConfirmation = "weak"
	w = performRequest(fixture.Engine, "POST", "/api/v1/Users", weakPassReq, authHeader)
	assert.Equal(t, 406, w.Code)

	// 3. Password mismatch with auth
	w = performRequest(fixture.Engine, "POST", "/api/v1/Users", badUserReq, authHeader)
	assert.Equal(t, 406, w.Code)

	// 4. Successful user creation
	validUserReq := dto.CreateUserRequest{
		Name:                 "Alice Smith",
		Email:                "alice@example.com",
		PhoneNumber:          "+1234567890",
		UserName:             "alicesmith",
		Password:             "SecurePass123!",
		PasswordConfirmation: "SecurePass123!",
	}
	w = performRequest(fixture.Engine, "POST", "/api/v1/Users", validUserReq, authHeader)
	assert.Equal(t, http.StatusOK, w.Code)

	var createResp response.ApiResponse[uuid.UUID]
	err = json.Unmarshal(w.Body.Bytes(), &createResp)
	require.NoError(t, err)
	assert.True(t, createResp.Succeeded)
	userId := createResp.Data
	assert.NotEqual(t, uuid.Nil, userId)

	// 5. Duplicate email should fail
	w = performRequest(fixture.Engine, "POST", "/api/v1/Users", validUserReq, authHeader)
	assert.Equal(t, 406, w.Code)

	// 6. Login with wrong password
	badLoginReq := dto.LoginRequest{
		Email:    "alice@example.com",
		Password: "WrongPassword!",
	}
	w = performRequest(fixture.Engine, "POST", "/api/v1/Accounts/Login", badLoginReq, nil)
	assert.Equal(t, http.StatusBadRequest, w.Code)

	// 7. Login with correct password
	loginReq := dto.LoginRequest{
		Email:    "alice@example.com",
		Password: "SecurePass123!",
	}
	w = performRequest(fixture.Engine, "POST", "/api/v1/Accounts/Login", loginReq, nil)
	assert.Equal(t, http.StatusOK, w.Code)

	var loginResp response.ApiResponse[dto.LoginResponse]
	err = json.Unmarshal(w.Body.Bytes(), &loginResp)
	require.NoError(t, err)
	assert.True(t, loginResp.Succeeded)
	assert.Equal(t, "Alice Smith", loginResp.Data.Name)
	assert.NotEmpty(t, loginResp.Data.Token)
	userToken := loginResp.Data.Token

	userAuthHeader := map[string]string{"Authorization": "Bearer " + userToken}

	// 8. Get current user
	w = performRequest(fixture.Engine, "GET", "/api/v1/Accounts", nil, userAuthHeader)
	assert.Equal(t, http.StatusOK, w.Code)

	var currentResp response.ApiResponse[dto.GetCurrentUserResponse]
	err = json.Unmarshal(w.Body.Bytes(), &currentResp)
	require.NoError(t, err)
	assert.Equal(t, userId, currentResp.Data.Id)
	assert.Equal(t, "alice@example.com", currentResp.Data.Email)

	// 9. Get active sessions
	w = performRequest(fixture.Engine, "GET", "/api/v1/Accounts/Sessions", nil, userAuthHeader)
	assert.Equal(t, http.StatusOK, w.Code)

	var sessionsResp response.ApiResponse[[]dto.UserSessionResponse]
	err = json.Unmarshal(w.Body.Bytes(), &sessionsResp)
	require.NoError(t, err)
	assert.True(t, sessionsResp.Succeeded)
	require.NotEmpty(t, sessionsResp.Data)
	sessionId := sessionsResp.Data[0].Id

	// 10. Revoke active session
	w = performRequest(fixture.Engine, "DELETE", fmt.Sprintf("/api/v1/Accounts/Sessions/%s", sessionId), nil, userAuthHeader)
	assert.Equal(t, http.StatusOK, w.Code)
}

func TestMovieLifecycle(t *testing.T) {
	fixture := SetupTestFixture(t)

	// User token for rating
	uid := uuid.New()
	userToken, err := fixture.TokenService.Generate(uid, "movie_fan", "fan@example.com", "stamp", nil)
	require.NoError(t, err)
	authHeader := map[string]string{"Authorization": "Bearer " + userToken}

	// 1. Create Movie
	createReq := dto.CreateMovieRequest{
		Title:          "Inception",
		Description:    "A thief who steals corporate secrets through dream-sharing technology.",
		Unavailable:    false,
		Length:         148,
		IsFree:         true,
		OutYear:        2010,
		IMDB:           "tt1375666",
		AgeRestriction: enum.MovieAgePG13,
	}
	w := performRequest(fixture.Engine, "POST", "/api/v1/Movies", createReq, nil)
	assert.Equal(t, http.StatusOK, w.Code)

	var createResp response.ApiResponse[uuid.UUID]
	err = json.Unmarshal(w.Body.Bytes(), &createResp)
	require.NoError(t, err)
	movieId := createResp.Data
	assert.NotEqual(t, uuid.Nil, movieId)

	// 2. Restrict Duplicate IMDB
	w = performRequest(fixture.Engine, "POST", "/api/v1/Movies", createReq, nil)
	assert.Equal(t, 406, w.Code)

	// 3. Get Movie by ID
	w = performRequest(fixture.Engine, "GET", fmt.Sprintf("/api/v1/Movies/%s", movieId), nil, nil)
	assert.Equal(t, http.StatusOK, w.Code)
	var getResp response.ApiResponse[dto.GetMovieByIdResponse]
	err = json.Unmarshal(w.Body.Bytes(), &getResp)
	require.NoError(t, err)
	assert.Equal(t, "Inception", getResp.Data.Title)
	assert.Equal(t, "PG-13", getResp.Data.AgeRestriction)
	assert.Equal(t, float64(0), getResp.Data.AverageScore)

	// 4. Rate Movie
	rateReq := dto.CreateMovieRateRequest{Score: 9}
	w = performRequest(fixture.Engine, "POST", fmt.Sprintf("/api/v1/Movies/%s/Rate", movieId), rateReq, authHeader)
	assert.Equal(t, http.StatusOK, w.Code)

	// 5. Get Movie again, verify average score is 9
	w = performRequest(fixture.Engine, "GET", fmt.Sprintf("/api/v1/Movies/%s", movieId), nil, nil)
	assert.Equal(t, http.StatusOK, w.Code)
	err = json.Unmarshal(w.Body.Bytes(), &getResp)
	require.NoError(t, err)
	assert.Equal(t, float64(9), getResp.Data.AverageScore)

	// 6. Update Movie
	updateReq := dto.UpdateMovieRequest{
		Title:          "Inception (Updated)",
		Description:    "Updated description",
		Unavailable:    true,
		Length:         150,
		IsFree:         false,
		OutYear:        2010,
		AgeRestriction: enum.MovieAgePG13,
	}
	w = performRequest(fixture.Engine, "PUT", fmt.Sprintf("/api/v1/Movies/%s", movieId), updateReq, nil)
	assert.Equal(t, http.StatusOK, w.Code)

	// 7. Delete Movie
	w = performRequest(fixture.Engine, "DELETE", fmt.Sprintf("/api/v1/Movies/%s", movieId), nil, nil)
	assert.Equal(t, http.StatusOK, w.Code)

	// 8. Verify NotFound after deletion
	w = performRequest(fixture.Engine, "GET", fmt.Sprintf("/api/v1/Movies/%s", movieId), nil, nil)
	assert.Equal(t, http.StatusNotFound, w.Code)
}

func TestSerieLifecycle(t *testing.T) {
	fixture := SetupTestFixture(t)

	uid := uuid.New()
	userToken, err := fixture.TokenService.Generate(uid, "serie_fan", "serie_fan@example.com", "stamp", nil)
	require.NoError(t, err)
	authHeader := map[string]string{"Authorization": "Bearer " + userToken}

	// 1. Create Serie
	lastYear := 2013
	createReq := dto.CreateSerieRequest{
		Title:           "Breaking Bad",
		Description:     "A chemistry teacher diagnosed with cancer turns to manufacturing methamphetamine.",
		IMDB:            "tt0903747",
		AgeRestriction:  enum.SerieAgeTVMA,
		Unavailable:     false,
		Length:          49,
		IsFree:          false,
		FirstSeasonYear: 2008,
		LastSeasonYear:  &lastYear,
	}
	w := performRequest(fixture.Engine, "POST", "/api/v1/Series", createReq, nil)
	assert.Equal(t, http.StatusOK, w.Code)

	var createResp response.ApiResponse[uuid.UUID]
	err = json.Unmarshal(w.Body.Bytes(), &createResp)
	require.NoError(t, err)
	serieId := createResp.Data
	assert.NotEqual(t, uuid.Nil, serieId)

	// 2. Duplicate IMDB
	w = performRequest(fixture.Engine, "POST", "/api/v1/Series", createReq, nil)
	assert.Equal(t, 406, w.Code)

	// 3. Get Serie by ID
	w = performRequest(fixture.Engine, "GET", fmt.Sprintf("/api/v1/Series/%s", serieId), nil, nil)
	assert.Equal(t, http.StatusOK, w.Code)
	var getResp response.ApiResponse[dto.GetSerieByIdResponse]
	err = json.Unmarshal(w.Body.Bytes(), &getResp)
	require.NoError(t, err)
	assert.Equal(t, "Breaking Bad", getResp.Data.Title)

	// 4. Rate Serie
	rateReq := dto.CreateSerieRateRequest{Score: 10}
	w = performRequest(fixture.Engine, "POST", fmt.Sprintf("/api/v1/Series/%s/Rate", serieId), rateReq, authHeader)
	assert.Equal(t, http.StatusOK, w.Code)

	// 5. Update Serie
	updateReq := dto.UpdateSerieRequest{
		Title:           "Breaking Bad (Updated)",
		Description:     "Updated description",
		AgeRestriction:  enum.SerieAgeTVMA,
		Unavailable:     false,
		Length:          50,
		IsFree:          true,
		FirstSeasonYear: 2008,
	}
	w = performRequest(fixture.Engine, "PUT", fmt.Sprintf("/api/v1/Series/%s", serieId), updateReq, nil)
	assert.Equal(t, http.StatusOK, w.Code)

	// 6. Delete Serie
	w = performRequest(fixture.Engine, "DELETE", fmt.Sprintf("/api/v1/Series/%s", serieId), nil, nil)
	assert.Equal(t, http.StatusOK, w.Code)
}

func TestGroupLifecycle(t *testing.T) {
	fixture := SetupTestFixture(t)

	desc := "Top 100 Action movies of all time"
	createReq := dto.CreateGroupRequest{
		Name:        "Action Classics",
		Description: &desc,
		IsPublic:    true,
	}
	w := performRequest(fixture.Engine, "POST", "/api/v1/Groups", createReq, nil)
	assert.Equal(t, http.StatusOK, w.Code)

	var createResp response.ApiResponse[uuid.UUID]
	err := json.Unmarshal(w.Body.Bytes(), &createResp)
	require.NoError(t, err)
	groupId := createResp.Data

	// Get Group
	w = performRequest(fixture.Engine, "GET", fmt.Sprintf("/api/v1/Groups/%s", groupId), nil, nil)
	assert.Equal(t, http.StatusOK, w.Code)

	var getResp response.ApiResponse[dto.GetGroupByIdResponse]
	err = json.Unmarshal(w.Body.Bytes(), &getResp)
	require.NoError(t, err)
	assert.Equal(t, "Action Classics", getResp.Data.Name)

	// Soft Delete Group
	w = performRequest(fixture.Engine, "DELETE", fmt.Sprintf("/api/v1/Groups/%s", groupId), nil, nil)
	assert.Equal(t, http.StatusOK, w.Code)

	// Verify not found after soft deletion
	w = performRequest(fixture.Engine, "GET", fmt.Sprintf("/api/v1/Groups/%s", groupId), nil, nil)
	assert.Equal(t, http.StatusNotFound, w.Code)
}

func TestSearchAndPagination(t *testing.T) {
	fixture := SetupTestFixture(t)

	// Seed multiple movies
	movies := []dto.CreateMovieRequest{
		{Title: "The Dark Knight", IMDB: "tt0468569", OutYear: 2008, IsFree: true, Length: 152, AgeRestriction: enum.MovieAgePG13},
		{Title: "The Dark Knight Rises", IMDB: "tt1345836", OutYear: 2012, IsFree: false, Length: 164, AgeRestriction: enum.MovieAgePG13},
		{Title: "Interstellar", IMDB: "tt0816692", OutYear: 2014, IsFree: true, Length: 169, AgeRestriction: enum.MovieAgePG13},
		{Title: "Memento", IMDB: "tt0209144", OutYear: 2000, IsFree: false, Length: 113, AgeRestriction: enum.MovieAgeR},
	}
	for _, m := range movies {
		performRequest(fixture.Engine, "POST", "/api/v1/Movies", m, nil)
	}

	// 1. Search all movies paginated
	searchReq := pagination.PaginationInput{
		PageNumber: 1,
		PageSize:   2,
		Order: pagination.SortModel{
			ColumnName: "outYear",
			Ascending:  false, // Descending: 2014, 2012, 2008, 2000
		},
	}
	w := performRequest(fixture.Engine, "POST", "/api/v1/Search/Movies", searchReq, nil)
	assert.Equal(t, http.StatusOK, w.Code)

	var searchResp response.ApiResponse[pagination.PaginationOutput[dto.GetMoviesPaginatedResponse]]
	err := json.Unmarshal(w.Body.Bytes(), &searchResp)
	require.NoError(t, err)
	assert.Equal(t, int64(4), searchResp.Data.TotalCount)
	assert.Equal(t, 2, searchResp.Data.TotalPages)
	assert.Equal(t, 2, len(searchResp.Data.Items))
	assert.Equal(t, "Interstellar", searchResp.Data.Items[0].Title)
	assert.Equal(t, "The Dark Knight Rises", searchResp.Data.Items[1].Title)

	// 2. Search with Contains filter: Title contains "Dark"
	searchWithFilter := pagination.PaginationInput{
		PageNumber: 1,
		PageSize:   10,
		SearchObjects: []pagination.SearchModelRequest{
			{
				Key:      "title",
				Operator: enum.Contains,
				Value:    "Dark",
			},
		},
	}
	w = performRequest(fixture.Engine, "POST", "/api/v1/Search/Movies", searchWithFilter, nil)
	assert.Equal(t, http.StatusOK, w.Code)
	err = json.Unmarshal(w.Body.Bytes(), &searchResp)
	require.NoError(t, err)
	assert.Equal(t, int64(2), searchResp.Data.TotalCount)
	assert.Equal(t, 2, len(searchResp.Data.Items))
}

func TestLocalization(t *testing.T) {
	fixture := SetupTestFixture(t)

	// Request with Persian language header
	headers := map[string]string{"Accept-Language": "fa-IR"}
	w := performRequest(fixture.Engine, "GET", "/api/v1/Movies/00000000-0000-0000-0000-000000000000", nil, headers)
	assert.Equal(t, http.StatusNotFound, w.Code)
	assert.Equal(t, "fa-IR", w.Header().Get("Content-Language"))

	var notFoundResp response.ApiResponse[any]
	err := json.Unmarshal(w.Body.Bytes(), &notFoundResp)
	require.NoError(t, err)
	// Should contain Persian localized message for not found: "{0} یافت نشد"
	assert.Contains(t, notFoundResp.Messages[0], "یافت نشد")
}
