# Build Stage
FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src

# Copiar csproj e restaurar dependências
COPY ["PessoaAPI.csproj", "./"]
RUN dotnet restore "PessoaAPI.csproj"

# Copiar todo o código e fazer build
COPY . .
RUN dotnet build "PessoaAPI.csproj" -c Release -o /app/build

# Publish Stage
FROM build AS publish
RUN dotnet publish "PessoaAPI.csproj" -c Release -o /app/publish

# Runtime Stage
FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS final
WORKDIR /app
EXPOSE 80
EXPOSE 443

# Copiar arquivos publicados
COPY --from=publish /app/publish .

# Variáveis de ambiente
ENV ASPNETCORE_URLS=http://+:80
ENV ASPNETCORE_ENVIRONMENT=Production
ENV DOTNET_RUNNING_IN_CONTAINER=true

ENTRYPOINT ["dotnet", "PessoaAPI.dll"]

