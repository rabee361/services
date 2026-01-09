FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src
COPY ["src/Ecommerce.Admin/Ecommerce.Admin.csproj", "src/Ecommerce.Admin/"]
RUN dotnet restore "src/Ecommerce.Admin/Ecommerce.Admin.csproj"
COPY . .
WORKDIR "/src/src/Ecommerce.Admin"
RUN dotnet build "Ecommerce.Admin.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Ecommerce.Admin.csproj" -c Release -o /app/publish

FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Ecommerce.Admin.dll"]

