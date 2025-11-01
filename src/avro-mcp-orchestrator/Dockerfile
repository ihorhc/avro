# Multi-stage build for Avro MCP Orchestrator
# Stage 1: Build stage
FROM mcr.microsoft.com/dotnet/sdk:10.0 AS builder

WORKDIR /src

# Copy the solution and project files
COPY ["./Avro.Mcp.Orchestrator.csproj", "./"]

# Restore dependencies
RUN dotnet restore "Avro.Mcp.Orchestrator.csproj"

# Copy source code
COPY . .

# Build and publish the application
RUN dotnet publish -c Release -o /app/publish --no-restore

# Stage 2: Runtime stage
FROM mcr.microsoft.com/dotnet/runtime:10.0 AS runtime

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
