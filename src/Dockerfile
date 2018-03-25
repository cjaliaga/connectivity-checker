FROM microsoft/aspnetcore:2.0 AS base
WORKDIR /app
EXPOSE 80

FROM microsoft/aspnetcore-build:2.0 AS build
WORKDIR /src
COPY ConnectivityChecker.sln ./
COPY ConnectivityChecker/ConnectivityChecker.csproj ConnectivityChecker/
COPY ConnectivityChecker.Core/ConnectivityChecker.Core.csproj ConnectivityChecker.Core/
COPY ConnectivityChecker.Contracts/ConnectivityChecker.Contracts.csproj ConnectivityChecker.Contracts/
RUN dotnet restore -nowarn:msb3202,nu1503
COPY . .
WORKDIR /src/ConnectivityChecker
RUN dotnet build -c Release -o /app

FROM build AS publish
RUN dotnet publish -c Release -o /app

FROM base AS final
WORKDIR /app
COPY --from=publish /app .
ENTRYPOINT ["dotnet", "ConnectivityChecker.dll"]
