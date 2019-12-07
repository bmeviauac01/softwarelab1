FROM mcr.microsoft.com/dotnet/core/runtime:3.1.0-buster-slim as base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/core/sdk:3.1.100-buster AS build
WORKDIR /src
COPY ["mssql/mssql.csproj", "mssql/"]
COPY ["ahk.common/ahk.common.csproj", "ahk.common/"]
COPY ["ahk.adatvez.mssqldb/ahk.adatvez.mssqldb.csproj", "ahk.adatvez.mssqldb/"]
RUN dotnet restore "mssql/mssql.csproj"
COPY ["mssql", "mssql"]
COPY ["ahk.common", "ahk.common"]
COPY ["ahk.adatvez.mssqldb", "ahk.adatvez.mssqldb"]
WORKDIR "/src/mssql"
RUN dotnet build "mssql.csproj" -c Release -o /app

FROM build AS publish
RUN dotnet publish "mssql.csproj" -c Release -o /app

FROM mcr.microsoft.com/dotnet/core/sdk:2.1.802-alpine3.10 AS obfuscate
WORKDIR /app
COPY --from=publish /app .
RUN dotnet tool install --global obfuscar.globaltool --version "2.2.25"
RUN /root/.dotnet/tools/obfuscar.console -s /app/obfuscar.xml

FROM base AS final
WORKDIR /app
COPY --from=publish /app .
COPY --from=obfuscate /obf .
RUN rm /app/obfuscar.xml

CMD dotnet "/app/mssql.dll"
