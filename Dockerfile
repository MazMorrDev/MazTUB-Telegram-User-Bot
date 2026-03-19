# Etapa 1: Build
FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src

# Copiar el archivo de proyecto y restaurar dependencias
COPY ["WorkFilterBot.csproj", "."]
RUN dotnet restore "WorkFilterBot.csproj"

# Copiar todo el código y compilar
COPY . .
RUN dotnet publish "WorkFilterBot.csproj" -c Release -o /app/publish

# Etapa 2: Runtime
FROM mcr.microsoft.com/dotnet/runtime:10.0
WORKDIR /app

# Instalar dependencias necesarias
RUN apt-get update && apt-get install -y \
    ca-certificates \
    && rm -rf /var/lib/apt/lists/*

# Copiar la aplicación compilada
COPY --from=build /app/publish .

# Crear directorio para la sesión
RUN mkdir -p /app/session

# Variable de entorno para la sesión
ENV WTelegram_session=/app/session/WTelegram.session

# Ejecutar la aplicación
ENTRYPOINT ["dotnet", "WorkFilterBot.dll"]