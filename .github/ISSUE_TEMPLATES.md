# AI-Ready Issue Templates

This directory contains templates for creating issues that can be processed by the AI-powered development pipeline.

## How to Use

1. Create a new issue in GitHub
2. Use one of the templates below
3. Add the `ai-ready` label
4. The AI pipeline will automatically process your issue

---

## Template: New Feature

```markdown
**Title:** Implement [Feature Name]

**Labels:** ai-ready, feature

**Description:**

### Overview
[Brief description of the feature and its purpose]

### Requirements
- [ ] Requirement 1
- [ ] Requirement 2
- [ ] Requirement 3

### Technical Details
- **Architecture Pattern:** [e.g., CQRS, Event-Driven, etc.]
- **Services Affected:** [List of services]
- **Database Changes:** [Yes/No - describe if yes]
- **API Endpoints:** [List new endpoints]

### Acceptance Criteria
- [ ] Feature works as described
- [ ] Unit tests with >80% coverage
- [ ] Integration tests included
- [ ] API documentation updated
- [ ] Performance requirements met

### Agent Instructions (Optional)
@architect: [Specific architectural guidance]
@implementation: [Specific implementation notes]
@testing: [Specific test requirements]
@devops: [Specific deployment needs]

### Additional Context
[Any additional information, diagrams, or references]
```

**Example:**
```markdown
**Title:** Implement notification service

**Labels:** ai-ready, feature

**Description:**

### Overview
Create a notification service that handles email, SMS, and push notifications across the platform.

### Requirements
- [ ] Email notifications via SendGrid
- [ ] SMS notifications via Twilio
- [ ] Push notifications via Firebase
- [ ] Multi-tenant support
- [ ] Async processing with MassTransit
- [ ] Notification templates
- [ ] Delivery tracking

### Technical Details
- **Architecture Pattern:** Event-Driven with CQRS
- **Services Affected:** Avro.Notifications (new service)
- **Database Changes:** Yes - new NotificationRequest aggregate, NotificationLog table
- **API Endpoints:**
  - POST /api/v1/notifications/send
  - GET /api/v1/notifications/{id}
  - GET /api/v1/notifications/status/{requestId}

### Acceptance Criteria
- [ ] All notification channels working
- [ ] Events trigger notifications correctly
- [ ] Templates render properly
- [ ] Delivery status tracked
- [ ] Tests cover happy path and errors
- [ ] Performance: <500ms p95 for dispatch

### Agent Instructions
@architect: Follow BEST template structure for new service
@implementation: Use notification abstraction pattern, implement retry logic
@testing: Include end-to-end tests with mock providers
@devops: Deploy as separate ECS service with auto-scaling

### Additional Context
Reference similar implementation in Orders service for event handling patterns.
```

---

## Template: Bug Fix

```markdown
**Title:** Fix [Bug Description]

**Labels:** ai-ready, bug

**Description:**

### Bug Description
[Clear description of the bug]

### Steps to Reproduce
1. Step 1
2. Step 2
3. Step 3

### Expected Behavior
[What should happen]

### Actual Behavior
[What actually happens]

### Environment
- Service: [Service name]
- Version: [Version if known]
- Environment: [Dev/Staging/Production]

### Error Logs
```
[Paste relevant error logs]
```

### Proposed Solution
[If known, describe the fix needed]

### Agent Instructions (Optional)
@implementation: [Specific fix guidance]
@testing: Add regression tests for this scenario
```

