FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src
COPY global.json ./
COPY FabricPlatform.API/FabricPlatform.API.csproj FabricPlatform.API/
RUN dotnet restore FabricPlatform.API/FabricPlatform.API.csproj
COPY . .
RUN dotnet publish FabricPlatform.API/FabricPlatform.API.csproj -c Release -o /app --no-restore

FROM mcr.microsoft.com/dotnet/aspnet:10.0
WORKDIR /app
COPY --from=build /app .
ENV ASPNETCORE_ENVIRONMENT=Production
CMD ["sh", "-c", "ASPNETCORE_URLS=http://0.0.0.0:${PORT:-8080} dotnet FabricPlatform.API.dll"]
