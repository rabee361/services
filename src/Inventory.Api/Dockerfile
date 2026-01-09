FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY ["src/Inventory.Api/Inventory.Api.csproj", "src/Inventory.Api/"]
COPY ["src/Shared.Contracts/Shared.Contracts.csproj", "src/Shared.Contracts/"]
RUN dotnet restore "src/Inventory.Api/Inventory.Api.csproj"
COPY . .
WORKDIR "/src/src/Inventory.Api"
RUN dotnet build "Inventory.Api.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Inventory.Api.csproj" -c Release -o /app/publish

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Inventory.Api.dll"]

