---
applyTo: "src/**/*.cs"
---

# Nullable Reference Types

## Guidelines
- **Always enable** nullable reference types in every .cs file:
  ```csharp
  #nullable enable
  ```
  
- Or enable globally in `.csproj`:
  ```xml
  <Nullable>enable</Nullable>
  ```

- Mark nullable parameters and properties explicitly:
  ```csharp
  public string? OptionalName { get; set; }
  public string RequiredName { get; set; } = string.Empty;
  ```

- Use null-forgiving operator `!` **only** in legacy code migrations or when you have verified null safety:
  ```csharp
  // Only when you're certain it's not null
  var value = dict["key"]!;
  ```

- Prefer null-coalescing operators:
  ```csharp
  string name = input?.Name ?? "Unknown";
  ```

## Anti-Patterns
- ❌ Never disable nullable warnings globally
- ❌ Don't overuse `!` operator - it defeats the purpose
- ❌ Avoid mixing nullable and non-nullable contexts