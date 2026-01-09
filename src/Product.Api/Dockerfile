FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY ["src/Product.Api/Product.Api.csproj", "src/Product.Api/"]
COPY ["src/Shared.Contracts/Shared.Contracts.csproj", "src/Shared.Contracts/"]
RUN dotnet restore "src/Product.Api/Product.Api.csproj"
COPY . .
WORKDIR "/src/src/Product.Api"
RUN dotnet build "Product.Api.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Product.Api.csproj" -c Release -o /app/publish

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS final
WORKDIR /app
COPY --from=publish /app/publish .
COPY src/Product.Api/wwwroot ./wwwroot
ENTRYPOINT ["dotnet", "Product.Api.dll"]

