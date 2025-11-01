# API Review Prompt

Use this prompt to conduct thorough API endpoint reviews.

## Review Checklist

### 1. API Design
- [ ] RESTful conventions followed (GET, POST, PUT, DELETE)
- [ ] Proper HTTP status codes used (200, 201, 400, 404, 500)
- [ ] Consistent naming conventions
- [ ] API versioning applied
- [ ] Pagination implemented for list endpoints
- [ ] Filtering and sorting options available

### 2. Request/Response
- [ ] DTOs used (not domain entities)
- [ ] Request validation implemented (FluentValidation or Data Annotations)
- [ ] Response shapes are consistent
- [ ] Error responses follow standard format
- [ ] Content-Type and Accept headers handled

### 3. Security
- [ ] Authentication required where appropriate
- [ ] Authorization policies applied
- [ ] Input validation present
- [ ] No sensitive data in responses
- [ ] Rate limiting configured
- [ ] CORS properly configured

### 4. Performance
- [ ] Async/await used for all I/O operations
- [ ] Database queries optimized (no N+1)
- [ ] Caching strategy considered
- [ ] Response compression enabled
- [ ] Unnecessary data not included in responses

### 5. Error Handling
- [ ] Global exception handler in place
- [ ] Specific exceptions for business rules
- [ ] User-friendly error messages
- [ ] Errors logged with proper severity
- [ ] Stack traces not exposed in production

### 6. Documentation
- [ ] XML comments on controller actions
- [ ] Swagger/OpenAPI annotations
- [ ] Example requests/responses documented
- [ ] API versioning documented

## Example Review

```csharp
[ApiController]
[Route("api/v{version:apiVersion}/[controller]")]
[ApiVersion("1.0")]
[EnableRateLimiting("fixed")]
public class OrdersController : ControllerBase
{
    /// <summary>
    /// Retrieves a paginated list of orders
    /// </summary>
    /// <param name="page">Page number (default: 1)</param>
    /// <param name="pageSize">Items per page (default: 20, max: 100)</param>
    [HttpGet]
    [ProducesResponseType(typeof(PagedResult<OrderDto>), StatusCodes.Status200OK)]
    public async Task<ActionResult<PagedResult<OrderDto>>> GetOrders(
        [FromQuery] int page = 1,
        [FromQuery] int pageSize = 20,
        CancellationToken cancellationToken = default)
    {
        if (pageSize > 100)
            return BadRequest("Page size cannot exceed 100");
        
        var query = new GetOrdersQuery(page, pageSize);
        var result = await _mediator.Send(query, cancellationToken);
        
        return Ok(result);
    }
}
```

## Usage

In Copilot Chat:
```
@workspace /api-review Review the OrdersController for best practices
```