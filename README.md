# Avro Platform

A modern .NET microservices platform implementing Clean Architecture, CQRS, and Domain-Driven Design patterns with an AI-powered development pipeline.

## 🚀 Features

- **Clean Architecture**: Clear separation of concerns with Domain, Application, and Infrastructure layers
- **CQRS Pattern**: Command/Query separation using MediatR
- **Event-Driven**: Domain events drive cross-service communication
- **Multi-Tenancy**: Built-in tenant isolation at all levels
- **AI-Powered Development**: Automated feature implementation using GitHub Copilot agents

## 🤖 AI-Powered Development Pipeline

This repository features a fully autonomous AI-driven SDLC that automatically processes issues and generates complete implementations.

**Simply add the `ai-ready` label to an issue**, and the AI agents will:
- ✅ Analyze architecture and validate design
- ✅ Write production code following platform standards
- ✅ Create comprehensive test suites
- ✅ Configure CI/CD and infrastructure
- ✅ Review code for quality, security, and performance
- ✅ Create a pull request ready for merge

**[Learn more about the AI Pipeline →](.github/AI_PIPELINE.md)**

## 🏗️ Architecture

The platform follows a layered architecture pattern:

```
src/
├── Avro.{Service}.Abstractions/    # Interfaces & contracts
├── Avro.{Service}.Domain/          # Business logic & aggregates
├── Avro.{Service}.Application/     # Commands, queries, handlers
├── Avro.{Service}.Infrastructure/  # Data access & integrations
├── Avro.{Service}.WebApi/          # HTTP API endpoints
├── Avro.{Service}.EventHandlers/   # Event consumers
└── Avro.{Service}.Workers/         # Background jobs
```

## 🛠️ Technology Stack

- **.NET 10**: Modern C# with nullable reference types
- **Entity Framework Core**: Database access with multi-tenancy
- **MediatR**: CQRS implementation
- **AWS**: Cloud infrastructure (ECS/Fargate, RDS, Lambda)
- **GitHub Copilot**: AI-powered development agents

## 📋 Getting Started

### Prerequisites

- .NET 10 SDK
- Docker (optional, for local development)
- AWS CLI (for deployment)

### Build

```bash
dotnet restore
dotnet build
```

### Test

```bash
dotnet test
```

### Run

```bash
dotnet run --project src/Avro.Mcp.Orchestrator
```

## 🧪 Testing

The platform maintains >80% code coverage with:
- **Unit Tests**: Business logic and domain validation
- **Integration Tests**: Database and service integration
- **E2E Tests**: Complete feature workflows

## 📚 Documentation

- [AI Pipeline Guide](.github/AI_PIPELINE.md) - Complete guide to AI-powered development
- [Copilot Instructions](.github/copilot-instructions.md) - Coding standards and guidelines
- [Architecture Patterns](.github/instructions/) - Detailed architecture documentation
- [Security Guidelines](.github/prompts/security-audit.prompt.md) - Security best practices

## 🔒 Security

Security is built into every layer:
- Automated vulnerability scanning
- Code quality checks
- OWASP compliance validation
- Multi-tenancy isolation

## 🤝 Contributing

We use an AI-powered development workflow:

1. **Create an issue** with detailed requirements
2. **Add the `ai-ready` label**
3. **AI agents process** and create a PR
4. **Review and merge** the generated code

For manual contributions, follow the coding standards in `.github/copilot-instructions.md`.

## 📄 License

See [LICENSE](LICENSE) file for details.

---

**Powered by GitHub Copilot** 🤖