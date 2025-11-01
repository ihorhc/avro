---
applyTo: "tests/**/*.cs"
---

# Unit Testing

## Guidelines
- **Use xUnit** as testing framework:
  ```csharp
  public class OrderServiceTests
  {
      [Fact]
      public async Task CreateOrder_WithValidData_ReturnsOrderDto()
      {
          // Arrange
          var mockRepo = new Mock<IOrderRepository>();
          var service = new OrderService(mockRepo.Object);
          var request = new CreateOrderRequest { CustomerName = "John" };
          
          // Act
          var result = await service.CreateOrderAsync(request);
          
          // Assert
          Assert.NotNull(result);
          Assert.Equal("John", result.CustomerName);
      }
      
      [Theory]
      [InlineData("")]
      [InlineData(null)]
      public async Task CreateOrder_WithInvalidName_ThrowsException(string name)
      {
          // Arrange
          var service = new OrderService(Mock.Of<IOrderRepository>());
          
          // Act & Assert
          await Assert.ThrowsAsync<ValidationException>(
              () => service.CreateOrderAsync(new CreateOrderRequest { CustomerName = name }));
      }
  }
  ```

- **Use Moq** for mocking dependencies:
  ```csharp
  var mockRepo = new Mock<IOrderRepository>();
  mockRepo.Setup(x => x.GetByIdAsync(It.IsAny<int>(), It.IsAny<CancellationToken>()))
          .ReturnsAsync(new Order { Id = 1, CustomerName = "John" });
  
  // Verify interactions
  mockRepo.Verify(x => x.AddAsync(It.IsAny<Order>(), It.IsAny<CancellationToken>()), 
                  Times.Once);
  ```

- **Use FluentAssertions** for readable assertions:
  ```csharp
  result.Should().NotBeNull();
  result.CustomerName.Should().Be("John");
  result.Items.Should().HaveCount(3);
  result.TotalAmount.Should().BeGreaterThan(0);
  ```

- **Follow AAA pattern** (Arrange, Act, Assert):
  ```csharp
  [Fact]
  public async Task Test_Method_Scenario_ExpectedBehavior()
  {
      // Arrange - Set up test data and dependencies
      
      // Act - Execute the method under test
      
      // Assert - Verify the expected outcome
  }
  ```

- **Aim for 80%+ code coverage**:
  ```bash
  dotnet test /p:CollectCoverage=true /p:CoverletOutputFormat=opencover
  ```

## Anti-Patterns
- ❌ Don't test implementation details - test behavior
- ❌ Never have multiple assertions for unrelated things in one test
- ❌ Avoid test interdependencies - each test should be independent