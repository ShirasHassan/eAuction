#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:3.1 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:3.1 AS build
WORKDIR /src
COPY ["eAuction.AuctionBC.Api/eAuction.AuctionBC.Api.csproj", "eAuction.AuctionBC.Api/"]
RUN dotnet restore "eAuction.AuctionBC.Api/eAuction.AuctionBC.Api.csproj"
COPY . .
WORKDIR "/src/eAuction.AuctionBC.Api"
RUN dotnet build "eAuction.AuctionBC.Api.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "eAuction.AuctionBC.Api.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "eAuction.AuctionBC.Api.dll"]