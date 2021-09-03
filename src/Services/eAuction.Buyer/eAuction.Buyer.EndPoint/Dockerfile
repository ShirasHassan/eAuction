#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:3.1 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:3.1 AS build
WORKDIR /src
COPY ["eAuction.Buyer.EndPoint/eAuction.Buyer.EndPoint.csproj", "eAuction.Buyer.EndPoint/"]
RUN dotnet restore "eAuction.Buyer.EndPoint/eAuction.Buyer.EndPoint.csproj"
COPY . .
WORKDIR "/src/eAuction.Buyer.EndPoint"
RUN dotnet build "eAuction.Buyer.EndPoint.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "eAuction.Buyer.EndPoint.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "eAuction.Buyer.EndPoint.dll"]