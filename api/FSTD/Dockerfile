# Base image with ASP.NET Core runtime
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER app
WORKDIR /app
EXPOSE 8080

# Build stage with .NET SDK
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src

# Copy all project files to their respective directories
COPY api/FSTD/src/FSTD.API/FSTD.API.csproj FSTD.API/
COPY api/FSTD/src/FSTD.DataCore/FSTD.DataCore.csproj FSTD.DataCore/
COPY api/FSTD/src/FSTD.Application/FSTD.Application.csproj FSTD.Application/
COPY api/FSTD/src/FSTD.Infrastructure/FSTD.Infrastructure.csproj FSTD.Infrastructure/
COPY helpers/ErrorsHandler/src/FSTD.ExceptionsHandler/FSTD.ExceptionsHandler.csproj /helpers/ErrorsHandler/src/FSTD.ExceptionsHandler/
COPY helpers/ErrorsHandler/src/FSTD.Exeptions.Models/FSTD.Exeptions.Models.csproj /helpers/ErrorsHandler/src/FSTD.Exeptions.Models/

# Restore dependencies for all projects
RUN dotnet restore FSTD.API/FSTD.API.csproj

# Copy all source files
COPY api/FSTD/src/FSTD.API/ FSTD.API/
COPY api/FSTD/src/FSTD.DataCore/ FSTD.DataCore/
COPY api/FSTD/src/FSTD.Application/ FSTD.Application/
COPY api/FSTD/src/FSTD.Infrastructure/ FSTD.Infrastructure/
COPY helpers/ErrorsHandler/src/FSTD.ExceptionsHandler/ /helpers/ErrorsHandler/src/FSTD.ExceptionsHandler/
COPY helpers/ErrorsHandler/src/FSTD.Exeptions.Models/ /helpers/ErrorsHandler/src/FSTD.Exeptions.Models/

# Build the project
WORKDIR /src/FSTD.API
RUN dotnet build FSTD.API.csproj -c $BUILD_CONFIGURATION -o /app/build

# Publish the project
FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "FSTD.API.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

# Final stage with runtime
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "FSTD.API.dll"]