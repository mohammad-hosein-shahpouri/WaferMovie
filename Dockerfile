FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
LABEL "Author"="Mohammad Hosein Shahpouri"
WORKDIR /app
EXPOSE 8000

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
COPY WaferMovie.Web/*.csproj  WaferMovie.Web/
COPY . ./
RUN dotnet restore WaferMovie.Web/*.csproj
RUN dotnet build WaferMovie.Web/*.csproj -c Release -o /app/build

FROM node:alpine as node
WORKDIR /app/WaferMovie.Web/ClientApp
RUN npm install && npm run build

FROM build AS publish
RUN dotnet publish WaferMovie.Web/*.csproj -c Release -o /app/out

FROM base AS final
WORKDIR /app
COPY --from=publish /app/out .
ENV ASPNETCORE_URLS=http://*:8000
ENTRYPOINT ["dotnet", "WaferMovie.Web.dll"]