FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER $APP_UID
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["BankMore.Tariffs.Web/BankMore.Tariffs.Web.csproj", "BankMore.Tariffs.Web/"]
RUN dotnet restore "BankMore.Tariffs.Web/BankMore.Tariffs.Web.csproj"
COPY . .
WORKDIR "/src/BankMore.Tariffs.Web"
RUN dotnet build "./BankMore.Tariffs.Web.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./BankMore.Tariffs.Web.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "BankMore.Tariffs.Web.dll"]
