FROM docker.pkg.github.com/bmeviauac01/ahk-ellenorzo-base-images/mssql-dotnet:2017.CU14-2.1 AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/core/sdk:2.1.802-bionic AS build
WORKDIR /src
COPY ["mssql/mssql.csproj", "mssql/"]
COPY ["ahkcommon/ahkcommon.csproj", "ahkcommon/"]
RUN dotnet restore "mssql/mssql.csproj"
COPY ["mssql", "mssql"]
COPY ["ahkcommon", "ahkcommon"]
WORKDIR "/src/mssql"
RUN dotnet build "mssql.csproj" -c Release -o /app

FROM build AS publish
RUN dotnet publish "mssql.csproj" -c Release -o /app

FROM base AS final
WORKDIR /app
COPY --from=publish /app .

# Elinditja az MSSQL szervert es futtatja a teszteket, majd leall
CMD echo "SQL Server indul..." \
    && ( /opt/mssql/bin/sqlservr --accept-eula & ) | grep -q "Service Broker manager has started" \
    && echo "SQL Server elindult." \
    && dotnet "mssql.dll" \
    && pkill sqlservr

# AHK console logger kulcs
ENV AHK_TEST_APP_CONSOLE_LOGGER_KEY=Ah3L9eR2