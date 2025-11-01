---
applyTo: "src/**/*.cs"
---

# Modern C# Features

## Guidelines
- Use **file-scoped namespaces** for all new files (C# 10+):
  ```csharp
  namespace Avro.Services.Orders;
  
  public class OrderService { }
  ```

- Use **global usings** in `GlobalUsings.cs` for common namespaces:
  ```csharp
  global using System;
  global using System.Linq;
  global using System.Threading.Tasks;
  global using Microsoft.Extensions.Logging;
  ```

- Prefer **primary constructors** for simple DTOs (C# 12+):
  ```csharp
  public class OrderDto(int Id, string CustomerName, decimal Total);
  ```

- Use **target-typed new** expressions:
  ```csharp
  List<Order> orders = new();
  ```

- Use **pattern matching** for type checks and null checks:
  ```csharp
  if (obj is Order { Status: OrderStatus.Completed } order)
  {
      // Use order
  }
  ```

## Anti-Patterns
- ❌ Avoid old namespace syntax with braces for new files
- ❌ Don't use `var` when the type is not obvious from the right side