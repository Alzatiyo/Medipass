# Etapa de compilación
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build-env
WORKDIR /app

# Copiar archivos .csproj y restaurar dependencias
COPY Domain/Domain.csproj Domain/
COPY Aplication/Aplication.csproj Aplication/
COPY Infrastructure/Infrastructure.csproj Infrastructure/
COPY Api/Api.csproj Api/
RUN dotnet restore Api/Api.csproj

# Copiar el resto de los archivos y publicar la aplicación
COPY . ./
RUN dotnet publish Api/Api.csproj -c Release -o out

# Etapa de ejecución (runtime)
FROM mcr.microsoft.com/dotnet/aspnet:8.0
WORKDIR /app
COPY --from=build-env /app/out .

# Exponer el puerto configurado y definir variable de ASP.NET
EXPOSE 5002
ENV ASPNETCORE_URLS=http://+:5002

ENTRYPOINT ["dotnet", "Api.dll"]
