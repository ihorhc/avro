---
applyTo: "src/**/Gateway/**/*.cs"
---

# API Gateway Patterns

## Guidelines
- **Use YARP (Yet Another Reverse Proxy)** for API Gateway:
  ```json
  {
    "ReverseProxy": {
      "Routes": {
        "orders-route": {
          "ClusterId": "orders-cluster",
          "Match": {
            "Path": "/api/orders/{**catch-all}"
          }
        }
      },
      "Clusters": {
        "orders-cluster": {
          "Destinations": {
            "destination1": {
              "Address": "https://orders-service:443"
            }
          }
        }
      }
    }
  }
  ```

- **Implement rate limiting** at the gateway level:
  ```csharp
  services.AddRateLimiter(options =>
  {
      options.GlobalLimiter = PartitionedRateLimiter.Create<HttpContext, string>(context =>
          RateLimitPartition.GetFixedWindowLimiter(
              partitionKey: context.User.Identity?.Name ?? context.Request.Headers.Host.ToString(),
              factory: partition => new FixedWindowRateLimiterOptions
              {
                  PermitLimit = 100,
                  Window = TimeSpan.FromMinutes(1)
              }));
  });
  ```

- **Add authentication/authorization** at gateway:
  ```csharp
  app.UseAuthentication();
  app.UseAuthorization();
  app.MapReverseProxy();
  ```

## Anti-Patterns
- ❌ Don't implement business logic in the gateway
- ❌ Never expose internal service URLs directly
- ❌ Avoid putting all services behind a single monolithic gateway