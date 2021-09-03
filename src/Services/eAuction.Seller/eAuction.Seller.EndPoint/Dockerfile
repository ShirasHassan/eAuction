#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:3.1 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:3.1 AS build
WORKDIR /src
COPY ["eAuction.Seller.EndPoint/eAuction.Seller.EndPoint.csproj", "eAuction.Seller.EndPoint/"]
RUN dotnet restore "eAuction.Seller.EndPoint/eAuction.Seller.EndPoint.csproj"
COPY . .
WORKDIR "/src/eAuction.Seller.EndPoint"
RUN dotnet build "eAuction.Seller.EndPoint.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "eAuction.Seller.EndPoint.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "eAuction.Seller.EndPoint.dll"]