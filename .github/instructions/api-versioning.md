---
applyTo: "src/**/Controllers/**/*.cs"
---

# API Versioning

## Guidelines
- **Use Asp.Versioning.Http** package for API versioning:
  ```csharp
  builder.Services.AddApiVersioning(options =>
  {
      options.DefaultApiVersion = new ApiVersion(1, 0);
      options.AssumeDefaultVersionWhenUnspecified = true;
      options.ReportApiVersions = true;
      options.ApiVersionReader = new UrlSegmentApiVersionReader();
  })
  .AddApiExplorer(options =>
  {
      options.GroupNameFormat = "'v'VVV";
      options.SubstituteApiVersionInUrl = true;
  });
  ```

- **Version controllers using attributes**:
  ```csharp
  [ApiController]
  [Route("api/v{version:apiVersion}/[controller]")]
  [ApiVersion("1.0")]
  [ApiVersion("2.0")]
  public class OrdersController : ControllerBase
  {
      [HttpGet]
      [MapToApiVersion("1.0")]
      public async Task<IActionResult> GetOrdersV1()
      {
          // Version 1 implementation
      }
      
      [HttpGet]
      [MapToApiVersion("2.0")]
      public async Task<IActionResult> GetOrdersV2()
      {
          // Version 2 implementation
      }
  }
  ```

- **Use header-based versioning** as alternative:
  ```csharp
  options.ApiVersionReader = new HeaderApiVersionReader("X-API-Version");
  ```

- **Deprecate old versions**:
  ```csharp
  [ApiVersion("1.0", Deprecated = true)]
  ```

## Anti-Patterns
- ❌ Don't break existing API versions
- ❌ Never version every small change - version for breaking changes
- ❌ Avoid supporting too many versions simultaneously