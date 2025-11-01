---
applyTo: "src/**/Repositories/**/*.cs"
---

# Repository Pattern

## Guidelines
- **Define generic repository interface**:
  ```csharp
  public interface IRepository<T> where T : class
  {
      Task<T?> GetByIdAsync(int id, CancellationToken ct = default);
      Task<IEnumerable<T>> GetAllAsync(CancellationToken ct = default);
      Task<T> AddAsync(T entity, CancellationToken ct = default);
      Task UpdateAsync(T entity, CancellationToken ct = default);
      Task DeleteAsync(int id, CancellationToken ct = default);
  }
  ```

- **Implement entity-specific repositories** for complex queries:
  ```csharp
  public interface IOrderRepository : IRepository<Order>
  {
      Task<IEnumerable<Order>> GetPendingOrdersAsync(CancellationToken ct = default);
      Task<Order?> GetOrderWithItemsAsync(int id, CancellationToken ct = default);
  }
  
  public class OrderRepository : Repository<Order>, IOrderRepository
  {
      public OrderRepository(AppDbContext context) : base(context) { }
      
      public async Task<IEnumerable<Order>> GetPendingOrdersAsync(CancellationToken ct)
      {
          return await _context.Orders
              .Where(o => o.Status == OrderStatus.Pending)
              .ToListAsync(ct);
      }
  }
  ```

- **Use Unit of Work pattern** to manage transactions:
  ```csharp
  public interface IUnitOfWork : IDisposable
  {
      IOrderRepository Orders { get; }
      ICustomerRepository Customers { get; }
      Task<int> SaveChangesAsync(CancellationToken ct = default);
  }
  ```

## Anti-Patterns
- ❌ Don't create a repository for every entity - only for aggregates
- ❌ Never expose IQueryable from repositories
- ❌ Avoid generic repositories for all use cases - be pragmatic