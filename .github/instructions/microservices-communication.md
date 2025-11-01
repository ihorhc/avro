---
applyTo: "src/**/HttpClients/**/*.cs,src/**/Services/**/*.cs"
---

# Microservices Communication

## Guidelines
- **Use typed HttpClient** with IHttpClientFactory:
  ```csharp
  public interface IOrderServiceClient
  {
      Task<OrderDto> GetOrderAsync(int id, CancellationToken ct);
  }
  
  public class OrderServiceClient : IOrderServiceClient
  {
      private readonly HttpClient _httpClient;
      
      public OrderServiceClient(HttpClient httpClient)
      {
          _httpClient = httpClient;
      }
  }
  
  // Registration
  services.AddHttpClient<IOrderServiceClient, OrderServiceClient>(client =>
  {
      client.BaseAddress = new Uri("https://orders-api.avro.com");
  });
  ```

- **Implement Polly resilience policies**:
  ```csharp
  services.AddHttpClient<IOrderServiceClient, OrderServiceClient>()
      .AddPolicyHandler(GetRetryPolicy())
      .AddPolicyHandler(GetCircuitBreakerPolicy());
  
  static IAsyncPolicy<HttpResponseMessage> GetRetryPolicy()
  {
      return HttpPolicyExtensions
          .HandleTransientHttpError()
          .WaitAndRetryAsync(3, retryAttempt => 
              TimeSpan.FromSeconds(Math.Pow(2, retryAttempt)));
  }
  ```

- **Use gRPC for internal service-to-service** communication:
  ```csharp
  services.AddGrpcClient<OrderService.OrderServiceClient>(o =>
  {
      o.Address = new Uri("https://orders.avro.internal");
  });
  ```

## Anti-Patterns
- ❌ Don't create HttpClient instances manually
- ❌ Never hardcode service URLs - use configuration
- ❌ Avoid cascading failures - implement circuit breakers