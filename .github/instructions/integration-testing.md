---
applyTo: "tests/**/*.cs"
---

# Integration Testing

## Guidelines
- **Use WebApplicationFactory** for API integration tests:
  ```csharp
  public class OrdersApiTests : IClassFixture<WebApplicationFactory<Program>>
  {
      private readonly WebApplicationFactory<Program> _factory;
      
      public OrdersApiTests(WebApplicationFactory<Program> factory)
      {
          _factory = factory;
      }
      
      [Fact]
      public async Task GetOrders_ReturnsSuccessStatusCode()
      {
          // Arrange
          var client = _factory.CreateClient();
          
          // Act
          var response = await client.GetAsync("/api/v1/orders");
          
          // Assert
          response.EnsureSuccessStatusCode();
          var content = await response.Content.ReadAsStringAsync();
          content.Should().NotBeNullOrEmpty();
      }
  }
  ```

- **Use TestContainers** for database integration tests:
  ```csharp
  public class DatabaseIntegrationTests : IAsyncLifetime
  {
      private readonly MsSqlContainer _container = new MsSqlBuilder()
          .WithImage("mcr.microsoft.com/mssql/server:2022-latest")
          .Build();
      
      public async Task InitializeAsync()
      {
          await _container.StartAsync();
      }
      
      public async Task DisposeAsync()
      {
          await _container.DisposeAsync();
      }
      
      [Fact]
      public async Task CanInsertAndRetrieveOrder()
      {
          // Arrange
          var options = new DbContextOptionsBuilder<AppDbContext>()
              .UseSqlServer(_container.GetConnectionString())
              .Options;
          
          await using var context = new AppDbContext(options);
          await context.Database.EnsureCreatedAsync();
          
          // Act
          var order = new Order { CustomerName = "John" };
          context.Orders.Add(order);
          await context.SaveChangesAsync();
          
          // Assert
          var retrieved = await context.Orders.FindAsync(order.Id);
          retrieved.Should().NotBeNull();
          retrieved!.CustomerName.Should().Be("John");
      }
  }
  ```

- **Override configuration for tests**:
  ```csharp
  var factory = new WebApplicationFactory<Program>()
      .WithWebHostBuilder(builder =>
      {
          builder.ConfigureAppConfiguration((context, config) =>
          {
              config.AddInMemoryCollection(new Dictionary<string, string>
              {
                  ["ConnectionStrings:DefaultConnection"] = _container.GetConnectionString()
              });
          });
      });
  ```

## Anti-Patterns
- ❌ Don't use production database for tests
- ❌ Never leave test data in database after tests complete
- ❌ Avoid slow tests - use in-memory or containerized databases