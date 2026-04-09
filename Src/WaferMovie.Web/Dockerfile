ARG DOTNET_VERSION=10.0

FROM mcr.microsoft.com/dotnet/aspnet:${DOTNET_VERSION}-noble AS base
LABEL "Author"="Mohammad Hosein Shahpouri"
WORKDIR /app
#RUN apk add --no-cache nano curl tzdata
EXPOSE 8080/TCP
EXPOSE 8443/TCP

FROM mcr.microsoft.com/dotnet/sdk:${DOTNET_VERSION}-noble AS build
WORKDIR /src
COPY ./Src /src
RUN --mount=type=cache,id=nuget,target=/root/.nuget/packages \
    dotnet restore WaferMovie.Web/*.csproj
RUN --mount=type=cache,id=nuget,target=/root/.nuget/packages \
    dotnet build --no-restore WaferMovie.Web/*.csproj -c Release -o /app/build

FROM build AS publish
RUN apk add --no-cache nodejs npm
RUN --mount=type=cache,id=nuget,target=/root/.nuget/packages \
    dotnet publish WaferMovie.Web/*.csproj -c Release -o /app/out

FROM base AS final
WORKDIR /app
COPY --from=publish /app/out .
ENTRYPOINT ["dotnet", "WaferMovie.Web.dll"]
