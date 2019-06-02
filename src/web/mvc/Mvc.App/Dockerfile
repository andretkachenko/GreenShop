FROM mcr.microsoft.com/dotnet/core/aspnet:2.2-stretch-slim AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/core/sdk:2.2-stretch AS build
WORKDIR /src
COPY ["src/web/mvc/MVC.App/Mvc.App.csproj", "src/web/mvc/MVC.App/"]
RUN dotnet restore "src/web/mvc/MVC.App/Mvc.App.csproj"
COPY . .
WORKDIR "/src/src/web/mvc/MVC.App"
RUN dotnet build "Mvc.App.csproj" -c Release -o /app

FROM build AS publish
RUN dotnet publish "Mvc.App.csproj" -c Release -o /app

FROM base AS final
WORKDIR /app
COPY --from=publish /app .
ENTRYPOINT ["dotnet", "Mvc.App.dll"]