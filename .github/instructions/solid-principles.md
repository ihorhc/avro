---
applyTo: "src/**/*.cs"
---

# SOLID Principles & Code Organization

## Single Responsibility Principle (SRP)

### One Type Per File
- **1 class = 1 file**
- **1 interface = 1 file**
- **1 struct = 1 file**
- **1 enum = 1 file**
- **1 record = 1 file**

```csharp
// ✅ Correct - File: Order.cs
public class Order
{
    public int Id { get; set; }
    public string CustomerName { get; set; }
}

// ❌ Wrong - Multiple types in one file
public class Order { }
public class OrderItem { }
public interface IOrderService { }
```

### File Naming Convention
- File name matches the primary type name exactly
- Use PascalCase for both file names and type names

```
src/
├── Domain/
│   ├── Order.cs           // public class Order
│   ├── OrderItem.cs       // public class OrderItem
│   ├── OrderStatus.cs     // public enum OrderStatus
│   └── IOrderRepository.cs // public interface IOrderRepository
├── ValueObjects/
│   ├── Money.cs           // public record Money
│   └── ExciseStamp.cs     // public record ExciseStamp
```

### Class Size Limits

**Maximum 200 lines of code per class**
- Includes method implementations
- Excludes XML documentation

```csharp
// ✅ Correct - Focused responsibility
public class OrderService
{
    private readonly IOrderRepository _repository;
    private readonly IMediator _mediator;
    
    public OrderService(IOrderRepository repository, IMediator mediator)
    {
        _repository = repository;
        _mediator = mediator;
    }
    
    public async Task<Order> CreateOrderAsync(CreateOrderCommand cmd, CancellationToken ct)
    {
        var order = Order.Create(cmd.CustomerName, cmd.Items);
        await _repository.AddAsync(order, ct);
        await _mediator.Publish(new OrderCreatedEvent(order.Id), ct);
        return order;
    }
    
    public async Task<Order?> GetOrderAsync(int id, CancellationToken ct)
    {
        return await _repository.GetByIdAsync(id, ct);
    }
}

// ❌ Wrong - Too many responsibilities
public class OrderService
{
    // Order management
    public async Task<Order> CreateOrderAsync(...) { }
    public async Task UpdateOrderAsync(...) { }
    public async Task<Order?> GetOrderAsync(...) { }
    
    // Payment processing
    public async Task<PaymentResult> ProcessPaymentAsync(...) { }
    public async Task RefundPaymentAsync(...) { }
    
    // Inventory management
    public async Task ReserveInventoryAsync(...) { }
    public async Task ReleaseInventoryAsync(...) { }
    
    // Notification
    public async Task SendOrderConfirmationAsync(...) { }
    public async Task SendShippingNotificationAsync(...) { }
    
    // ... many more responsibilities
}
```

## Don't Repeat Yourself (DRY)

### Common Code in Shared Classes
- Extract shared logic to base classes or utility classes
- Reuse domain logic through aggregates
- Use extension methods for cross-cutting concerns

```csharp
// ✅ Correct - Shared base class
public abstract class AggregateRoot
{
    public string Id { get; protected set; }
    public string Tenant { get; protected set; }
    
    private readonly List<IDomainEvent> _domainEvents = new();
    public IReadOnlyCollection<IDomainEvent> DomainEvents => _domainEvents.AsReadOnly();
    
    protected void AddDomainEvent(IDomainEvent domainEvent) => _domainEvents.Add(domainEvent);
    public void ClearDomainEvents() => _domainEvents.Clear();
}

public class Order : AggregateRoot { }
public class Customer : AggregateRoot { }

// ❌ Wrong - Duplicate domain event logic in each aggregate
public class Order
{
    private readonly List<IDomainEvent> _domainEvents = new();
    protected void AddDomainEvent(IDomainEvent evt) => _domainEvents.Add(evt);
}

public class Customer
{
    private readonly List<IDomainEvent> _domainEvents = new();
    protected void AddDomainEvent(IDomainEvent evt) => _domainEvents.Add(evt);
}
```

### Reusable Validators
- Create validator classes for complex validation logic
- Share through validation base classes

```csharp
// ✅ Correct - Reusable validator
public abstract class EntityValidator<T> : AbstractValidator<T> where T : class
{
    protected void ConfigureCommonRules()
    {
        // Common validation rules
    }
}

public class OrderValidator : EntityValidator<Order>
{
    public OrderValidator()
    {
        ConfigureCommonRules();
        // Order-specific rules
    }
}

// ❌ Wrong - Duplicate validation logic
public class OrderValidator : AbstractValidator<Order>
{
    public OrderValidator()
    {
        RuleFor(x => x.Id).NotEmpty();
        RuleFor(x => x.Tenant).NotEmpty();
        // ... duplicate validation
    }
}

public class CustomerValidator : AbstractValidator<Customer>
{
    public CustomerValidator()
    {
        RuleFor(x => x.Id).NotEmpty();
        RuleFor(x => x.Tenant).NotEmpty();
        // ... duplicate validation
    }
}
```

## Violation Detection

### Signs of SRP Violation
- Class has multiple reasons to change
- Class name uses "And", "Handler", "Manager", "Processor"
- Class has 10+ public methods
- Class exceeds 200 lines
- Difficult to unit test in isolation
- Hard to name the class

### Signs of DRY Violation
- Copy-paste code across multiple files
- Similar logic in different classes
- Duplicate validation/business rules
- Repeated error handling patterns

## Anti-Patterns

- ❌ Multiple types in one file
- ❌ Classes larger than 200 lines
- ❌ Mixed concerns in one class
- ❌ Duplicate validation logic
- ❌ Repeated error handling
- ❌ Classes with unclear single responsibility
- ❌ Shared static utility classes with unrelated methods

## Related Guidelines

See also:
- [Modern C# Features](./modern-csharp-features.md) - File-scoped namespaces
- [DDD Aggregates](./ddd-aggregates.md) - Rich domain models
- [Value Objects](./value-objects.md) - Immutable value types
- [CQRS & MediatR](./cqrs-mediatr.md) - Separate concerns with handlers
