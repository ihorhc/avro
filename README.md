# AVRO - AI-First Monorepo SDLC Platform

Enterprise-scale AI-driven development lifecycle platform designed for high-velocity teams building with and alongside AI.

## Overview

AVRO is a comprehensive monorepo platform that orchestrates 100+ interconnected projects and 5K+ structured tasks. It combines a powerful C#/.NET 8 polyglot backend with a modern shadcn/TypeScript frontend, all optimized for AI agent workflows.

## Architecture

### Monorepo Structure

```
avro/
├── backend/              # C#/.NET 8 backend services
│   ├── core/            # Core platform services
│   ├── services/        # Microservices
│   └── shared/          # Shared libraries
├── frontend/            # TypeScript/React frontend
│   ├── apps/           # Frontend applications
│   └── packages/       # Shared UI components
├── ai/                 # AI agent configurations
│   ├── agents/         # Agent definitions
│   ├── context/        # Context optimization
│   └── workflows/      # Autonomous workflows
├── projects/           # Project configurations
├── tasks/              # Task registry
└── docs/               # Documentation
```

## Key Features

- **AI-Optimized Architecture**: Standardized structure for AI agent navigation
- **Task Registry**: Centralized management of 5K+ structured tasks
- **Cross-Project Dependencies**: Intelligent dependency tracking and resolution
- **Autonomous Workflows**: Issue-to-PR automation with human oversight
- **Enterprise Scale**: Supports 100+ interconnected projects
- **Polyglot Backend**: C#/.NET 8 with extensibility for other languages
- **Modern Frontend**: shadcn UI components with TypeScript

## Getting Started

### Prerequisites

- .NET 8.0 SDK or later
- Node.js 20+ and pnpm
- Docker (for containerized services)

### Backend Setup

```bash
cd backend
dotnet restore
dotnet build
dotnet test
```

### Frontend Setup

```bash
cd frontend
pnpm install
pnpm dev
```

### AI Agent Setup

The platform is pre-configured for AI agents with:
- Standardized project structure
- Task registry with metadata
- Dependency graphs
- Workflow templates

See [AI Agent Guide](docs/ai-agents.md) for details.

## Project Configuration

Projects are defined in `projects/` with standardized metadata:
- Dependencies
- Task assignments
- Build configurations
- AI context hints

## Task Management

Tasks are registered in `tasks/registry.json` with:
- Unique identifiers
- Project associations
- Dependencies
- Priority levels
- AI execution metadata

## Development

### Adding a New Project

1. Create project directory under appropriate category
2. Add project configuration to `projects/`
3. Register in dependency graph
4. Update task registry

### Creating Tasks

1. Add task definition to registry
2. Link to project
3. Define dependencies
4. Add AI context metadata

## Architecture Principles

1. **Standardization**: Consistent patterns across all projects
2. **Modularity**: Loosely coupled, highly cohesive components
3. **AI-First**: Optimized for autonomous agent workflows
4. **Human Oversight**: Critical decisions require approval
5. **High Velocity**: Fast iteration with quality gates

## Contributing

See [CONTRIBUTING.md](CONTRIBUTING.md) for guidelines.

## License

MIT License - see [LICENSE](LICENSE) for details.

## Support

- Documentation: [docs/](docs/)
- Issues: GitHub Issues
- Discussions: GitHub Discussions