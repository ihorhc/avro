---
applyTo: "src/**/*.csproj,src/**/Program.cs"
---

# Corporate BEST Template Patterns

## Project Structure

### Folder Naming Convention
- **Always use PascalCase with dots**, never kebab-case
  ```
  ✅ Good:   Avro.Mcp.Orchestrator/
  ❌ Bad:    avro-mcp-orchestrator/
  ✅ Good:   Avro.Orders.WebApi/
  ❌ Bad:    avro-orders-webapi/
  ```

### Layered Architecture - One Project Per Layer
Each service should have a dedicated project for each architectural layer:

- **Standard solution layout**:
  ```
  ├── src/
  │   ├── Avro.{ServiceName}.Abstractions/    # Interfaces & contracts
  │   ├── Avro.{ServiceName}.Domain/          # Business logic & aggregates
  │   ├── Avro.{ServiceName}.Infrastructure/  # EF Core, repositories, external services
  │   ├── Avro.{ServiceName}.Application/     # Commands, queries, handlers (MediatR)
  │   ├── Avro.{ServiceName}.WebApi/          # ASP.NET Core API (ECS/Fargate)
  │   ├── Avro.{ServiceName}.EventHandlers/   # Event consumers & handlers
  │   └── Avro.{ServiceName}.Workers/         # Background jobs (Lambda/SQS)
  ├── tests/
  │   ├── Avro.{ServiceName}.UnitTests/
  │   └── Avro.{ServiceName}.IntegrationTests/
  ├── infra/
  │   └── Avro.{ServiceName}.Infrastructure.Cdk/  # AWS CDK infrastructure (C#)
  └── contracts/
      ├── Avro.{ServiceName}.Contracts/       # Shared DTOs & API contracts
      └── Avro.{ServiceName}.Sdk/             # Refit HTTP client
  ```

### Layer Responsibilities

| Layer | Purpose | Key Exports |
|-------|---------|------------|
| **Abstractions** | Interfaces & contracts | `IRepository<T>`, `IService`, DTOs |
| **Domain** | Business logic & domain rules | Aggregates, Value Objects, Domain Events |
| **Infrastructure** | Data access & external integrations | EF DbContext, Repositories, Services |
| **Application** | Use cases & orchestration | Commands, Queries, Handlers (MediatR) |
| **WebApi** | HTTP endpoints | Controllers, Middleware, Program.cs |
| **EventHandlers** | Async event processing | Event Consumers, Background Tasks |
| **Workers** | Long-running background work | Lambda handlers, SQS processors |

## Naming Conventions

### Project Naming Pattern
All projects follow the pattern: `Avro.{ServiceName}.{Layer}`

Examples:
```csharp
// Orders Service
Avro.Orders.Abstractions
Avro.Orders.Domain
Avro.Orders.Infrastructure
Avro.Orders.Application
Avro.Orders.WebApi
Avro.Orders.EventHandlers
Avro.Orders.Workers
Avro.Orders.UnitTests
Avro.Orders.IntegrationTests

// MCP Orchestrator Service
Avro.Mcp.Abstractions
Avro.Mcp.Domain
Avro.Mcp.Infrastructure
Avro.Mcp.Application
Avro.Mcp.WebApi
```

### Code Entity Naming
- **Service naming**: Consistent within each layer
  ```csharp
  // Abstractions layer - define interfaces
  public interface IOrderService { }
  public interface IOrderRepository { }
  
  // Application layer - command handlers
  public record CreateOrderCommand : IRequest<OrderModel>;
  public class CreateOrderCommandHandler : IRequestHandler<CreateOrderCommand, OrderModel>;
  
  // Infrastructure layer - implementations
  public class OrderRepository : IOrderRepository { }
  public class OrderService : IOrderService { }
  
  // Domain layer - aggregates & value objects
  public class Order : AggregateRoot { }
  public record Money(decimal Amount, string Currency);
  ```

- **Repository naming**:
  ```csharp
  // Interface in Abstractions
  public interface IOrderRepository : IRepository<Order> { }
  
  // Implementation in Infrastructure
  public class OrderRepository : Repository<Order>, IOrderRepository { }
  ```

- **API Endpoints naming**:
  ```csharp
  public static class OrdersEndpoints
  {
      public static void MapOrdersEndpoints(this WebApplication app) { }
  }
  ```

