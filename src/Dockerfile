FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS base
WORKDIR /app
ENV ASPNETCORE_ENVIRONMENT=Development
EXPOSE 80
EXPOSE 8080

FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src
COPY ["Roborally.webapi/Roborally.webapi.csproj", "Roborally.webapi/"]
RUN dotnet restore "Roborally.webapi/Roborally.webapi.csproj"
COPY . .
WORKDIR "/src/Roborally.webapi"
RUN dotnet build "Roborally.webapi.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Roborally.webapi.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
# Copy game board JSON files
COPY ["core/Roborally.core.domain/Game/Gameboard/maps/", "/app/maps/"]

ENTRYPOINT ["dotnet", "Roborally.webapi.dll"]