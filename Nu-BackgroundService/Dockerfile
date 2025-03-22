FROM mcr.microsoft.com/dotnet/runtime:8.0 AS base
USER $APP_UID
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["Nu-Cache/Nu-Cache.csproj", "Nu-Cache/"]
RUN dotnet restore "Nu-Cache/Nu-Cache.csproj"
COPY . .
WORKDIR "/src/Nu-Cache"
RUN dotnet build "Nu-Cache.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "Nu-Cache.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Nu-Cache.dll"]
