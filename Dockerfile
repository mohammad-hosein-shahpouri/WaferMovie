FROM golang:1.26-alpine AS builder

WORKDIR /app

# Cache dependencies
COPY go.mod go.sum ./
RUN go mod download

# Copy source code
COPY . .

# Build statically linked binary
RUN CGO_ENABLED=0 GOOS=linux go build -ldflags="-w -s" -o /app/bin/wafer-movie ./cmd/api

FROM alpine:3.21 AS final

RUN apk --no-cache add ca-certificates tzdata curl tini

WORKDIR /app

COPY --from=builder /app/bin/wafer-movie /app/wafer-movie

ENV TZ=Asia/Tehran
ENV PORT=8080
ENV ENVIRONMENT=production

EXPOSE 8080

ENTRYPOINT ["/sbin/tini", "--"]
CMD ["/app/wafer-movie"]

