# ============================================================
# MS-EHRLogger — Dockerfile Multi-Stage (.NET 8)
# ============================================================

# Stage 1: Build
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS builder
WORKDIR /src

COPY MsEhrLogger.csproj .
RUN dotnet restore

COPY . .
RUN dotnet publish -c Release -o /app/publish --no-restore

# ── Stage 2: Runtime (imagen mínima) ──────────────────────────────────────
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS runtime

LABEL maintainer="medipass-team@medipass.com"
LABEL service="ms-ehrlogger"
LABEL version="1.0.0"

WORKDIR /app

# Usuario no-root
RUN addgroup --system appgroup && adduser --system --ingroup appgroup appuser
COPY --from=builder /app/publish .
RUN chown -R appuser:appgroup /app
USER appuser

ENV ASPNETCORE_ENVIRONMENT=Docker
ENV ASPNETCORE_URLS=http://0.0.0.0:8083

EXPOSE 8083

HEALTHCHECK --interval=30s --timeout=10s --start-period=40s --retries=3 \
    CMD curl -f http://localhost:8083/health || exit 1

ENTRYPOINT ["dotnet", "MsEhrLogger.dll"]
