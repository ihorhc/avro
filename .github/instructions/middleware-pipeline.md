---
applyTo: "src/**/Middleware/**/*.cs"
---

# Middleware Pipeline

## Guidelines
- **Order middleware correctly** in `Program.cs`:
  ```csharp
  var app = builder.Build();
  
  // 1. Exception handling (must be first)
  app.UseExceptionHandler("/error");
  
  // 2. HTTPS redirection
  app.UseHttpsRedirection();
  
  // 3. Static files (before routing)
  app.UseStaticFiles();
  
  // 4. Routing
  app.UseRouting();
  
  // 5. CORS (after routing, before auth)
  app.UseCors();
  
  // 6. Authentication (before authorization)
  app.UseAuthentication();
  
  // 7. Authorization
  app.UseAuthorization();
  
  // 8. Rate limiting
  app.UseRateLimiter();
  
  // 9. Custom middleware
  app.UseMiddleware<RequestLoggingMiddleware>();
  
  // 10. Endpoints (must be last)
  app.MapControllers();
  ```

- **Create custom middleware** with proper structure:
  ```csharp
  public class RequestLoggingMiddleware
  {
      private readonly RequestDelegate _next;
      private readonly ILogger<RequestLoggingMiddleware> _logger;
      
      public RequestLoggingMiddleware(
          RequestDelegate next,
          ILogger<RequestLoggingMiddleware> logger)
      {
          _next = next;
          _logger = logger;
      }
      
      public async Task InvokeAsync(HttpContext context)
      {
          _logger.LogInformation("Request: {Method} {Path}", 
              context.Request.Method, 
              context.Request.Path);
          
          await _next(context);
          
          _logger.LogInformation("Response: {StatusCode}", 
              context.Response.StatusCode);
      }
  }
  ```

- **Use extension methods** for middleware registration:
  ```csharp
  public static class MiddlewareExtensions
  {
      public static IApplicationBuilder UseRequestLogging(
          this IApplicationBuilder builder)
      {
          return builder.UseMiddleware<RequestLoggingMiddleware>();
      }
  }
  ```

## Anti-Patterns
- ❌ Don't place middleware in wrong order - order matters!
- ❌ Never forget to call `await _next(context)`
- ❌ Avoid heavy processing in middleware - keep it lightweight