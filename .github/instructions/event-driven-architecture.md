---
applyTo: "src/**/Events/**/*.cs,src/**/EventHandlers/**/*.cs"
---

# Event-Driven Architecture

## Guidelines
- **Define domain events** as immutable records:
  ```csharp
  public record OrderCreatedEvent(
      int OrderId,
      string CustomerName,
      decimal TotalAmount,
      DateTime CreatedAt) : INotification;
  ```

- **Publish events** after successful database transactions:
  ```csharp
  await _context.SaveChangesAsync(cancellationToken);
  await _mediator.Publish(new OrderCreatedEvent(...), cancellationToken);
  ```

- **Use MassTransit or NServiceBus** for distributed events:
  ```csharp
  public class OrderCreatedConsumer : IConsumer<OrderCreatedEvent>
  {
      public async Task Consume(ConsumeContext<OrderCreatedEvent> context)
      {
          var evt = context.Message;
          // Handle event
      }
  }
  ```

- **Implement idempotent handlers** to handle duplicate events:
  ```csharp
  public async Task Handle(OrderCreatedEvent evt, CancellationToken ct)
  {
      if (await _processedEvents.ContainsAsync(evt.EventId))
          return; // Already processed
      
      // Process event
      await _processedEvents.AddAsync(evt.EventId);
  }
  ```

## Anti-Patterns
- ❌ Don't publish events before saving to database
- ❌ Never throw exceptions in event handlers - log and handle gracefully
- ❌ Avoid synchronous processing of distributed events