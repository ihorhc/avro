# Multi-stage build for Avro MCP Orchestrator
# Stage 1: Build stage
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS builder

WORKDIR /src

# Copy all project files for layered architecture
COPY ["src/Avro.Mcp.Abstractions/Avro.Mcp.Abstractions.csproj", "Avro.Mcp.Abstractions/"]
COPY ["src/Avro.Mcp.Domain/Avro.Mcp.Domain.csproj", "Avro.Mcp.Domain/"]
COPY ["src/Avro.Mcp.Infrastructure/Avro.Mcp.Infrastructure.csproj", "Avro.Mcp.Infrastructure/"]
COPY ["src/Avro.Mcp.Application/Avro.Mcp.Application.csproj", "Avro.Mcp.Application/"]
COPY ["src/Avro.Mcp.Orchestrator/Avro.Mcp.Orchestrator.csproj", "Avro.Mcp.Orchestrator/"]
COPY ["Avro.sln", "./"]
COPY ["NuGet.config", "./"]

# Restore dependencies
RUN dotnet restore "Avro.sln"

# Copy source code
COPY . .

# Build and publish the application
RUN dotnet publish -c Release -o /app/publish --no-restore "src/Avro.Mcp.Orchestrator/Avro.Mcp.Orchestrator.csproj"

# Stage 2: Runtime stage
FROM mcr.microsoft.com/dotnet/runtime:9.0 AS runtime

WORKDIR /app

# Create logs directory
RUN mkdir -p /app/logs

# Copy published application from builder
COPY --from=builder /app/publish .

# Create non-root user for security
RUN useradd -m -u 1001 orchestrator && \
    chown -R orchestrator:orchestrator /app

USER orchestrator

# Health check
HEALTHCHECK --interval=30s --timeout=10s --start-period=5s --retries=3 \
    CMD dotnet Avro.Mcp.Orchestrator.dll status || exit 1

# Default command
ENTRYPOINT ["dotnet", "Avro.Mcp.Orchestrator.dll"]
CMD ["status"]
