FROM mcr.microsoft.com/dotnet/core/aspnet:2.1.14-stretch-slim as base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/core/sdk:2.1.607-stretch AS build
RUN dotnet tool install --global obfuscar.globaltool --version "2.2.25"
WORKDIR /src
COPY ["efrest/evalapp/evalapp.csproj", "efrest/evalapp/"]
COPY ["ahk.common/ahk.common.csproj", "ahk.common/"]
RUN dotnet restore "efrest/evalapp/evalapp.csproj"
COPY ["efrest", "efrest"]
COPY ["ahk.common", "ahk.common"]
WORKDIR "/src/efrest/evalapp"
RUN dotnet build "evalapp.csproj" -c Release -o /app

FROM build AS publish
RUN dotnet publish "evalapp.csproj" -c Release -o /app
RUN /root/.dotnet/tools/obfuscar.console -s /app/obfuscar.xml

FROM base AS final
WORKDIR /app
COPY --from=publish /app .
COPY --from=publish /obf .
RUN rm /app/obfuscar.xml

CMD cp ./api/bin/Debug/netcoreapp2.1/api.dll /app || (echo "Solution application not found - Nem talalhato a megoldas alkalmazas"; exit 1) \
	&& dotnet "/app/efrest.dll"
