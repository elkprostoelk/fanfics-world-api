#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER app
WORKDIR /app
COPY ["fanficsworld.pfx", "."]
EXPOSE 5000
EXPOSE 5001

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["FanficsWorld.WebAPI/FanficsWorld.WebAPI.csproj", "FanficsWorld.WebAPI/"]
COPY ["FanficsWorld.Common/FanficsWorld.Common.csproj", "FanficsWorld.Common/"]
COPY ["FanficsWorld.DataAccess/FanficsWorld.DataAccess.csproj", "FanficsWorld.DataAccess/"]
COPY ["FanficsWorld.Services/FanficsWorld.Services.csproj", "FanficsWorld.Services/"]
RUN dotnet restore "./FanficsWorld.WebAPI/./FanficsWorld.WebAPI.csproj"
COPY . .
WORKDIR "/src/FanficsWorld.WebAPI"
RUN dotnet build "./FanficsWorld.WebAPI.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./FanficsWorld.WebAPI.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "FanficsWorld.WebAPI.dll"]