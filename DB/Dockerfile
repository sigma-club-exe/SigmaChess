FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER $APP_UID
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["SigmaChess/SigmaChess.csproj", "SigmaChess/"]
COPY ["SigmaChess.Contracts/SigmaChess.Contracts.csproj", "SigmaChess.Contracts/"]
COPY ["SigmaChess.Core/SigmaChess.Core.csproj", "SigmaChess.Core/"]
COPY ["SigmaChess.DataAccess/SigmaChess.DataAccess.csproj", "SigmaChess.DataAccess/"]
RUN dotnet restore "SigmaChess/SigmaChess.csproj"
COPY . .
WORKDIR "/src/SigmaChess"
RUN dotnet build "SigmaChess.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "SigmaChess.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "SigmaChess.dll"]
