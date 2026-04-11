FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src

COPY ["word.API/Word.API.csproj", "word.API/"]
COPY ["Word.Application/Word.Application.csproj", "Word.Application/"]
COPY ["Word.Domain/Word.Domain.csproj", "Word.Domain/"]
COPY ["Word.Infrastructure/Word.Infrastructure.csproj", "Word.Infrastructure/"]
RUN dotnet restore "word.API/Word.API.csproj"

COPY . .
WORKDIR /src/word.API
RUN dotnet publish "Word.API.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS final
WORKDIR /app
COPY --from=build /app/publish .

ENV ASPNETCORE_ENVIRONMENT=Production
ENV ASPNETCORE_URLS=http://+:8080
EXPOSE 8080

ENTRYPOINT ["dotnet", "Word.API.dll"]
