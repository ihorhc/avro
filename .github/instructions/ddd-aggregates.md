---
applyTo: "src/**/Domain/**/*.cs,src/**/Aggregates/**/*.cs"
---

# Domain-Driven Design & Aggregates

## Guidelines
- **Use Aggregate Root pattern** for domain entities with business logic:
  ```csharp
  public abstract class AggregateRoot
  {
      public string Id { get; protected set; }
      public string Tenant { get; protected set; }
      
      private readonly List<IDomainEvent> _domainEvents = new();
      public IReadOnlyCollection<IDomainEvent> DomainEvents => _domainEvents.AsReadOnly();
      
      protected void AddDomainEvent(IDomainEvent domainEvent) => _domainEvents.Add(domainEvent);
      public void ClearDomainEvents() => _domainEvents.Clear();
  }
  ```

- **Implement Rich Domain Model** - business logic in aggregates, not services:
  ```csharp
  public class UgiAggregate : AggregateRoot
  {
      // Static factory method for creation
      public static UgiAggregate Create(string ugiCode, string tenant, string facilityId, 
          List<ExciseStamp> stamps, string createdBy)
      {
          var ugi = new UgiAggregate
          {
              Id = Guid.NewGuid().ToString(),
              UgiCode = ugiCode,
              Tenant = tenant,
              State = UgiState.Draft
          };
          
          ugi.AddDomainEvent(new UgiCreatedEvent(ugi.Id, ugiCode, createdBy));
          return ugi;
      }
      
      // Business operations
      public void Activate(string activatedBy)
      {
          if (!_stateMachine.CanFire(UgiTrigger.Activate))
              throw new InvalidStateTransitionException();
              
          var previousState = State;
          _stateMachine.Fire(UgiTrigger.Activate);
          State = _stateMachine.State;
          
          AddDomainEvent(new UgiStateChangedEvent(Id, previousState, State, activatedBy));
      }
  }
  ```

- **Domain Events with MediatR integration**:
  ```csharp
  // Domain event inherits INotification for MediatR
  public interface IDomainEvent : INotification
  {
      DateTime OccurredAt { get; }
      string AggregateId { get; }
  }
  
  public abstract class DomainEventBase : IDomainEvent
  {
      public DateTime OccurredAt { get; }
      public string AggregateId { get; }
      
      protected DomainEventBase(string aggregateId)
      {
          AggregateId = aggregateId;
          OccurredAt = DateTime.UtcNow;
      }
  }
  
  // Concrete event
  public record UgiCreatedEvent(string AggregateId, string UgiCode, string CreatedBy) 
      : DomainEventBase(AggregateId);
  ```

- **Publish events after persistence**:
  ```csharp
  public async Task<UgiAggregate> CreateUgiAsync(CreateUgiCommand cmd, CancellationToken ct)
  {
      var ugi = UgiAggregate.Create(cmd.UgiCode, cmd.Tenant, cmd.FacilityId, 
          cmd.ExciseStamps, cmd.CreatedBy);
      
      await _repository.AddAsync(ugi, ct);
      await _context.SaveChangesAsync(ct); // Persist first
      
      // Publish domain events through MediatR
      foreach (var evt in ugi.DomainEvents)
          await _mediator.Publish(evt, ct);
      
      ugi.ClearDomainEvents();
      return ugi;
  }
  ```

## Anti-Patterns
- ❌ Don't put business logic in application services - use aggregates
- ❌ Never expose domain events as public setters
- ❌ Avoid anemic domain models with only getters/setters
- ❌ Don't publish events before database transaction commits