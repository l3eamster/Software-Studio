FROM mcr.microsoft.com/dotnet/core/sdk:3.1 AS build
WORKDIR /build

COPY . .
RUN dotnet restore
RUN dotnet publish -c Release -o /app
COPY DonutzStudio.db /app


FROM mcr.microsoft.com/dotnet/core/aspnet:3.1 AS final
RUN apt update && apt install -y netcat
WORKDIR /app
COPY --from=build /app .
ENTRYPOINT ["dotnet", "DonutzStudio.dll"]