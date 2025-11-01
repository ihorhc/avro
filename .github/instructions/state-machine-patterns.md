---
applyTo: "src/**/Domain/**/*.cs,src/**/StateMachines/**/*.cs"
---

# State Machine Pattern (Stateless Library)

## Guidelines
- **Use Stateless library** for complex state transitions:
  ```csharp
  using Stateless;
  
  public class UgiStateMachine
  {
      private readonly StateMachine<UgiState, UgiTrigger> _machine;
      
      public UgiStateMachine(UgiState initialState)
      {
          _machine = new StateMachine<UgiState, UgiTrigger>(initialState);
          ConfigureTransitions();
      }
      
      private void ConfigureTransitions()
      {
          _machine.Configure(UgiState.Draft)
              .Permit(UgiTrigger.Activate, UgiState.Active)
              .Permit(UgiTrigger.Cancel, UgiState.Cancelled);
          
          _machine.Configure(UgiState.Active)
              .Permit(UgiTrigger.Deactivate, UgiState.Inactive)
              .Permit(UgiTrigger.Transfer, UgiState.Transferred);
          
          _machine.Configure(UgiState.Inactive)
              .Permit(UgiTrigger.Activate, UgiState.Active);
      }
      
      public UgiState State => _machine.State;
      public bool CanFire(UgiTrigger trigger) => _machine.CanFire(trigger);
      public void Fire(UgiTrigger trigger) => _machine.Fire(trigger);
  }
  ```

- **Integrate with aggregates**:
  ```csharp
  public class UgiAggregate : AggregateRoot
  {
      private readonly UgiStateMachine _stateMachine;
      
      public UgiState State { get; private set; }
      
      private UgiAggregate()
      {
          _stateMachine = new UgiStateMachine(State);
      }
      
      public void Activate(string activatedBy)
      {
          if (!_stateMachine.CanFire(UgiTrigger.Activate))
              throw new InvalidStateTransitionException(
                  $"Cannot activate UGI in state {State}");
          
          var previousState = State;
          _stateMachine.Fire(UgiTrigger.Activate);
          State = _stateMachine.State;
          
          AddDomainEvent(new UgiStateChangedEvent(Id, previousState, State, activatedBy));
      }
  }
  ```

- **State Machine as Single Source of Truth**:
  ```csharp
  // ❌ Don't duplicate transition logic
  public bool CanTransitionTo(UgiState targetState) { } // WRONG - duplicates logic
  
  // ✅ Always delegate to state machine
  public bool CanActivate() => _stateMachine.CanFire(UgiTrigger.Activate);
  ```

- **Use PlantUML for documentation**:
  ```plantuml
  @startuml
  [*] --> Draft
  Draft --> Active : Activate
  Draft --> Cancelled : Cancel
  Active --> Inactive : Deactivate
  Active --> Transferred : Transfer
  Inactive --> Active : Activate
  @enduml
  ```

## Anti-Patterns
- ❌ Don't manually check state transitions - use CanFire()
- ❌ Never duplicate state machine logic in aggregates
- ❌ Avoid exposing StateMachine instance publicly
- ❌ Don't create state machine in each method - initialize once