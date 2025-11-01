---
applyTo: "src/**/Controllers/**/*.cs,src/**/Middleware/**/*.cs"
---

# Rate Limiting

## Guidelines
- **Use ASP.NET Core rate limiting middleware** (ASP.NET 7+):
  ```csharp
  builder.Services.AddRateLimiter(options =>
  {
      // Fixed window limiter
      options.AddFixedWindowLimiter("fixed", opt =>
      {
          opt.PermitLimit = 100;
          opt.Window = TimeSpan.FromMinutes(1);
          opt.QueueProcessingOrder = QueueProcessingOrder.OldestFirst;
          opt.QueueLimit = 10;
      });
      
      // Sliding window limiter
      options.AddSlidingWindowLimiter("sliding", opt =>
      {
          opt.PermitLimit = 100;
          opt.Window = TimeSpan.FromMinutes(1);
          opt.SegmentsPerWindow = 6;
      });
      
      // Token bucket limiter
      options.AddTokenBucketLimiter("token", opt =>
      {
          opt.TokenLimit = 1000;
          opt.ReplenishmentPeriod = TimeSpan.FromMinutes(1);
          opt.TokensPerPeriod = 100;
      });
  });
  
  app.UseRateLimiter();
  ```

- **Apply rate limiting to specific endpoints**:
  ```csharp
  [EnableRateLimiting("fixed")]
  [HttpGet]
  public async Task<IActionResult> GetOrders()
  {
      // Implementation
  }
  ```

- **Implement per-user rate limiting**:
  ```csharp
  options.GlobalLimiter = PartitionedRateLimiter.Create<HttpContext, string>(
      context =>
      {
          var username = context.User.Identity?.Name ?? "anonymous";
          
          return RateLimitPartition.GetFixedWindowLimiter(username, 
              _ => new FixedWindowRateLimiterOptions
              {
                  PermitLimit = 10,
                  Window = TimeSpan.FromMinutes(1)
              });
      });
  ```

## Anti-Patterns
- ❌ Don't apply same limits to all endpoints - differentiate by risk
- ❌ Never forget to add rejection status code handler
- ❌ Avoid too restrictive limits that harm legitimate users