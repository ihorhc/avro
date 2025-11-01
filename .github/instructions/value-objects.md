---
applyTo: "src/**/ValueObjects/**/*.cs,src/**/Domain/**/*.cs"
---

# Value Objects & Immutability

## Guidelines
- **Use record types for value objects** (C# 9+):
  ```csharp
  public record ExciseStamp(string Code, string Type, DateTime IssuedDate)
  {
      // Value-based equality by default
      // Immutable by default
  }
  ```

- **Complex value objects with validation**:
  ```csharp
  public class ContentData
  {
      public IReadOnlyList<ExciseStamp> ExciseStamps { get; }
      public IReadOnlyList<string> ChildUgiIds { get; }
      public ContentSource Source { get; }
      public IReadOnlyDictionary<string, object> Metadata { get; }
      
      public ContentData(
          List<ExciseStamp> stamps, 
          List<string> childIds, 
          ContentSource source,
          Dictionary<string, object>? metadata = null)
      {
          if (stamps == null || stamps.Count == 0)
              throw new ArgumentException("At least one stamp required");
          
          ExciseStamps = stamps.AsReadOnly();
          ChildUgiIds = childIds?.AsReadOnly() ?? new List<string>().AsReadOnly();
          Source = source;
          Metadata = metadata?.AsReadOnly() ?? 
              new Dictionary<string, object>().AsReadOnly();
      }
      
      // Factory method for cloning with changes
      public ContentData WithStamps(List<ExciseStamp> newStamps) =>
          new(newStamps, ChildUgiIds.ToList(), Source, 
              Metadata.ToDictionary(kv => kv.Key, kv => kv.Value));
  }
  ```

- **Enum value objects**:
  ```csharp
  public enum ContentSource
  {
      Manual = 0,      // Created via API
      Protocol = 1,    // From scanning protocol
      EED = 2         // From Electronic Excise Document
  }
  
  // Use in domain logic
  public bool IsManuallyCreated => ContentData.Source == ContentSource.Manual;
  ```

- **Immutable collections**:
  ```csharp
  public class UgiAggregate : AggregateRoot
  {
      private readonly List<ExciseStamp> _stamps = new();
      public IReadOnlyList<ExciseStamp> Stamps => _stamps.AsReadOnly();
      
      public void AddStamp(ExciseStamp stamp)
      {
          if (_stamps.Any(s => s.Code == stamp.Code))
              throw new DuplicateStampException(stamp.Code);
          
          _stamps.Add(stamp);
      }
  }
  ```

- **Required properties for DTOs**:
  ```csharp
  public record CreateUgiCommand : IRequest<UgiModel>
  {
      public required string UgiCode { get; init; }
      public required string Tenant { get; init; }
      public required string FacilityId { get; init; }
      public required List<ExciseStamp> ExciseStamps { get; init; }
      public List<string>? ChildUgiIds { get; init; } // Optional
  }
  ```

## Anti-Patterns
- ❌ Don't use mutable value objects
- ❌ Never expose internal collections directly - use AsReadOnly()
- ❌ Avoid value objects with identity (use entities instead)
- ❌ Don't create value objects without validation