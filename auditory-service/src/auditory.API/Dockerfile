
FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src
COPY ["Auditory.API.csproj", "./"]
COPY . .
RUN dotnet publish "Auditory.API.csproj" -c Release -o /app/publish

FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS final
WORKDIR /app

ENV ASPNETCORE_URLS=http://+:5006
USER app
EXPOSE 5006

COPY --from=build /app/publish .
ENTRYPOINT ["dotnet", "Auditory.API.dll"]