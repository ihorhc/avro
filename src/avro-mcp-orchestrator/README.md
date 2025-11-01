# Avro MCP Orchestrator

A CLI-based orchestrator for managing Model Context Protocol (MCP) server instances. This tool provides comprehensive lifecycle management for MCP servers including start, stop, monitoring, and configuration.

## Features

- **Server Management**: Add, start, stop, and monitor MCP servers
- **Configuration**: Persistent configuration storage in JSON format
- **Process Monitoring**: View status, PID, and uptime for running servers
- **Auto-start**: Configure servers to start automatically
- **Logging**: Comprehensive structured logging with Serilog
- **Rich CLI**: Beautiful command-line interface with Spectre.Console

## Installation

```bash
dotnet build
dotnet publish -c Release
```

## Usage

### Add a Server

```bash
./avro-mcp-orchestrator server add <name> <command> [options]

# Example
./avro-mcp-orchestrator server add my-server "node" --args "index.js" --timeout 30
```

Options:
- `-a, --args` - Command arguments
- `-w, --working-dir` - Working directory for the server
- `-t, --timeout` - Server timeout in seconds (1-300)

### List Servers

```bash
./avro-mcp-orchestrator list
```

### Start Servers

```bash
# Start a specific server
./avro-mcp-orchestrator start <server-name>

# Start all auto-start servers
./avro-mcp-orchestrator start
```

### Stop Servers

```bash
# Stop a specific server
./avro-mcp-orchestrator stop <server-name>

# Stop all running servers
./avro-mcp-orchestrator stop
```

### View Status

```bash
./avro-mcp-orchestrator status
```

### View Configuration

```bash
./avro-mcp-orchestrator config show
```

### Global Options

- `-c, --config` - Path to configuration file (default: `~/.avro/mcp-config.json`)
- `-l, --log-level` - Logging level (default: Information)
- `-v, --verbose` - Enable verbose output

## Configuration

Server configurations are stored in JSON format at `~/.avro/mcp-config.json`:

```json
[
  {
    "name": "my-server",
    "command": "node",
    "arguments": "index.js",
    "workingDirectory": "/path/to/server",
    "timeoutSeconds": 30,
    "autoStart": true,
    "environment": {
      "NODE_ENV": "production"
    }
  }
]
```

## Logging

Logs are written to:
- Console (based on log level)
- File: `logs/mcp-orchestrator-YYYY-MM-DD.txt` (daily rolling)

## Architecture

- **Program.cs**: Command-line interface definition and setup
- **McpOrchestrator.cs**: Core orchestrator logic and server instance management
- **ServerConfig**: Data model for server configuration
- **McpServerInstance**: Process wrapper for individual server instances

## Development

Follow the Avro platform guidelines:
- Use modern C# features and nullable reference types
- Implement proper error handling and logging
- Follow SOLID principles
- Use async/await for all I/O operations

## Examples

### Add a Python MCP Server

```bash
./avro-mcp-orchestrator server add python-server "python3" --args "server.py" --working-dir /home/user/mcp-servers
```

### Add a Node.js MCP Server with Auto-start

```bash
./avro-mcp-orchestrator server add node-server "node" --args "dist/index.js" --timeout 60
```

### Monitor All Servers

```bash
./avro-mcp-orchestrator status
```

## Contributing

All code should follow the Avro platform coding standards. See `.github/instructions` for detailed guidelines.