- **Commands/Queries naming**:
  ```csharp
  // Commands (singular action)
  public record CreateOrderCommand : IRequest<OrderModel>;
  public record UpdateOrderCommand : IRequest<OrderModel>;
  public record DeleteOrderCommand : IRequest<Unit>;
  
  // Queries (plural for lists)
  public record GetOrdersQuery(int Page, int PageSize) : IRequest<PagedResult<OrderModel>>;
  public record GetOrderByIdQuery(int Id) : IRequest<OrderModel>;
  ```

## Required Technologies
- **Compute**: ECS/Fargate containers (primary), Lambda for events
- **Database**: Aurora RDS with Entity Framework Core
- **API**: Application Load Balancer + ASP.NET Core Minimal APIs
- **Messaging**: SQS for async commands
- **Infrastructure**: AWS CDK in C#

## Mandatory Libraries
```xml
<ItemGroup>
  <PackageReference Include="MediatR" />
  <PackageReference Include="AutoMapper" />
  <PackageReference Include="FluentValidation" />
  <PackageReference Include="Microsoft.EntityFrameworkCore" />
  <PackageReference Include="Refit" />
  <PackageReference Include="Serilog" />
  <PackageReference Include="OpenTelemetry" />
  <PackageReference Include="Stateless" />
</ItemGroup>
```

## Multi-Tenancy Pattern
```csharp
public abstract class AggregateRoot
{
    public string Tenant { get; protected set; }  // Required for all aggregates
}

// EF Core global query filter
modelBuilder.Entity<UgiModel>()
    .HasQueryFilter(e => e.Tenant == _currentTenant.TenantId);
```

## Anti-Patterns
- ❌ Don't use kebab-case for project names (e.g., `avro-mcp-orchestrator`)
- ❌ Never mix PascalCase and kebab-case in the same solution
- ❌ Don't create monolithic projects that mix multiple layers
- ❌ Avoid combining Infrastructure + Domain in one project
- ❌ Never skip Abstractions layer - always define contracts
- ❌ Don't use DynamoDB for services - use Aurora RDS
- ❌ Never hardcode tenant IDs - use dependency injection
- ❌ Avoid Lambda for primary APIs - use ECS/Fargate
- ❌ Don't skip multi-tenancy support in aggregates

## Migration Guide for Existing Projects

If updating existing projects from flat structure to layered structure:

### Before (Monolithic)
```
├── MyService/
│   ├── Controllers/
│   ├── Services/
│   ├── Models/
│   ├── Data/
│   └── Program.cs
```

### After (Layered)
```
├── Avro.MyService.Abstractions/
│   ├── IMyService.cs
│   ├── IRepository.cs
│   └── DTOs/
├── Avro.MyService.Domain/
│   ├── Aggregates/
│   ├── ValueObjects/
│   └── Events/
├── Avro.MyService.Infrastructure/
│   ├── Data/
│   ├── Repositories/
│   └── ExternalServices/
├── Avro.MyService.Application/
│   ├── Commands/
│   ├── Queries/
│   └── Handlers/
├── Avro.MyService.WebApi/
│   ├── Controllers/
│   ├── Endpoints/
│   ├── Middleware/
│   └── Program.cs
└── Avro.MyService.UnitTests/
    └── [Test files by layer]
```

### Key Dependencies (one-way)
```
WebApi → Application → Domain
        ↓              ↑
    Infrastructure ----
        ↓
    Abstractions
```

## Related Guidelines

See also:
- [SOLID Principles & Code Organization](./solid-principles.md) - One type per file, class size limits
- [Modern C# Features](./modern-csharp-features.md) - File-scoped namespaces, primary constructors
- [CQRS & MediatR](./cqrs-mediatr.md) - Commands/queries structure per Application layer
- [DDD Aggregates & Domain Events](./ddd-aggregates.md) - Domain layer patterns
- [EF Core Patterns](./ef-core-patterns.md) - Infrastructure layer database patterns
- [Repository Pattern](./repository-pattern.md) - Data access layer design
- [Unit Testing](./unit-testing.md) - UnitTests project structure
- [Integration Testing](./integration-testing.md) - IntegrationTests project structure