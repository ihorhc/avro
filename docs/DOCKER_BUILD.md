# Docker Build Guide for Avro MCP Orchestrator

This document provides instructions for building and running the Avro MCP Orchestrator using Docker.

## Quick Start

### Build the Docker Image

```bash
# Build from the repository root
docker build -f src/avro-mcp-orchestrator/Dockerfile -t avro-mcp-orchestrator:latest .

# Or build from the orchestrator directory
cd src/avro-mcp-orchestrator
docker build -t avro-mcp-orchestrator:latest .
```

### Run the Container

```bash
# View status
docker run --rm avro-mcp-orchestrator:latest status

# List servers
docker run --rm avro-mcp-orchestrator:latest list

# Add a server
docker run --rm avro-mcp-orchestrator:latest server add my-server "command" --args "args"

# Start servers
docker run --rm avro-mcp-orchestrator:latest start

# Stop servers
docker run --rm avro-mcp-orchestrator:latest stop
```

## Docker Compose Example

Create a `docker-compose.yml` in the repository root:

```yaml
version: '3.8'

services:
  avro-mcp-orchestrator:
    build:
      context: .
      dockerfile: src/avro-mcp-orchestrator/Dockerfile
    container_name: avro-mcp-orchestrator
    volumes:
      - mcp-config:/home/orchestrator/.avro
      - mcp-logs:/app/logs
    environment:
      - DOTNET_ENVIRONMENT=Production
    restart: unless-stopped
    command: ["start"]

volumes:
  mcp-config:
  mcp-logs:
```

## Advanced Build Options

### Build with Specific .NET Runtime

```bash
# Use .NET 8 instead of 10
docker build --build-arg DOTNET_VERSION=8.0 -t avro-mcp-orchestrator:net8 .
```

### Multi-platform Build (arm64, amd64)

```bash
# Requires buildx
docker buildx build --platform linux/amd64,linux/arm64 -t avro-mcp-orchestrator:latest .
```

### Production Build with Tags

```bash
# Build with version tag
docker build -t avro-mcp-orchestrator:1.0.0 .
docker tag avro-mcp-orchestrator:1.0.0 avro-mcp-orchestrator:latest
```

## Volume Mounting

### Mount Configuration and Logs

```bash
docker run -v /local/path/to/config:/home/orchestrator/.avro \
           -v /local/path/to/logs:/app/logs \
           avro-mcp-orchestrator:latest status
```

### Keep Configuration Persistent

```bash
docker run -d \
  --name mcp-orchestrator \
  -v mcp-config:/home/orchestrator/.avro \
  -v mcp-logs:/app/logs \
  avro-mcp-orchestrator:latest start
```

## Environment Variables

Configure the orchestrator using environment variables:

```bash
docker run -e DOTNET_ENVIRONMENT=Production \
           -e DOTNET_LOG_LEVEL=Information \
           avro-mcp-orchestrator:latest status
```

## Container Networking

### Network Mode

```bash
# Host network (Linux only)
docker run --network host avro-mcp-orchestrator:latest status

# Bridge network (default)
docker run -p 8080:8080 avro-mcp-orchestrator:latest status
```

### Docker Network

```bash
# Create a network for MCP services
docker network create avro-mcp-network

# Run container on network
docker run --network avro-mcp-network \
           --name orchestrator \
           avro-mcp-orchestrator:latest start
```

## Debugging

### View Container Logs

```bash
# Run interactively
docker run -it avro-mcp-orchestrator:latest bash

# View logs
docker logs <container-id>

# Follow logs
docker logs -f <container-id>
```

### Health Check Status

```bash
docker inspect --format='{{.State.Health.Status}}' <container-id>
```

## Optimization

### Image Size Optimization

The Dockerfile uses multi-stage builds to minimize the final image size:
- SDK stage: Used only for compilation
- Runtime stage: Only includes .NET runtime (no build tools)

Current estimated image size: ~200-250 MB (varies by platform)

### Cache Optimization

The build cache is optimized by:
1. Restoring dependencies first
2. Copying source code separately
3. Using specific SDK and runtime versions

## Security Considerations

- **Non-root user**: Container runs as `orchestrator` (UID 1001)
- **Health checks**: Automatic health verification every 30 seconds
- **Minimal surface**: Runtime-only base image eliminates build tools
- **Read-only filesystem**: Can be enforced with `--read-only` flag

## Integration with CI/CD

### GitHub Actions Example

```yaml
name: Build and Push Docker Image

on:
  push:
    branches: [main]
    paths:
      - 'src/avro-mcp-orchestrator/**'

jobs:
  build-and-push:
    runs-on: ubuntu-latest
    steps:
      - uses: actions/checkout@v3
      
      - name: Set up Docker Buildx
        uses: docker/setup-buildx-action@v2
      
      - name: Build and push
        uses: docker/build-push-action@v4
        with:
          context: .
          file: src/avro-mcp-orchestrator/Dockerfile
          push: false
          tags: avro-mcp-orchestrator:latest
```

## Troubleshooting

### Build Fails with "project file not found"

Ensure you're building from the repository root and the path is correct:
```bash
docker build -f src/avro-mcp-orchestrator/Dockerfile -t avro-mcp-orchestrator:latest .
```

### Container Exits Immediately

Check the logs:
```bash
docker run avro-mcp-orchestrator:latest status
docker logs <container-id>
```

### Permission Denied Errors

Ensure volumes are writable and ownership is correct:
```bash
chmod 755 /local/path/to/config
docker run -v /local/path/to/config:/home/orchestrator/.avro avro-mcp-orchestrator:latest status
```

## Resources

- [Docker Documentation](https://docs.docker.com/)
- [.NET Docker Images](https://hub.docker.com/_/microsoft-dotnet)
- [Multi-stage Builds](https://docs.docker.com/build/building/multi-stage/)
