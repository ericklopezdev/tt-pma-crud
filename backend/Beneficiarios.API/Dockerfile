FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY ["Beneficiarios.API/Beneficiarios.API.csproj", "Beneficiarios.API/"]
COPY ["Beneficiarios.Application/Beneficiarios.Application.csproj", "Beneficiarios.Application/"]
COPY ["Beneficiarios.Domain/Beneficiarios.Domain.csproj", "Beneficiarios.Domain/"]
COPY ["Beneficiarios.Infrastructure/Beneficiarios.Infrastructure.csproj", "Beneficiarios.Infrastructure/"]
RUN dotnet restore "./Beneficiarios.API/Beneficiarios.API.csproj"
COPY . .
WORKDIR "/src/Beneficiarios.API"
RUN dotnet build "./Beneficiarios.API.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "./Beneficiarios.API.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Beneficiarios.API.dll"]