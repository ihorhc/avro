# Avro Platform – Copilot Project Instructions

## AI Development Workflow

This repository uses an AI-powered development pipeline for automated feature implementation.

### How It Works

1. **Create Issue** with label `ai-ready` and detailed requirements
2. **AI Pipeline Triggers** automatically via GitHub Actions
3. **Parallel Agent Execution**:
   - **Architect Agent**: Validates design and creates implementation strategy
   - **Implementation Agent**: Writes production code following approved architecture
   - **Testing Agent**: Creates comprehensive test suites in parallel
   - **DevOps Agent**: Sets up CI/CD and infrastructure configuration
   - **Review Agent**: Validates code quality, security, and compliance
4. **Auto-PR Creation**: Pull request generated with all changes
5. **Review & Merge**: Human review or auto-merge based on quality gates

### Agent Context

All agents work within the Avro platform context:

- **Monorepo Structure**: .NET backend + TypeScript frontend
- **Cloud Platform**: AWS deployment (ECS/Fargate, RDS, Lambda)
- **Architecture**: Clean Architecture, CQRS, DDD, Event-Driven
- **Standards**: 80%+ code coverage, async/await, multi-tenancy

### Agent Coordination

Agents execute in parallel where possible:

```
Ticket (ai-ready, TODO)
  ↓
Architecture Analysis
  ↓
┌──────────────┬──────────────┬──────────────┐
│ Implementation│   Testing    │   DevOps     │
└──────────────┴──────────────┴──────────────┘
  ↓
Code Review
  ↓
Auto-merge or Draft PR
```

### Usage

**Label an issue with `ai-ready`** to trigger the pipeline:

```markdown
Title: Implement notification service
Labels: ai-ready

Description:
Create a notification service that handles:
- Email notifications via SendGrid
- SMS notifications via Twilio
- Push notifications via Firebase
- Multi-tenant support
- Async processing with MassTransit
```

Add `auto-merge` label for automatic merge after successful review.

## General Coding Standards
- Use modern C# features and nullable reference types for all new code.
- Follow SOLID principles (see [SOLID Principles & Code Organization](./instructions/solid-principles.md)) and established naming conventions.
- All services must implement structured logging and dependency injection pattern.
- Prefer `async/await` for all I/O operations.
- Use record types for DTOs and value objects where appropriate.

## Project Naming & Structure Standards
- **Always use PascalCase with dots** for C# project folders: `Avro.Mcp.Orchestrator` ✅ (not `avro-mcp-orchestrator` ❌)
- **Implement layered architecture** with one project per layer:
  - `Avro.{ServiceName}.Abstractions` - Interfaces & contracts
  - `Avro.{ServiceName}.Domain` - Business logic & aggregates
  - `Avro.{ServiceName}.Infrastructure` - Data access & external integrations
  - `Avro.{ServiceName}.Application` - Commands, queries, handlers (MediatR)
  - `Avro.{ServiceName}.WebApi` - HTTP API endpoints
  - `Avro.{ServiceName}.EventHandlers` - Event consumers
  - `Avro.{ServiceName}.Workers` - Background jobs
- See [BEST Template Structure](./instructions/best-template-patterns.md) for complete project organization.

## Architecture Principles
- Follow Clean Architecture with clear separation of concerns.
- Use CQRS pattern with MediatR for command/query handling.
- Implement event-driven architecture for cross-service communication.
- Apply Domain-Driven Design patterns for business logic.

## Security & Quality
- Never commit secrets, connection strings, or API keys.
- Always validate inputs at API boundaries.
- Use parameterized queries to prevent SQL injection.
- Maintain minimum 80% code coverage with unit tests.

## How to Find More Rules
- For architecture patterns, see: [Architecture Collection](./instructions/architecture-collection.md)
- For security best practices, see: [Security Collection](./instructions/security-collection.md)
- For testing guidelines, see: [Testing Collection](./instructions/testing-collection.md)

## Core Topics
### C# & .NET
- [SOLID Principles & Code Organization](./instructions/solid-principles.md)
- [Modern C# Features](./instructions/modern-csharp-features.md)
- [Nullable Reference Types](./instructions/nullable-reference-types.md)
- [Async/Await Patterns](./instructions/async-await-patterns.md)
- [Value Objects & Immutability](./instructions/value-objects.md)

### Domain-Driven Design
- [DDD Aggregates & Domain Events](./instructions/ddd-aggregates.md)
- [State Machine Patterns](./instructions/state-machine-patterns.md)
- [BEST Template Structure](./instructions/best-template-patterns.md)

### Architecture
- [CQRS & MediatR](./instructions/cqrs-mediatr.md)
- [Event-Driven Architecture](./instructions/event-driven-architecture.md)
- [Microservices Communication](./instructions/microservices-communication.md)
- [API Gateway Patterns](./instructions/api-gateway-patterns.md)
- [Saga Pattern](./instructions/saga-pattern.md)

### Data Access
- [EF Core Patterns](./instructions/ef-core-patterns.md)
- [Repository Pattern](./instructions/repository-pattern.md)

### Web API
- [Middleware Pipeline](./instructions/middleware-pipeline.md)
- [Rate Limiting](./instructions/rate-limiting.md)
- [API Versioning](./instructions/api-versioning.md)

### Security
- [Input Validation](./instructions/input-validation.md)
- [SQL Injection Prevention](./instructions/sql-injection-prevention.md)
- [Secrets Management](./instructions/secrets-management.md)

### Testing
- [Unit Testing](./instructions/unit-testing.md)
- [Integration Testing](./instructions/integration-testing.md)

## On-Demand Prompts
For specific tasks, use these prompt files:
- [API Review](./prompts/api-review.prompt.md)
- [Performance Tuning](./prompts/performance-tuning.prompt.md)
- [Security Audit](./prompts/security-audit.prompt.md)