---
applyTo: "src/**/Data/**/*.cs,src/**/Repositories/**/*.cs"
---

# SQL Injection Prevention

## Guidelines
- **Always use parameterized queries** with EF Core (automatic):
  ```csharp
  // ✅ Safe - EF Core uses parameterized queries
  var orders = await _context.Orders
      .Where(o => o.CustomerName == customerName)
      .ToListAsync();
  ```

- **Use parameters with raw SQL**:
  ```csharp
  // ✅ Safe - parameterized
  var orders = await _context.Orders
      .FromSqlRaw("SELECT * FROM Orders WHERE CustomerId = {0}", customerId)
      .ToListAsync();
  
  // Or use interpolation (converted to parameters)
  var orders = await _context.Orders
      .FromSql($"SELECT * FROM Orders WHERE CustomerId = {customerId}")
      .ToListAsync();
  ```

- **Never concatenate user input into SQL**:
  ```csharp
  // ❌ DANGEROUS - SQL injection vulnerability
  var sql = $"SELECT * FROM Orders WHERE CustomerId = {userInput}";
  var orders = await _context.Orders.FromSqlRaw(sql).ToListAsync();
  
  // ❌ DANGEROUS - string concatenation
  var sql = "SELECT * FROM Orders WHERE Name = '" + userName + "'";
  ```

- **Validate and whitelist dynamic table/column names**:
  ```csharp
  private static readonly HashSet<string> AllowedSortColumns = new()
  {
      "CustomerName", "OrderDate", "TotalAmount"
  };
  
  public async Task<List<Order>> GetOrdersSortedAsync(string sortColumn)
  {
      if (!AllowedSortColumns.Contains(sortColumn))
          throw new ArgumentException("Invalid sort column");
      
      var sql = $"SELECT * FROM Orders ORDER BY {sortColumn}";
      return await _context.Orders.FromSqlRaw(sql).ToListAsync();
  }
  ```

## Anti-Patterns
- ❌ Never use string concatenation for SQL queries
- ❌ Don't use `FromSqlRaw` with unsanitized user input
- ❌ Avoid dynamic SQL unless absolutely necessary