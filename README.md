# WaferMovie

WaferMovie is a high-performance backend API service for movie and TV series catalog management, streaming/download links, ratings, user sessions, and dynamic search, written in **Golang**.

## Tech Stack

- **Language**: Go 1.26
- **Web Framework**: [Gin](https://github.com/gin-gonic/gin)
- **Database / ORM**: PostgreSQL with [GORM](https://gorm.io)
- **Cache**: Redis with [go-redis/v9](https://github.com/redis/go-redis)
- **Authentication**: JWT with [golang-jwt/v5](https://github.com/golang-jwt/jwt)
- **Identifiers**: UUID v7 with [google/uuid](https://github.com/google/uuid)
- **Password Security**: Bcrypt with backward-compatible ASP.NET Identity V3 PBKDF2 hash verification
- **Containerization**: Multi-stage Alpine Docker image and Docker Compose

## Architecture & Project Layout

```
wafer-movie/
├── cmd/
│   └── api/
│       └── main.go               # Application entrypoint & graceful shutdown
├── internal/
│   ├── app/                      # Gin router & dependency wire-up
│   ├── config/                   # Configuration loader (env vars & defaults)
│   ├── domain/
│   │   ├── dto/                  # Request & Response Data Transfer Objects
│   │   ├── entity/               # GORM entities (User, Movie, Serie, Group, etc.)
│   │   └── enum/                 # Domain enums (Gender, Age Restrictions, etc.)
│   ├── handler/                  # HTTP route handlers
│   ├── middleware/               # Auth (JWT), Localization, CORS, Recovery
│   ├── repository/
│   │   ├── postgres/             # PostgreSQL connection & AutoMigrate
│   │   └── redis/                # Redis caching & memory cache fallback
│   └── service/                  # Business logic (Accounts, Users, Movies, Series, Groups, Search)
├── pkg/
│   ├── hasher/                   # Password hashing (bcrypt + ASP.NET Identity v3)
│   ├── i18n/                     # Internationalization (en-US, fa-IR)
│   ├── pagination/               # Dynamic column filter & pagination engine
│   ├── response/                 # Standardized ApiResponse format
│   └── token/                    # JWT token generator & validator
├── test/                         # Integration test suite (SQLite in-memory)
├── compose.yml                   # Docker Compose specification
└── Dockerfile                    # Multi-stage production container build
```

## API Endpoints

All API endpoints follow the `/api/v1` prefix and return standard `ApiResponse` objects.

### Accounts (`/api/v1/Accounts`)
- `POST /api/v1/Accounts/Login`: Authenticate with email & password, create session, return JWT token.
- `GET /api/v1/Accounts`: Get authenticated user profile (cached in Redis).
- `GET /api/v1/Accounts/Sessions`: List active sessions for the current user.
- `DELETE /api/v1/Accounts/Sessions/:id`: Revoke an active session.

### Users (`/api/v1/Users`) - *Requires Authentication*
- `POST /api/v1/Users`: Register a new user with password complexity rules and duplicate checks.
- `PUT /api/v1/Users/:id`: Update user profile information.
- `DELETE /api/v1/Users/:id`: Delete user.

### Movies (`/api/v1/Movies`)
- `GET /api/v1/Movies/:id`: Get movie details and average rating score (cached in Redis).
- `POST /api/v1/Movies`: Create a movie (enforces unique IMDB code).
- `PUT /api/v1/Movies/:id`: Update movie details.
- `DELETE /api/v1/Movies/:id`: Delete a movie.
- `POST /api/v1/Movies/:id/Rate`: Submit a rating (1-10) for a movie (*Requires Authentication*).

### Series (`/api/v1/Series`)
- `GET /api/v1/Series/:id`: Get series details and average rating score (cached in Redis).
- `POST /api/v1/Series`: Create a TV series (enforces unique IMDB code).
- `PUT /api/v1/Series/:id`: Update TV series details.
- `DELETE /api/v1/Series/:id`: Delete TV series.
- `POST /api/v1/Series/:id/Rate`: Submit a rating (1-10) for a series (*Requires Authentication*).

### Groups (`/api/v1/Groups`)
- `GET /api/v1/Groups/:id`: Get group details.
- `POST /api/v1/Groups`: Create a collection group.
- `PUT /api/v1/Groups/:id`: Update group details.
- `DELETE /api/v1/Groups/:id`: Soft delete a group.

### Search (`/api/v1/Search`)
- `POST /api/v1/Search`: Dynamic paginated search over movies.
- `POST /api/v1/Search/Movies`: Dynamic paginated search over movies with column filters and ordering.
- `POST /api/v1/Search/Series`: Dynamic paginated search over series with column filters and ordering.

### Health Probe
- `GET /ready`: Health check verifying PostgreSQL and Redis statuses.

## API Documentation (Swagger & Scalar)

Interactive API documentation and client testing playgrounds are available directly in your browser:

- **Scalar UI (Modern API Reference)**: [http://localhost:8080/scalar](http://localhost:8080/scalar) or [http://localhost:8080/docs](http://localhost:8080/docs)
- **Swagger UI**: [http://localhost:8080/swagger](http://localhost:8080/swagger)
- **Raw OpenAPI 3.0 Spec**: [http://localhost:8080/docs/openapi.json](http://localhost:8080/docs/openapi.json)

Both interfaces support interactive request execution and JWT Bearer authorization (`Bearer <token>`).

## Localization

Pass the `Accept-Language` header to receive localized messages:
- `en-US` (Default)
- `fa-IR` (Persian)

The API responds with a matching `Content-Language` header.

## Running the Application

### Using Docker Compose (Recommended)

Start the entire stack (API, PostgreSQL, Redis) with a single command:

```bash
docker compose up --build -d
```

The API will be available at `http://localhost:8080`.

### Running Locally

Ensure PostgreSQL and Redis are running, then:

```bash
# Set environment variables if needed
export POSTGRES_HOST=127.0.0.1
export POSTGRES_PORT=5432
export POSTGRES_USER=postgres
export POSTGRES_PASSWORD=postgres
export POSTGRES_DB=WaferMovie.App
export REDIS_ADDR=127.0.0.1:6379

# Run the API
go run ./cmd/api
```

### Running Tests

Run the complete unit and integration test suite:

```bash
go test -v ./...
```

