#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["cens.auth.api/cens.auth.api.csproj", "cens.auth.api/"]
COPY ["cens.auth.application/cens.auth.application.csproj", "cens.auth.application/"]
COPY ["cens.auth.domain/cens.auth.domain.csproj", "cens.auth.domain/"]
COPY ["cens.auth.notification/cens.auth.notification.csproj", "cens.auth.notification/"]
COPY ["cens.auth.infraestructure/cens.auth.infraestructure.csproj", "cens.auth.infraestructure/"]
RUN dotnet restore "cens.auth.api/cens.auth.api.csproj"
COPY . .
WORKDIR "/src/cens.auth.api"
RUN dotnet build "cens.auth.api.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "cens.auth.api.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "cens.auth.api.dll"]