FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
COPY ["Server/app.db", "/app/"]
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["Server/Respect.Server.csproj", "Server/"]
RUN dotnet restore "Server/Respect.Server.csproj"
COPY . .
WORKDIR "/src/Server"
RUN dotnet build "Respect.Server.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Respect.Server.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Respect.Server.dll"]