# Etapa 1: Imagem base de runtime (mais leve)
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
# Expõe a porta 8080 (usada pelo Kestrel)
EXPOSE 8080

# Define a variável para o ASP.NET escutar em todas as interfaces
ENV ASPNETCORE_URLS=http://+:8080
ENV ASPNETCORE_ENVIRONMENT=Production

# Etapa 2: Build da aplicação
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Copia o csproj e restaura as dependências (para cache eficiente)
COPY ["src/PayFlow/PayFlow.csproj", "src/PayFlow/"]
RUN dotnet restore "src/PayFlow/PayFlow.csproj"

# Copia o restante do código e publica
COPY . .
WORKDIR "/src/src/PayFlow"
RUN dotnet publish "PayFlow.csproj" -c Release -o /app/publish /p:UseAppHost=false

# Etapa 3: Imagem final
FROM base AS final
WORKDIR /app
COPY --from=build /app/publish .
EXPOSE 8080
ENTRYPOINT ["dotnet", "PayFlow.dll"]
