FROM mcr.microsoft.com/dotnet/core/aspnet:2.2-stretch-slim AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/core/sdk:2.2-stretch AS build
WORKDIR /src
COPY ["src/gateways/web.bff.shopping/Web.Bff.Shopping.Api/Web.Bff.Shopping.Api.csproj", "src/gateways/web.bff.shopping/Web.Bff.Shopping.Api/"]
RUN dotnet restore "src/gateways/web.bff.shopping/Web.Bff.Shopping.Api/Web.Bff.Shopping.Api.csproj"
COPY . .
WORKDIR "/src/src/gateways/web.bff.shopping/Web.Bff.Shopping.Api"
RUN dotnet build "Web.Bff.Shopping.Api.csproj" -c Release -o /app

FROM build AS publish
RUN dotnet publish "Web.Bff.Shopping.Api.csproj" -c Release -o /app

FROM base AS final
WORKDIR /app
COPY --from=publish /app .
ENTRYPOINT ["dotnet", "Web.Bff.Shopping.Api.dll"]