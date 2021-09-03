#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:3.1 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:3.1 AS build
WORKDIR /src
COPY ["eAuction.Buyer.Api/eAuction.Buyer.Api.csproj", "eAuction.Buyer.Api/"]
RUN dotnet restore "eAuction.Buyer.Api/eAuction.Buyer.Api.csproj"
COPY . .
WORKDIR "/src/eAuction.Buyer.Api"
RUN dotnet build "eAuction.Buyer.Api.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "eAuction.Buyer.Api.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "eAuction.Buyer.Api.dll"]