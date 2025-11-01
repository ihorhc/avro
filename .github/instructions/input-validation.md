---
applyTo: "src/**/*.cs"
---

# Input Validation

## Guidelines
- **Use Data Annotations** for simple validation:
  ```csharp
  public class CreateOrderRequest
  {
      [Required]
      [StringLength(100, MinimumLength = 1)]
      public string CustomerName { get; set; } = string.Empty;
      
      [Required]
      [EmailAddress]
      public string Email { get; set; } = string.Empty;
      
      [Range(1, int.MaxValue)]
      public int Quantity { get; set; }
      
      [Required]
      [MinLength(1, ErrorMessage = "At least one item is required")]
      public List<OrderItemDto> Items { get; set; } = new();
  }
  ```

- **Use FluentValidation** for complex validation:
  ```csharp
  public class CreateOrderRequestValidator : AbstractValidator<CreateOrderRequest>
  {
      public CreateOrderRequestValidator()
      {
          RuleFor(x => x.CustomerName)
              .NotEmpty()
              .MaximumLength(100);
          
          RuleFor(x => x.Email)
              .NotEmpty()
              .EmailAddress();
          
          RuleFor(x => x.Items)
              .NotEmpty()
              .WithMessage("At least one item is required");
          
          RuleForEach(x => x.Items)
              .SetValidator(new OrderItemValidator());
      }
  }
  
  // Register in Program.cs
  services.AddValidatorsFromAssemblyContaining<CreateOrderRequestValidator>();
  services.AddFluentValidationAutoValidation();
  ```

- **Validate at API boundaries**:
  ```csharp
  [HttpPost]
  public async Task<IActionResult> CreateOrder(
      [FromBody] CreateOrderRequest request)
  {
      if (!ModelState.IsValid)
          return BadRequest(ModelState);
      
      // Process request
  }
  ```

- **Sanitize input to prevent XSS**:
  ```csharp
  using Microsoft.AspNetCore.Html;
  using System.Text.Encodings.Web;
  
  var sanitized = HtmlEncoder.Default.Encode(userInput);
  ```

## Anti-Patterns
- ❌ Never trust client input - always validate server-side
- ❌ Don't rely solely on client-side validation
- ❌ Avoid exposing detailed validation errors to end users (security risk)