# Architecture Collection

This collection aggregates architectural patterns and best practices for the Avro platform.

## Included Topics

### Domain-Driven Design
- [DDD Aggregates & Domain Events](./ddd-aggregates.md) - Rich domain models with MediatR integration
- [State Machine Patterns](./state-machine-patterns.md) - Stateless library for complex state transitions
- [Value Objects](./value-objects.md) - Immutable value objects and records
- [BEST Template Patterns](./best-template-patterns.md) - Corporate standards and project structure

### CQRS & Event-Driven Design
- [CQRS & MediatR Patterns](./cqrs-mediatr.md) - Command/Query separation with MediatR
- [Event-Driven Architecture](./event-driven-architecture.md) - Domain events and event handlers
- [Saga Pattern](./saga-pattern.md) - Distributed transaction management

### Microservices Patterns
- [Microservices Communication](./microservices-communication.md) - HTTP, gRPC, and service discovery
- [API Gateway Patterns](./api-gateway-patterns.md) - YARP configuration and gateway best practices

### Data Access
- [EF Core Patterns](./ef-core-patterns.md) - Entity Framework Core with Aurora RDS
- [Repository Pattern](./repository-pattern.md) - Repository and Unit of Work patterns

## When to Use This Collection

Reference this collection when:
- Designing new microservices or bounded contexts
- Implementing cross-service communication
- Planning event-driven workflows
- Architecting distributed transactions
- Setting up new data access layers
- Conducting architecture reviews

## Usage

In Copilot Chat:
```
@workspace Use the architecture collection to help me design a new order processing service
```

In PR descriptions:
```
Architecture review needed - see .github/instructions/architecture-collection.md
```