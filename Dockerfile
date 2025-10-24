# syntax=docker/dockerfile:1

# Build stage: restore dependencies, compile, and publish the application
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src

# Copy the csproj file and restore as distinct layers
COPY ["AntonHateBot/AntonHateBot.csproj", "AntonHateBot/"]
RUN dotnet restore "AntonHateBot/AntonHateBot.csproj"

# Copy the remaining source code and publish the application
COPY . .
WORKDIR /src/AntonHateBot
RUN dotnet publish "AntonHateBot.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

# Runtime stage: use the smaller runtime image
FROM mcr.microsoft.com/dotnet/runtime:9.0 AS final
WORKDIR /app

# Copy the published output from the build stage
COPY --from=build /app/publish .

# Expose the entry point for the bot
ENTRYPOINT ["dotnet", "AntonHateBot.dll"]
