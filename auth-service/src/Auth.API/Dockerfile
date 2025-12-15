FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src

COPY Auth.API.csproj .
RUN dotnet restore
COPY . .

RUN dotnet publish -c Release -o /app/publish --no-restore

FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS final
WORKDIR /app
COPY --from=build /app/publish .

EXPOSE 8083
EXPOSE 8084

ENTRYPOINT ["dotnet", "Auth.API.dll"]