# Consulte https://aka.ms/customizecontainer para aprender a personalizar su contenedor de depuración y cómo Visual Studio usa este Dockerfile para compilar sus imágenes para una depuración más rápida.

# Esta fase se usa cuando se ejecuta desde VS en modo rápido (valor predeterminado para la configuración de depuración)
FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443


# Esta fase se usa para compilar el proyecto de servicio
FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["auth0rize.auth.api/auth0rize.auth.api.csproj", "auth0rize.auth.api/"]
COPY ["auth0rize.auth.application/auth0rize.auth.application.csproj", "auth0rize.auth.application/"]
COPY ["auth0rize.auth.domain/auth0rize.auth.domain.csproj", "auth0rize.auth.domain/"]
COPY ["auth0rize.auth.notification/auth0rize.auth.notification.csproj", "auth0rize.auth.notification/"]
COPY ["auth0rize.auth.infraestructure/auth0rize.auth.infraestructure.csproj", "auth0rize.auth.infraestructure/"]
RUN dotnet restore "./auth0rize.auth.api/auth0rize.auth.api.csproj"
COPY . .
WORKDIR "/src/auth0rize.auth.api"
RUN dotnet build "./auth0rize.auth.api.csproj" -c $BUILD_CONFIGURATION -o /app/build

# Esta fase se usa para publicar el proyecto de servicio que se copiará en la fase final.
FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./auth0rize.auth.api.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

# Esta fase se usa en producción o cuando se ejecuta desde VS en modo normal (valor predeterminado cuando no se usa la configuración de depuración)
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "auth0rize.auth.api.dll"]