# See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

# This stage is used when running from VS in fast mode (Default for Debug configuration)
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER app
WORKDIR /app
EXPOSE 8080

# This stage is used to build the service project
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["Fase03.Api/Fase03.Api.csproj", "./Fase03.Api/"]
COPY ["Fase03.Infra.IoC/Fase03.Infra.IoC.csproj", "./Fase03.Infra.IoC/"]
COPY ["Fase03.Application/Fase03.Application.csproj", "./Fase03.Application/"]
COPY ["Fase03.Domain/Fase03.Domain.csproj", "./Fase03.Domain/"]
COPY ["Fase03.Domain.Core/Fase03.Domain.Core.csproj", "./Fase03.Domain.Core/"]
COPY . .
WORKDIR "/src/Fase03.Api"
RUN dotnet build "./Fase03.Api.csproj" -c $BUILD_CONFIGURATION -o /app/build

# This stage is used to publish the service project to be copied to the final stage
FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./Fase03.Api.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

# This stage is used in production or when running from VS in regular mode (Default when not using the Debug configuration)
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Fase03.Api.dll"]