**Example:**
```markdown
**Title:** Fix multi-tenant query filter not applied in OrderRepository

**Labels:** ai-ready, bug, security

**Description:**

### Bug Description
The OrderRepository.GetOrdersAsync method is not applying tenant filter, allowing cross-tenant data access.

### Steps to Reproduce
1. Create order as Tenant A
2. Query orders as Tenant B
3. Observe that Tenant B can see Tenant A's orders

### Expected Behavior
Each tenant should only see their own orders

### Actual Behavior
All tenants can see all orders

### Environment
- Service: Avro.Orders.Infrastructure
- Version: Latest main
- Environment: Staging (caught in testing)

### Error Logs
```
No error - data leak vulnerability
```

### Proposed Solution
Add .Where(o => o.Tenant == currentTenant) to the query in OrderRepository.GetOrdersAsync

### Agent Instructions
@implementation: Apply tenant filter, audit all repository methods
@testing: Add multi-tenant test scenarios for all queries
@review: Verify no other repositories have same issue
```

---

## Template: Performance Improvement

```markdown
**Title:** Optimize [Component/Feature]

**Labels:** ai-ready, performance

**Description:**

### Performance Issue
[Description of the performance problem]

### Current Metrics
- Metric 1: [e.g., p95 latency: 2s]
- Metric 2: [e.g., throughput: 100 req/s]
- Metric 3: [e.g., memory usage: 500MB]

### Target Metrics
- Metric 1: [e.g., p95 latency: <500ms]
- Metric 2: [e.g., throughput: >500 req/s]
- Metric 3: [e.g., memory usage: <200MB]

### Identified Issues
- Issue 1: [e.g., N+1 query problem]
- Issue 2: [e.g., Missing database index]

### Proposed Optimizations
- Optimization 1
- Optimization 2

### Agent Instructions
@implementation: Add caching layer, optimize queries
@testing: Include performance benchmarks
@review: Validate no regression in functionality
```

---

## Template: Refactoring

```markdown
**Title:** Refactor [Component/Pattern]

**Labels:** ai-ready, refactoring

**Description:**

### Current State
[Description of current implementation]

### Problems
- Problem 1
- Problem 2

### Proposed Refactoring
[Description of desired end state]

### Benefits
- Benefit 1
- Benefit 2

### Constraints
- Must maintain backward compatibility
- No behavior changes
- All existing tests must pass

### Agent Instructions
@architect: Validate refactoring aligns with platform patterns
@implementation: Maintain existing API contracts
@testing: Ensure 100% test coverage maintained
```

---

## Template: Infrastructure/DevOps

```markdown
**Title:** Setup [Infrastructure Component]

**Labels:** ai-ready, infrastructure

**Description:**

### Infrastructure Need
[Description of what needs to be set up]

### Requirements
- Requirement 1
- Requirement 2

### Technology Stack
[Specific technologies to use]

### Acceptance Criteria
- [ ] Infrastructure provisioned
- [ ] Monitoring configured
- [ ] Alerts set up
- [ ] Documentation updated

### Agent Instructions
@devops: [Specific infrastructure requirements]
@architect: Validate infrastructure aligns with platform standards
```

---

## Advanced: Parallel Feature Development

For complex features requiring multiple issues:

```markdown
**Epic:** [Feature Name]

**Sub-Issues (all with ai-ready label):**
1. Issue #123: Backend API implementation
2. Issue #124: Database schema and migrations
3. Issue #125: Event handlers
4. Issue #126: Admin UI

Each sub-issue will be processed in parallel by the AI pipeline.
```

---

## Best Practices

### ✅ DO
- Be specific and detailed in requirements
- Provide clear acceptance criteria
- Include relevant context and examples
- Specify technical constraints
- Give agent-specific instructions when needed

### ❌ DON'T
- Keep descriptions vague
- Skip acceptance criteria
- Forget to add `ai-ready` label
- Mix multiple unrelated features
- Skip technical details

---

## Monitoring Pipeline Execution

After creating an AI-ready issue:

1. **Check Comments**: AI will comment with progress updates
2. **Watch Labels**: Labels change as pipeline progresses
   - `ai-processing` → Currently running
   - `ai-completed` → PR created
   - `ai-failed` → Error occurred
3. **Review PR**: Check the auto-generated pull request
4. **Merge**: Approve and merge when ready

---

For more information, see the [AI Pipeline Documentation](../AI_PIPELINE.md).
