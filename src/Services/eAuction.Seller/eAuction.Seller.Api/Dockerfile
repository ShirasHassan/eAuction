#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:3.1 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:3.1 AS build
WORKDIR /src
COPY ["eAuction.Seller.Api/eAuction.Seller.Api.csproj", "eAuction.Seller.Api/"]
RUN dotnet restore "eAuction.Seller.Api/eAuction.Seller.Api.csproj"
COPY . .
WORKDIR "/src/eAuction.Seller.Api"
RUN dotnet build "eAuction.Seller.Api.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "eAuction.Seller.Api.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "eAuction.Seller.Api.dll"]