FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY ["src/Users.Api/Users.Api.csproj", "src/Users.Api/"]
COPY ["src/Shared.Contracts/Shared.Contracts.csproj", "src/Shared.Contracts/"]
RUN dotnet restore "src/Users.Api/Users.Api.csproj"
COPY . .
WORKDIR "/src/src/Users.Api"
RUN dotnet build "Users.Api.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Users.Api.csproj" -c Release -o /app/publish

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS final
WORKDIR /app
COPY --from=publish /app/publish .
COPY src/Users.Api/seed.sql .
ENTRYPOINT ["dotnet", "Users.Api.dll"]

