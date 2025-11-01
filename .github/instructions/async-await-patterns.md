---
applyTo: "src/**/*.cs"
---

# Async/Await Patterns

## Guidelines
- **Always use async/await** for I/O operations (database, HTTP, file system):
  ```csharp
  public async Task<Order> GetOrderAsync(int id, CancellationToken ct)
  {
      return await _context.Orders.FindAsync(id, ct);
  }
  ```

- Include `CancellationToken` parameters for all async methods:
  ```csharp
  public async Task ProcessAsync(CancellationToken cancellationToken = default)
  ```

- Use `ConfigureAwait(false)` in library code, not in ASP.NET Core:
  ```csharp
  // In libraries only
  await task.ConfigureAwait(false);
  ```

- Avoid `async void` except for event handlers:
  ```csharp
  // ❌ Wrong
  public async void ProcessOrder() { }
  
  // ✅ Correct
  public async Task ProcessOrderAsync() { }
  ```

- Use `ValueTask<T>` for hot paths that often complete synchronously:
  ```csharp
  public ValueTask<int> GetCachedValueAsync(string key)
  ```

## Anti-Patterns
- ❌ Don't use `.Result` or `.Wait()` - causes deadlocks
- ❌ Don't mix sync and async code unnecessarily
- ❌ Never use `async void` for regular methods