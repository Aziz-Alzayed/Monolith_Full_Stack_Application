# Base image with Azure Functions runtime for isolated mode
FROM mcr.microsoft.com/azure-functions/dotnet-isolated:4-dotnet-isolated8.0 AS base
WORKDIR /home/site/wwwroot
EXPOSE 8080

# Build stage
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src

# Copy all project files to their respective directories
COPY services/FSTD.TimerTriggers/FSTD.SeedResetTrigger/FSTD.SeedResetTrigger.csproj FSTD.SeedResetTrigger/

# Restore dependencies for all projects
RUN dotnet restore FSTD.SeedResetTrigger/FSTD.SeedResetTrigger.csproj

# Copy all source files
COPY services/FSTD.TimerTriggers/FSTD.SeedResetTrigger/ FSTD.SeedResetTrigger/

# Build the project
WORKDIR "/src/FSTD.SeedResetTrigger"
RUN dotnet build "FSTD.SeedResetTrigger.csproj" -c $BUILD_CONFIGURATION -o /app/build

# Publish the project
FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./FSTD.SeedResetTrigger.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /home/site/wwwroot
COPY --from=publish /app/publish .
ENV AzureWebJobsScriptRoot=/home/site/wwwroot \
    AzureFunctionsJobHost__Logging__Console__IsEnabled=true