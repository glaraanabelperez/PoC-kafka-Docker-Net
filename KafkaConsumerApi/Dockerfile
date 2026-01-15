
# Etapa 1: Compilación
FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["KafkaConsumerApi.csproj", "."]

RUN dotnet restore "./KafkaConsumerApi.csproj"

COPY . .
ENV NUGET_PACKAGES=/root/.nuget/packages
WORKDIR "/src/."
RUN dotnet build "KafkaConsumerApi.csproj" -c Debug -o /app/build

# Etapa 2: Publicación
FROM build AS publish
RUN dotnet publish "KafkaConsumerApi.csproj" -c Debug -o /app/publish /p:UseAppHost=false

# Etapa 3: Imagen final
FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS final
WORKDIR /app

# Instalar debugger vsdbg (opcional, solo si harás attach manual)
RUN apt-get update \
    && apt-get install -y unzip curl \
    && curl -sSL https://aka.ms/getvsdbgsh | bash /dev/stdin -v latest -l /vsdbg

COPY --from=publish /app/publish .
ENV ASPNETCORE_URLS=http://+:8082
EXPOSE 8082
ENTRYPOINT ["dotnet", "KafkaConsumerApi.dll"]
