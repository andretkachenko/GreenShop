FROM mcr.microsoft.com/dotnet/core/aspnet:2.2-stretch-slim AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/core/sdk:2.2-stretch AS build
WORKDIR /src
COPY ["src/services/catalog/Catalog.Api/Catalog.Api.csproj", "src/services/catalog/Catalog.Api/"]
RUN dotnet restore "src/services/catalog/Catalog.Api/Catalog.Api.csproj"
COPY . .
WORKDIR "/src/src/services/catalog/Catalog.Api"
RUN dotnet build "Catalog.Api.csproj" -c Release -o /app

FROM build AS publish
RUN dotnet publish "Catalog.Api.csproj" -c Release -o /app

FROM base AS final
WORKDIR /app
COPY --from=publish /app .
ENTRYPOINT ["dotnet", "Catalog.Api.dll"]