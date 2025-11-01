---
applyTo: "src/**/Data/**/*.cs,src/**/Persistence/**/*.cs,src/**/DB.EF/**/*.cs"
---

# EF Core Patterns

## Guidelines
- **Use DbContext with dependency injection**:
  ```csharp
  services.AddDbContext<AppDbContext>(options =>
      options.UseNpgsql(  // Aurora PostgreSQL
          configuration.GetConnectionString("DefaultConnection"),
          sqlOptions => sqlOptions.EnableRetryOnFailure()));
  ```

- **Multi-tenancy with global query filters**:
  ```csharp
  public class AppDbContext : DbContext
  {
      private readonly ICurrentTenant _currentTenant;
      
      protected override void OnModelCreating(ModelBuilder modelBuilder)
      {
          // Global filter for all entities with Tenant property
          foreach (var entityType in modelBuilder.Model.GetEntityTypes())
          {
              if (typeof(ITenantEntity).IsAssignableFrom(entityType.ClrType))
              {
                  var parameter = Expression.Parameter(entityType.ClrType, "e");
                  var property = Expression.Property(parameter, nameof(ITenantEntity.Tenant));
                  var tenantId = Expression.Constant(_currentTenant.TenantId);
                  var filter = Expression.Lambda(Expression.Equal(property, tenantId), parameter);
                  
                  modelBuilder.Entity(entityType.ClrType).HasQueryFilter(filter);
              }
          }
      }
  }
  ```

- **Configure entities using Fluent API**:
  ```csharp
  public class UgiConfiguration : IEntityTypeConfiguration<UgiModel>
  {
      public void Configure(EntityTypeBuilder<UgiModel> builder)
      {
          builder.HasKey(o => o.Id);
          builder.Property(o => o.UgiCode).IsRequired().HasMaxLength(50);
          builder.Property(o => o.Tenant).IsRequired().HasMaxLength(50);
          
          builder.HasIndex(o => new { o.Tenant, o.UgiCode }).IsUnique();
          builder.HasMany(o => o.ExciseStamps).WithOne().HasForeignKey(i => i.UgiId);
      }
  }
  ```

- **Use AsNoTracking() for read-only queries**:
  ```csharp
  var orders = await _context.Orders
      .AsNoTracking()
      .Where(o => o.Status == OrderStatus.Pending)
      .ToListAsync(cancellationToken);
  ```

- **Project to DTOs in queries**:
  ```csharp
  var orderDtos = await _context.Orders
      .Select(o => new OrderDto
      {
          Id = o.Id,
          CustomerName = o.CustomerName,
          Total = o.Items.Sum(i => i.Price * i.Quantity)
      })
      .ToListAsync(cancellationToken);
  ```

- **Use transactions for multiple operations**:
  ```csharp
  using var transaction = await _context.Database.BeginTransactionAsync(ct);
  try
  {
      await _context.Orders.AddAsync(order, ct);
      await _context.SaveChangesAsync(ct);
      
      await _context.Inventory.UpdateRangeAsync(items, ct);
      await _context.SaveChangesAsync(ct);
      
      await transaction.CommitAsync(ct);
  }
  catch
  {
      await transaction.RollbackAsync(ct);
      throw;
  }
  ```

## Anti-Patterns
- ❌ Don't use lazy loading in production - causes N+1 queries
- ❌ Never track entities for read-only operations
- ❌ Avoid using `ToList()` then filtering in memory