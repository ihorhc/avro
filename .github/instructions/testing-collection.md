# Testing Collection

This collection aggregates testing strategies and best practices for the Avro platform.

## Included Topics

### Test Types
- [Unit Testing](./unit-testing.md) - xUnit, Moq, and FluentAssertions
- [Integration Testing](./integration-testing.md) - WebApplicationFactory and TestContainers

### Related Topics
- [CQRS & MediatR](./cqrs-mediatr.md) - Testing command and query handlers
- [Repository Pattern](./repository-pattern.md) - Mocking repositories
- [EF Core Patterns](./ef-core-patterns.md) - In-memory database testing

## Testing Strategy

### Unit Tests (80% of tests)
- Test business logic in isolation
- Mock all external dependencies
- Fast execution (< 100ms per test)
- Target: 80%+ code coverage

### Integration Tests (15% of tests)
- Test API endpoints end-to-end
- Use TestContainers for real database
- Verify cross-component interactions
- Test authentication/authorization flows

### E2E Tests (5% of tests)
- Test critical user journeys
- Use production-like environment
- Automate deployment verification

## Test Naming Convention

```csharp
[Fact]
public void MethodName_Scenario_ExpectedBehavior()
{
    // UnitOfWork_StateUnderTest_ExpectedBehavior
}
```

Examples:
- `CreateOrder_WithValidData_ReturnsOrderDto`
- `CreateOrder_WithEmptyCustomerName_ThrowsValidationException`
- `GetOrders_WhenNoneExist_ReturnsEmptyList`

## When to Use This Collection

Reference this collection when:
- Writing tests for new features
- Improving test coverage
- Setting up test infrastructure
- Reviewing test quality
- Debugging failing tests
- Onboarding developers to testing practices

## Usage

In Copilot Chat:
```
@workspace Use the testing collection to help me write tests for this service
```

In PR descriptions:
```
Tests added - following .github/instructions/testing-collection.md
```