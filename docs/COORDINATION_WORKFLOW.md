---
name: Avro Multi-Agent Coordination Workflow
description: Complete guide for parallel development with specialized agents
---

# Multi-Agent Coordination Workflow

This document describes how the 5 specialized agents work together to accelerate feature development from weeks to days through parallel execution and clear handoff points.

## The 5-Agent Architecture

| Agent | Role | Key Responsibility | Parallel Work |
|-------|------|-------------------|----------------|
| **Architect** | Strategic Design | Validates design, creates implementation strategy | Defines structure while others prepare |
| **Implementation** | Code Creation | Writes production code per specifications | Codes in parallel with test design |
| **Testing** | Quality Assurance | Creates comprehensive test suites | Writes tests in parallel with code |
| **Review** | Quality Gate | Validates code quality and compliance | Reviews submissions immediately |
| **DevOps** | Operations | Manages deployment automation | Prepares infrastructure concurrently |

## Workflow Phases

### Phase 1: Requirement & Design (Day 1)

**Trigger**: User story/feature request enters the sprint

**Architect Agent**:
```markdown
## Design Document
- [ ] Parse requirements
- [ ] Create architecture diagram
- [ ] Define domain boundaries
- [ ] Design API contracts
- [ ] Identify integration points
- [ ] List dependencies
- [ ] Create implementation strategy
```

**Deliverable**: `DESIGN.md` with:
- Architecture diagram (ASCII or PlantUML)
- Domain models and value objects
- API contracts (request/response)
- Database schema
- Integration points
- Risk assessment

**Example Design Output**:
```markdown
# Order Service Design

## Architecture
```
┌─────────────────┐
│   Order API     │
├─────────────────┤
│ Application     │
│  - Handlers     │
│  - Validators   │
├─────────────────┤
│ Domain          │
│  - Aggregates   │
│  - Events       │
├─────────────────┤
│ Infrastructure  │
│  - Repository   │
│  - EF Mappings  │
└─────────────────┘
```

## Domain Models
- OrderAggregate: Root entity managing order lifecycle
- OrderItem: Value object for line items
- OrderStatus: Enum for state management
- OrderConfirmedEvent: Domain event

## API Contracts
POST /api/v1/orders
- Request: CreateOrderRequest { customerId, items[] }
- Response: OrderDto { orderId, status, total }

## Dependencies
- IOrderRepository: Persistence
- IMediator: Event publishing
- ILogger: Observability
```

**Next Step**: Pass to Implementation, Testing, and DevOps agents

---

### Phase 2: Parallel Execution (Days 2-3)

Three agents work in parallel on separate concerns:

#### Implementation Agent Path

**Takes**: DESIGN.md

**Executes**:
1. Create domain entities and aggregates
2. Implement application layer (handlers, validators)
3. Create repository interfaces
4. Implement EF Core mappings
5. Create API endpoints

**Outputs**:
- Domain layer: `OrderAggregate.cs`, `OrderItem.cs`, `OrderConfirmedEvent.cs`
- Application layer: `CreateOrderHandler.cs`, `CreateOrderValidator.cs`
- Infrastructure layer: `OrderRepository.cs`, `OrderConfiguration.cs`
- API layer: `OrdersEndpoints.cs`

**Coordination Points**:
```csharp
// ✅ Public interfaces designed for testing
public interface IOrderRepository
{
    Task<Order?> GetByIdAsync(string id, CancellationToken ct);
    Task AddAsync(Order order, CancellationToken ct);
}

// ✅ Async patterns enable Testing Agent to mock
public async Task<OrderDto> Handle(CreateOrderCommand req, CancellationToken ct)
```

#### Testing Agent Path

**Takes**: DESIGN.md (while waiting for implementation)

**Executes in Parallel**:
1. Design test strategy
2. Create test fixtures and builders
3. Define test data
4. Write unit test shells

**Phase 2a - Before Implementation Complete**:
```csharp
// Create fixtures and builders
public class OrderTestDataBuilder
{
    public Order Build() => /* ... */
}

// Create test templates
[Fact]
public async Task Create_WithValidData_ReturnsOrder()
{
    // [Arrange - test builder ready]
    // [Act - awaiting implementation]
    // [Assert - test expectations defined]
}
```

**Phase 2b - As Implementation Completes**:
```csharp
[Fact]
public async Task Create_WithValidData_ReturnsOrder()
{
    // Arrange
    var cmd = new CreateOrderCommand { /* data */ };
    var handler = new CreateOrderHandler(_repo, _publisher);
    
    // Act
    var result = await handler.Handle(cmd, ct);
    
    // Assert
    result.Should().NotBeNull();
}
```

**Integration Test Preparation**:
```csharp
// Create test database setup (TestContainer ready)
public class OrderHandlerIntegrationTests : IAsyncLifetime
{
    private readonly MsSqlContainer _container;
    
    public async Task InitializeAsync()
        => await _container.StartAsync();
}
```

**Output**: `OrderServiceTests.cs`, `OrderHandlerIntegrationTests.cs`, `OrderTestDataBuilder.cs`

