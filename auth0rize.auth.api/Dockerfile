#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["auth0rize.auth.api/auth0rize.auth.api.csproj", "auth0rize.auth.api/"]
COPY ["auth0rize.auth.application/auth0rize.auth.application.csproj", "auth0rize.auth.application/"]
COPY ["auth0rize.auth.domain/auth0rize.auth.domain.csproj", "auth0rize.auth.domain/"]
COPY ["auth0rize.auth.infraestructure/auth0rize.auth.infraestructure.csproj", "auth0rize.auth.infraestructure/"]
RUN dotnet restore "auth0rize.auth.api/auth0rize.auth.api.csproj"
COPY . .
WORKDIR "/src/auth0rize.auth.api"
RUN dotnet build "auth0rize.auth.api.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "auth0rize.auth.api.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "auth0rize.auth.api.dll"]