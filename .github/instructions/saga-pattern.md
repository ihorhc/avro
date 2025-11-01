---
applyTo: "src/**/Sagas/**/*.cs"
---

# Saga Pattern

## Guidelines
- **Use MassTransit state machine sagas** for distributed transactions:
  ```csharp
  public class OrderStateMachine : MassTransitStateMachine<OrderState>
  {
      public OrderStateMachine()
      {
          InstanceState(x => x.CurrentState);
          
          Event(() => OrderSubmitted);
          Event(() => PaymentCompleted);
          Event(() => PaymentFailed);
          
          Initially(
              When(OrderSubmitted)
                  .TransitionTo(AwaitingPayment)
                  .Publish(context => new ProcessPaymentCommand(...)));
          
          During(AwaitingPayment,
              When(PaymentCompleted)
                  .TransitionTo(Completed)
                  .Publish(context => new OrderCompletedEvent(...)),
              When(PaymentFailed)
                  .TransitionTo(Cancelled)
                  .Publish(context => new CancelOrderCommand(...)));
      }
      
      public State AwaitingPayment { get; private set; }
      public State Completed { get; private set; }
      public State Cancelled { get; private set; }
  }
  ```

- **Implement compensating actions** for rollback:
  ```csharp
  When(InventoryReservationFailed)
      .TransitionTo(Compensating)
      .Publish(context => new CancelPaymentCommand(...))
      .Publish(context => new ReleaseInventoryCommand(...));
  ```

- **Store saga state** in a durable store:
  ```csharp
  services.AddMassTransit(x =>
  {
      x.AddSagaStateMachine<OrderStateMachine, OrderState>()
          .EntityFrameworkRepository(r =>
          {
              r.ExistingDbContext<SagaDbContext>();
          });
  });
  ```

## Anti-Patterns
- ❌ Don't use sagas for simple workflows - use regular commands
- ❌ Never ignore compensation logic
- ❌ Avoid long-running sagas without timeout handling