#### DevOps Agent Path

**Takes**: DESIGN.md

**Executes in Parallel**:
1. Design infrastructure requirements
2. Create infrastructure as code (AWS CDK)
3. Configure environments
4. Set up CI/CD pipeline
5. Create deployment automation

**Output**:
- `infra/avro-order-service.ts`: AWS CDK stack
- `.github/workflows/deploy-order-service.yml`: GitHub Actions
- `deployment/staging.yml`: Staging environment config
- `deployment/production.yml`: Production environment config
- Health checks and monitoring

---

### Phase 3: Integration (Day 4)

**Testing Agent** creates integration tests with implementation:

```csharp
// Now that code exists, create full integration tests
[Fact]
public async Task CreateOrder_PersistsToDatabase()
{
    // Create real database (using TestContainer)
    var order = new Order { /* populated by builder */ };
    await _context.Orders.AddAsync(order);
    await _context.SaveChangesAsync();
    
    // Verify in database
    var persisted = await _context.Orders.FindAsync(order.Id);
    persisted.Should().NotBeNull();
}
```

**Review Agent** begins static analysis:

```markdown
## Code Quality Review (In Progress)
- [✅] Compiles successfully
- [✅] No critical security issues
- [⏳] Code coverage analysis (tests running)
- [⏳] Performance profiling (integration tests)
- [⏳] Architecture validation
```

**DevOps Agent** prepares deployment infrastructure:

```bash
# Infrastructure deployed to staging
aws cloudformation deploy \
  --template-file infra/output.yml \
  --stack-name avro-order-service-staging

# Health checks configured
curl https://staging-api.avro.com/health
```

---

### Phase 4: Quality Assurance (Days 4-5)

**Testing Agent** finalizes all test suites:

```markdown
## Test Coverage Report
- Unit Tests: 85% coverage
- Integration Tests: All critical paths
- E2E Tests: Order creation flow
- Performance: All tests <500ms
- Status: ✅ Ready for review
```

**Review Agent** performs comprehensive validation:

```markdown
## Code Review Results

### ✅ Approval Criteria Met
- Security: Zero vulnerabilities
- Performance: Within SLA
- Architecture: Compliant with standards
- Testing: 85% coverage achieved
- Code Quality: No critical issues

### Status: APPROVED FOR MERGE
```

**Branches Ready**:
- `feature/order-service-domain`: Domain layer
- `feature/order-service-api`: API endpoints
- `feature/order-service-tests`: Test suite
- `infra/order-service-iac`: Infrastructure

---

### Phase 5: Merge & Deploy (Day 5+)

**Sequence**:

1. **Merge to main** (Review Agent approval):
   ```bash
   # All branch checks pass
   git merge --squash feature/order-service-domain
   git merge --squash feature/order-service-api
   git merge --squash feature/order-service-tests
   git merge --squash infra/order-service-iac
   ```

2. **CI/CD Triggers** (DevOps Agent):
   ```
   → Build
   → Test (1000+ tests in parallel)
   → Security Scan
   → Build Docker Image
   → Push to ECR
   → Deploy to Staging
   → Smoke Tests
   → Ready for Production
   ```

3. **Production Deployment** (Manual approval):
   ```bash
   # Blue-green deployment
   # Rollback available if issues
   # Monitoring active
   ```

---

## Agent Communication Protocol

### Status Updates

**Format**: Each agent posts status in shared task.md

```markdown
# Feature: Order Service Implementation

## Architect Agent Status
- [x] Design document completed
- [x] API contracts defined
- [x] Domain models approved
- Status: ✅ COMPLETE - Ready for handoff

## Implementation Agent Status
- [x] Domain layer complete
- [x] Application handlers complete
- [ ] API endpoints (in progress)
- [ ] Error handling (pending)
- Status: 🟡 IN PROGRESS - 60% complete

## Testing Agent Status
- [x] Test fixtures created
- [x] Unit tests defined
- [ ] Integration tests writing
- [ ] Performance tests pending
- Status: 🟡 IN PROGRESS - 50% complete

## Review Agent Status
- [ ] Static analysis (pending)
- [ ] Security scan (pending)
- [ ] Performance review (pending)
- Status: ⏳ NOT STARTED - Awaiting implementation

## DevOps Agent Status
- [x] Infrastructure as code
- [x] CI/CD pipeline created
- [x] Environments configured
- Status: ✅ COMPLETE - Staging ready
```

### Handoff Points

**Architect → Implementation**:
```
READY WHEN:
- Design document complete
- API contracts finalized
- Domain models approved

HANDOFF INCLUDES:
- DESIGN.md with diagrams
- Example implementations
- Testing strategy document
```

**Implementation → Testing**:
```
READY WHEN:
- Public interfaces designed
- Sample implementations provided
- Can be mocked/stubbed

HANDOFF INCLUDES:
- Code skeleton
- Interface definitions
- Mockable dependencies
```

