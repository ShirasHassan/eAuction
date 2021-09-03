#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:3.1 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:3.1 AS build
WORKDIR /src
COPY ["eAuction.AuctionBC.EndPoint/eAuction.AuctionBC.EndPoint.csproj", "eAuction.AuctionBC.EndPoint/"]
RUN dotnet restore "eAuction.AuctionBC.EndPoint/eAuction.AuctionBC.EndPoint.csproj"
COPY . .
WORKDIR "/src/eAuction.AuctionBC.EndPoint"
RUN dotnet build "eAuction.AuctionBC.EndPoint.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "eAuction.AuctionBC.EndPoint.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "eAuction.AuctionBC.EndPoint.dll"]