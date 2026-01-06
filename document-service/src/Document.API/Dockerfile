FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src

# copiar csproj
COPY document-service/src/Document.API/Document.API.csproj document-service/src/Document.API/
COPY RabbitMq/RabbitMq.csproj RabbitMq/

# restore limpio (LINUX)
RUN dotnet restore document-service/src/Document.API/Document.API.csproj --no-cache

# copiar código
COPY document-service/src/Document.API/ document-service/src/Document.API/
COPY RabbitMq/ RabbitMq/

WORKDIR /src/document-service/src/Document.API

# publish (SIN --no-restore)
RUN dotnet publish -c Release -o /app/publish

FROM mcr.microsoft.com/dotnet/aspnet:10.0
WORKDIR /app
COPY --from=build /app/publish .
ENTRYPOINT ["dotnet", "Document.API.dll"]