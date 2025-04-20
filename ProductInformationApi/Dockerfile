# Use the official .NET SDK image to build and publish
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

# Use the SDK image to build the app
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY ["ProductInformationApi/ProductInformationApi.csproj", "ProductInformationApi/"]
RUN dotnet restore "ProductInformationApi/ProductInformationApi.csproj"
COPY . .
WORKDIR "/src/ProductInformationApi"
RUN dotnet build "ProductInformationApi.csproj" -c Release -o /app/build
RUN dotnet publish "ProductInformationApi.csproj" -c Release -o /app/publish

# Final runtime image
FROM base AS final
WORKDIR /app
COPY --from=build /app/publish .
ENTRYPOINT ["dotnet", "ProductInformationApi.dll"]