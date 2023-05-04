FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
LABEL "Author"="Mohammad Hosein Shahpouri"
WORKDIR /app
EXPOSE 8000

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ./Src ./
RUN dotnet restore WaferMovie.Web/*.csproj
RUN dotnet build --no-restore WaferMovie.Web/*.csproj -c Release -o /app/build

FROM build AS publish
RUN apt-get update && curl -sL https://deb.nodesource.com/setup_lts.x | bash - && apt-get install -y nodejs
RUN dotnet publish WaferMovie.Web/*.csproj -c Release -o /app/out

FROM base AS final
WORKDIR /app
HEALTHCHECK --interval=5s --timeout=10s --retries=3 CMD curl --fail http://localhost:8000/healthcheck || exit 1
COPY --from=publish /app/out .
ENV ASPNETCORE_URLS=http://*:8000
ENTRYPOINT ["dotnet", "WaferMovie.Web.dll"]
