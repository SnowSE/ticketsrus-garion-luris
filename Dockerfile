FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
RUN apt-get update && apt-get install -y wget 
WORKDIR /app 

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY ["TicketsRUs.WebApp/TicketsRUs.WebApp.csproj", "web/"]
RUN dotnet restore "web/TicketsRUs.WebApp.csproj"

WORKDIR "/src/web"
COPY . .
RUN dotnet build "TicketsRUs.WebApp/TicketsRUs.WebApp.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "TicketsRUs.WebApp/TicketsRUs.WebApp.csproj" -c Release -o /app/publish

FROM base as final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT [ "dotnet", "TicketsRUs.WebApp.dll" ]