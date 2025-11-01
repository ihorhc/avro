---
applyTo: "src/**/Commands/**/*.cs,src/**/Queries/**/*.cs"
---

# CQRS & MediatR

## Guidelines
- **Separate commands and queries** using MediatR:
  ```csharp
  // Command
  public record CreateOrderCommand(string CustomerName, List<OrderItem> Items) 
      : IRequest<OrderDto>;
  
  // Query
  public record GetOrderQuery(int OrderId) : IRequest<OrderDto>;
  ```

- **One handler per command/query**:
  ```csharp
  public class CreateOrderCommandHandler 
      : IRequestHandler<CreateOrderCommand, OrderDto>
  {
      public async Task<OrderDto> Handle(
          CreateOrderCommand request, 
          CancellationToken cancellationToken)
      {
          // Implementation
      }
  }
  ```

- Use **FluentValidation** for command validation:
  ```csharp
  public class CreateOrderCommandValidator : AbstractValidator<CreateOrderCommand>
  {
      public CreateOrderCommandValidator()
      {
          RuleFor(x => x.CustomerName).NotEmpty().MaximumLength(100);
      }
  }
  ```

- Register MediatR behaviors for cross-cutting concerns:
  ```csharp
  services.AddMediatR(cfg => {
      cfg.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly());
      cfg.AddBehavior(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));
      cfg.AddBehavior(typeof(IPipelineBehavior<,>), typeof(LoggingBehavior<,>));
  });
  ```

## Anti-Patterns
- ❌ Don't put business logic in controllers - use handlers
- ❌ Never return domain entities directly - use DTOs
- ❌ Avoid queries that modify state