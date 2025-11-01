# Performance Tuning Prompt

Use this prompt for performance optimization and profiling guidance.

## Performance Analysis Areas

### 1. Database Performance
- [ ] **Query Optimization**
  - Use `AsNoTracking()` for read-only queries
  - Project to DTOs in queries (avoid SELECT *)
  - Avoid N+1 queries - use `Include()` or split queries
  - Use compiled queries for hot paths
  - Check query execution plans

- [ ] **Indexing**
  - Add indexes for frequently queried columns
  - Include indexes for covering queries
  - Remove unused indexes
  - Monitor index fragmentation

- [ ] **Connection Management**
  - Use connection pooling
  - Dispose DbContext properly
  - Monitor connection pool exhaustion

### 2. API Performance
- [ ] **Response Time**
  - Target: < 200ms for simple queries
  - Target: < 1s for complex operations
  - Use async/await consistently
  - Implement timeout policies

- [ ] **Caching**
  - Use distributed caching (Redis) for shared data
  - Use memory cache for single-instance data
  - Implement cache invalidation strategy
  - Set appropriate cache expiration

- [ ] **Response Optimization**
  - Enable response compression
  - Minimize response payload size
  - Use pagination for large result sets
  - Consider GraphQL for complex queries

### 3. Memory Management
- [ ] Use `ValueTask<T>` for frequently called async methods
- [ ] Pool large objects with `ArrayPool<T>`
- [ ] Dispose IDisposable objects properly
- [ ] Monitor memory leaks with diagnostic tools
- [ ] Use `Span<T>` and `Memory<T>` for high-performance scenarios

### 4. Async/Concurrency
- [ ] Avoid blocking calls (`.Result`, `.Wait()`)
- [ ] Use `ConfigureAwait(false)` in libraries
- [ ] Implement proper cancellation token handling
- [ ] Use `Parallel.ForEachAsync` for CPU-bound work
- [ ] Monitor thread pool starvation

### 5. Serialization
- [ ] Use `System.Text.Json` (faster than Newtonsoft.Json)
- [ ] Configure source generators for JSON
- [ ] Avoid circular references
- [ ] Use minimal API models

## Performance Testing

### Load Testing with k6
```javascript
import http from 'k6/http';
import { check, sleep } from 'k6';

export let options = {
  stages: [
    { duration: '2m', target: 100 }, // Ramp up
    { duration: '5m', target: 100 }, // Steady state
    { duration: '2m', target: 0 },   // Ramp down
  ],
  thresholds: {
    http_req_duration: ['p(95)<500'], // 95% under 500ms
  },
};

export default function() {
  let response = http.get('https://api.avro.com/api/v1/orders');
  check(response, { 'status is 200': (r) => r.status === 200 });
  sleep(1);
}
```

### Benchmarking with BenchmarkDotNet
```csharp
[MemoryDiagnoser]
public class OrderServiceBenchmarks
{
    private OrderService _service;
    
    [GlobalSetup]
    public void Setup()
    {
        _service = new OrderService(/* deps */);
    }
    
    [Benchmark]
    public async Task GetOrders()
    {
        await _service.GetOrdersAsync(1, 20);
    }
}
```

## Profiling Tools
- **Application Insights** - Production monitoring
- **dotnet-trace** - CPU profiling
- **dotnet-counters** - Real-time metrics
- **dotnet-dump** - Memory analysis
- **MiniProfiler** - Database query profiling
- **BenchmarkDotNet** - Micro-benchmarking

## Common Performance Patterns

### Before (Slow)
```csharp
// N+1 Query Problem
var orders = await _context.Orders.ToListAsync();
foreach (var order in orders)
{
    order.Customer = await _context.Customers.FindAsync(order.CustomerId);
}
```

### After (Fast)
```csharp
// Single query with Include
var orders = await _context.Orders
    .Include(o => o.Customer)
    .ToListAsync();
```

## Usage

In Copilot Chat:
```
@workspace /performance-tuning Analyze the OrderService for performance bottlenecks
```