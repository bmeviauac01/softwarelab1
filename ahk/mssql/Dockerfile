FROM mcr.microsoft.com/dotnet/core/runtime:2.1.14-alpine3.10 as base
# https://github.com/dotnet/SqlClient/issues/81#issuecomment-545214765
# https://github.com/dotnet/SqlClient/issues/81#issuecomment-545214765
RUN apk add --no-cache icu-libs
ENV DOTNET_SYSTEM_GLOBALIZATION_INVARIANT=false
WORKDIR /app

FROM mcr.microsoft.com/dotnet/core/sdk:2.1.802-alpine3.10 AS build
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

FROM base AS final
WORKDIR /app
COPY --from=publish /app .

CMD dotnet "/app/mssql.dll"
