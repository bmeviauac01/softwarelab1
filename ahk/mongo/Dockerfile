FROM mcr.microsoft.com/dotnet/core/aspnet:2.1.14-stretch-slim as base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/core/sdk:2.1.607-stretch AS build
RUN dotnet tool install --global obfuscar.globaltool --version "2.2.25"
WORKDIR /src
COPY ["mongo/mongo.csproj", "mongo/"]
COPY ["ahk.common/ahk.common.csproj", "ahk.common/"]
RUN dotnet restore "mongo/mongo.csproj"
COPY ["mongo", "mongo"]
COPY ["ahk.common", "ahk.common"]
WORKDIR "/src/mongo"
RUN dotnet build "mongo.csproj" -c Release --no-restore

FROM build AS publish
RUN dotnet publish "mongo.csproj" -c Release -o /app --no-build
COPY ["mongo/obfuscar.xml", "."]
RUN /root/.dotnet/tools/obfuscar.console -s obfuscar.xml

FROM base AS final
WORKDIR /app
COPY --from=publish /app .
COPY --from=publish /obf .

CMD cp ./api/bin/Debug/netcoreapp2.1/MongoLabor.dll /app || (echo "Solution application not found - Nem talalhato a megoldas alkalmazas"; exit 1) \
	&& dotnet "/app/mongo.dll"
