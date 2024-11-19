# Menggunakan SDK .NET 8 sebagai build environment
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 7000

ENV ASPNETCORE_URLS="http://+:7000"
ENV ASPNETCORE_ENVIRONMENT=Production

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Copy semua project ke dalam container
COPY . .

# Restore dependencies
RUN dotnet restore

# Build project dengan referensinya
RUN dotnet build "NET.Starter.API/NET.Starter.API.csproj" -c Release -o /app/build

FROM build AS publish
# Publish dengan referensi project
RUN dotnet publish "NET.Starter.API/NET.Starter.API.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app

COPY --from=publish /app/publish .

ENTRYPOINT ["dotnet", "NET.Starter.API.dll"]

# run in command prompt
# docker build -t account_name/adeefhelpdesk:1.0 .
# docker push account_name/adeefhelpdesk:1.0