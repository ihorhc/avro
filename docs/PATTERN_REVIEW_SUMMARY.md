# Design Pattern Review - Quick Reference

## Current Implementation Status

```
✅ Repository Pattern          (Configuration persistence)
✅ Singleton Pattern            (Process instance management)
✅ Facade Pattern               (CLI command interface)
✅ Process Wrapper Pattern      (Server lifecycle management)
✅ Async/Await Throughout       (I/O operations)
✅ Structured Logging           (Serilog integration)

❌ Command Pattern              (REQUIRED by Avro standards)
❌ Provider Pattern             (REQUIRED by Avro standards)
❌ Factory Pattern              (Enhanced object creation)
⚠️  Dependency Injection        (Minimal usage)
⚠️  FluentValidation            (Using basic validation)
```

## Quick Wins (Highest Impact)

### 1. Add Provider Pattern for Configuration
**File**: Create `Providers/IConfigurationProvider.cs`
```csharp
public interface IConfigurationProvider
{
    Task<List<ServerConfig>> LoadAsync(CancellationToken ct);
    Task SaveAsync(List<ServerConfig> configs, CancellationToken ct);
}
```
**Impact**: ⭐⭐⭐⭐⭐ Makes code testable and extensible

### 2. Add FluentValidation
**File**: Create `Validators/ServerConfigValidator.cs`
```csharp
public class ServerConfigValidator : AbstractValidator<ServerConfig>
{
    public ServerConfigValidator()
    {
        RuleFor(x => x.Name).NotEmpty();
        RuleFor(x => x.Command).NotEmpty();
        RuleFor(x => x.TimeoutSeconds).InclusiveBetween(1, 300);
    }
}
```
**Impact**: ⭐⭐⭐⭐ Aligns with Avro standards

### 3. Fix Async Handlers
**File**: Modify `Program.cs`
```csharp
// Before
list.SetHandler(() => new McpOrchestrator().ListServersAsync().Wait());

// After
list.SetHandler(async () => await new McpOrchestrator().ListServersAsync());
```
**Impact**: ⭐⭐⭐ Prevents UI blocking

---

## Risk Assessment

| Risk | Severity | Mitigation |
|------|----------|-----------|
| Command Injection via Arguments | Medium | Use ArgumentList for process args |
| Path Traversal in WorkingDirectory | Medium | Validate paths are under allowed root |
| Dictionary Not Thread-Safe | Low | Use ConcurrentDictionary |
| Process Resources Not Cleaned | Low | Implement IDisposable |

---

## File Structure After Improvements

```
src/avro-mcp-orchestrator/
├── Program.cs                    (Entry point)
├── McpOrchestrator.cs           (Refactored with DI)
├── McpServerInstance.cs         (Process wrapper)
├── Types.cs                      (Value objects)
├── Providers/
│   ├── IConfigurationProvider.cs (NEW)
│   └── JsonConfigurationProvider.cs (NEW)
├── Commands/
│   ├── ICommand.cs              (NEW)
│   ├── CommandHandler.cs        (NEW)
│   ├── AddServerCommandHandler.cs (NEW)
│   ├── StartServerCommandHandler.cs (NEW)
│   └── ... (other command handlers)
├── Validators/
│   └── ServerConfigValidator.cs (NEW)
├── Exceptions/
│   └── ServerException.cs       (NEW)
└── README.md
```

---

## Code Examples: Before & After

### Configuration Management

**BEFORE** (Monolithic):
```csharp
public class McpOrchestrator
{
    private void LoadConfiguration() { /* 20+ lines */ }
    private async Task SaveConfigurationAsync() { /* 15+ lines */ }
    private void InitializeConfigDirectory() { /* 8 lines */ }
}
```

**AFTER** (Extracted Provider):
```csharp
public interface IConfigurationProvider
{
    Task<List<ServerConfig>> LoadAsync(CancellationToken ct);
    Task SaveAsync(List<ServerConfig> configs, CancellationToken ct);
}

public class McpOrchestrator
{
    private readonly IConfigurationProvider _provider;
    
    public async Task Initialize()
    {
        _configurations = await _provider.LoadAsync(default);
    }
}
```

---

## Testing Improvement

**BEFORE** (Hard to test):
```csharp
// Can't test - creates McpOrchestrator directly
var orchestrator = new McpOrchestrator();
await orchestrator.AddServerAsync(config);  // Uses actual file system
```

**AFTER** (Easily testable):
```csharp
var mockProvider = new Mock<IConfigurationProvider>();
var orchestrator = new McpOrchestrator(mockProvider.Object);
await orchestrator.AddServerAsync(config);  // Uses mock
mockProvider.Verify(x => x.SaveAsync(It.IsAny<List<ServerConfig>>(), default));
```

---

## Implementation Timeline

| Phase | Tasks | Timeline |
|-------|-------|----------|
| **1** | Provider Pattern, FluentValidation, Async Handlers | 1-2 days |
| **2** | Command Pattern, DI Setup | 2-3 days |
| **3** | Exception Handling, Resource Management | 1-2 days |
| **4** | Unit Tests, Integration Tests | 3-5 days |
| **5** | Security Hardening, Documentation | 2-3 days |

**Total Estimated Effort**: 9-15 days

---

## Key References

- **Avro Standards**: See `.github/instructions/`
- **Design Patterns**: Gang of Four patterns
- **C# Best Practices**: Microsoft Docs
- **SOLID Principles**: Uncle Bob's Clean Code

---

## Next Steps

1. ✅ Review this assessment
2. ⏭️ Create Provider interface and implementation
3. ⏭️ Add FluentValidation dependency
4. ⏭️ Implement command handlers
5. ⏭️ Add comprehensive unit tests
6. ⏭️ Security audit and hardening
