FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src
COPY ["src/Ecommerce.Web/Ecommerce.Web.csproj", "src/Ecommerce.Web/"]
RUN dotnet restore "src/Ecommerce.Web/Ecommerce.Web.csproj"
COPY . .
WORKDIR "/src/src/Ecommerce.Web"
RUN dotnet build "Ecommerce.Web.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Ecommerce.Web.csproj" -c Release -o /app/publish

FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Ecommerce.Web.dll"]

