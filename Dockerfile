FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["src/BambikiBackend.Api/BambikiBackend.Api.csproj", "src/BambikiBackend.Api/"]
COPY ["src/BambikiBackend.AI/BambikiBackend.AI.csproj", "src/BambikiBackend.AI/"]
RUN dotnet restore "src/BambikiBackend.Api/BambikiBackend.Api.csproj"
COPY . .
WORKDIR "/src/src/BambikiBackend.Api"
RUN dotnet build "BambikiBackend.Api.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "BambikiBackend.Api.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "BambikiBackend.Api.dll"]