**Testing → Review**:
```
READY WHEN:
- All tests written
- Coverage >80%
- Tests passing

HANDOFF INCLUDES:
- Test suite with examples
- Coverage report
- Performance benchmark results
```

**All → DevOps**:
```
READY WHEN:
- Code complete
- Tests passing
- Review approved

HANDOFF INCLUDES:
- Compiled artifacts
- Docker image
- Configuration files
```

---

## Parallel Execution Timeline

```
DAY 1: Design Phase
  Architect: ████████████████ (Design complete)
  
DAY 2-3: Parallel Development
  Architect: ✅ (Monitoring)
  Impl:      ████████ (Domain layer)
  Testing:   ████████ (Test fixtures)
  DevOps:    ████████ (Infrastructure)
  Review:    ⏳ (Waiting for code)
  
DAY 4: Integration & Review
  Architect: ✅ (Design validation)
  Impl:      ████████████████ (API complete)
  Testing:   ██████████████ (Tests complete)
  DevOps:    ████████████████ (Staged)
  Review:    ████████████ (In progress)
  
DAY 5: Quality Gate & Deploy
  Architect: ✅ (Sign-off)
  Impl:      ✅ (Sign-off)
  Testing:   ✅ (Coverage approved)
  DevOps:    ⏳ (Deploy to staging)
  Review:    ████████████████ (Ready to merge)
  
DAY 5+: Production
  All:       ✅ DEPLOYED TO PRODUCTION
```

---

## Dependency Management

### Critical Path
```
Architect → Implementation ──┐
                              ├→ Testing → Review → DevOps Deploy
DevOps (parallel) ────────────┘
```

### No Blocking Dependencies
- Testing doesn't wait for complete implementation (uses mocks)
- DevOps doesn't wait for code (uses infrastructure as code)
- Review runs after completion (quality gate, not blocker)

### Coordination Points
1. **Design Approval**: Architect signs off (before implementation starts)
2. **API Contract Lock**: Implementation locks contracts (before Testing finalizes)
3. **Code Completion**: Implementation signals ready (Testing finalizes)
4. **Test Approval**: Testing signals coverage OK (Review begins)
5. **Review Gate**: Review approves (before merge)
6. **Merge Approval**: All agents sign off (before deploy)

---

## Tools & Channels

### Issue Tracking
- GitHub Issues: Feature tracking
- PR Comments: Agent feedback
- Status Updates: Shared task.md file

### Communication
- Slack: Real-time coordination
- PR Reviews: Code feedback
- GitHub Discussions: Architecture decisions

### Automation
- GitHub Actions: Build, test, deploy
- CodeQL: Security scanning
- SonarCloud: Code quality
- Codecov: Coverage tracking

---

## Success Metrics

### Development Velocity
- **Before**: 2-3 weeks per feature
- **After**: 3-5 days per feature
- **Improvement**: 80% reduction in time-to-delivery

### Quality Metrics
- Code Coverage: >80%
- Security Issues: Zero critical in production
- Bug Escape Rate: <5% to production
- Performance: <5% degradation per release

### Reliability Metrics
- Build Success: 99%+
- Test Pass Rate: 100%
- Deployment Success: 100%
- Rollback Frequency: <1 per quarter

---

## Anti-Patterns to Avoid

### ❌ Serial Execution
```
Architect → Implementation → Testing → Review → Deploy
(Takes 2-3 weeks)
```

### ✅ Parallel Execution
```
                ┌→ Implementation
Architect ─────┤→ Testing (with stubs)
                └→ DevOps (infrastructure)
(Takes 3-5 days)
```

### ❌ Blocking on Completion
```
Review waits for all tests to pass before starting
(Creates bottleneck)
```

### ✅ Progressive Review
```
Review begins with architecture review, continues with code,
completes with test validation (continuous feedback)
```

### ❌ Agent Overlap
```
Testing writes implementation code
Review writes tests
(Causes confusion and rework)
```

### ✅ Clear Boundaries
```
- Architect: Design only
- Implementation: Code only
- Testing: Tests only
- Review: Quality validation only
- DevOps: Deployment only
```

---

## Escalation Procedures

### If Design Issues Found During Implementation
1. Implementation Agent flags issue in task.md
2. Architect Agent reviews and either:
   - Approves workaround (if minor)
   - Requests design amendment (if significant)
3. Communication continues in GitHub discussion
4. No progress block - continue with approved changes

### If Tests Can't Achieve Coverage Target
1. Testing Agent documents gaps
2. Review Agent suggests code improvements
3. Implementation Agent makes targeted enhancements
4. Cycle back to testing
5. Escalate if structural issue

### If Infrastructure Blocks Deployment
1. DevOps Agent identifies blocker
2. Coordinate with AWS/ops team
3. Implement workaround if available
4. Document for future improvements
5. Continue with alternative deployment path

---

## Continuous Improvement

Every 2 weeks:
1. Review metrics from last 2-week cycle
2. Identify bottlenecks
3. Update coordination workflow
4. Share learnings with team
5. Adjust agent responsibilities if needed

