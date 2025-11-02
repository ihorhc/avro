# Quick Reference: Task Event Triggering & Rescheduling

## What This Feature Does

Enables DevOps engineers and AI agents to:
- ✅ **Manually trigger** tasks via API
- ✅ **Reschedule** failed tasks with exponential backoff
- ✅ **Trigger conditionally** based on task dependencies
- ✅ **Query event history** with filtering and search
- ✅ **Perform bulk operations** on multiple tasks
- ✅ **Receive notifications** when tasks are triggered/rescheduled

## Core APIs

### 1. Manual Trigger
```bash
POST /api/v1/tasks/{taskId}/trigger
```
**What it does**: Immediately trigger a task execution
**Response**: `{ executionId, estimatedCompletion }`
**Idempotent**: Yes (with X-Idempotency-Key header)

### 2. Reschedule Task
```bash
PUT /api/v1/tasks/{taskId}/reschedule
```
**Strategies**:
- `immediate` - Execute as soon as possible
- `delayed` - Wait N minutes/hours
- `exponential_backoff` - 2^retryCount delay
- `cron` - Custom cron schedule
- `conditional` - Trigger when dependencies complete

### 3. Query Event History
```bash
GET /api/v1/tasks/{taskId}/events?types=TaskTriggered,TaskRescheduled&from=2025-01-01&to=2025-01-31
```
**Filters**: Event type, date range, actor, pagination

### 4. Bulk Operations
```bash
POST /api/v1/tasks/bulk/trigger
PUT /api/v1/tasks/bulk/reschedule
```
**Supports**: Batch processing with per-task results and transactional modes

## Key Features

### Idempotency
- Prevent duplicate executions with `X-Idempotency-Key`
- Keys retained for 24 hours
- Multiple calls with same key return identical result

### Exponential Backoff
- Formula: `delay = baseDelay * (2 ^ retryCount)`
- Example: baseDelay=1m, retryCount=2 → delay=4m
- Includes jitter to prevent thundering herd

### Conditional Triggering
- Trigger only when dependencies complete
- Circular dependency detection
- Transitive dependency resolution
- Fail fast with detailed condition report

### Event Audit Trail
- Immutable append-only log
- Every state change captured
- Complete metadata (actor, timestamp, previous/new state)
- Enables full compliance and debugging

### Event Types
- `TaskCreated` - Task created
- `TaskTriggered` - Task manually triggered
- `TaskRescheduled` - Task rescheduling scheduled
- `TaskStarted` - Task execution started
- `TaskCompleted` - Task completed successfully
- `TaskFailed` - Task execution failed
- `TaskCancelled` - Task cancelled

## Performance Targets

| Operation | Target Latency | Throughput |
|-----------|-----------------|------------|
| Manual trigger | <500ms (P95) | - |
| Reschedule | <300ms (P95) | - |
| Event query | <1s P95 (10K events) | - |
| Bulk operations | <2s (100 tasks) | 5,000+ tasks/min |

## Authorization & Security

### RBAC Permissions
- `task:trigger` - Required to manually trigger tasks
- `task:reschedule` - Required to reschedule tasks
- `task:read-events` - Required to query event history

### Rate Limits
- Per-user: 1,000 triggers/hour
- Per-task: 100 triggers/hour
- System: 10,000 triggers/minute

### Multi-Tenancy
- All events scoped to tenant
- No cross-tenant visibility
- Per-tenant rate limiting

## Use Cases

### DevOps Operations
```
Failed deployment task
  ↓
POST /api/v1/tasks/{id}/trigger  (immediate retry)
  ↓
Task executes successfully
  ↓
Deployment complete
```

### Resilient Retry Pattern
```
Task fails
  ↓
PUT /api/v1/tasks/{id}/reschedule with exponential_backoff
  ↓
Retry after: 1m, 2m, 4m, 8m, 16m, 32m, 1h...
  ↓
Task succeeds on retry 3
```

### AI Workflow Coordination
```
Task A completes
  ↓
Trigger Task B (conditional on A success)
  ↓
Trigger Task C (conditional on A AND B success)
  ↓
Trigger Task D (alternative if B fails)
  ↓
Workflow completes or rolls back
```

## Example Requests

### Trigger with Context
```json
POST /api/v1/tasks/task-123/trigger
Headers: X-Idempotency-Key: req-2025-01-02-uuid

{
  "context": {
    "env": "staging",
    "priority": "high"
  },
  "triggerReason": "manual-retry"
}
```

### Reschedule with Backoff
```json
PUT /api/v1/tasks/task-456/reschedule

{
  "strategy": "exponential_backoff",
  "baseDelay": "1m",
  "maxDelay": "1h",
  "currentRetryCount": 2
}
```

### Query Event History
```
GET /api/v1/tasks/task-789/events?types=TaskTriggered,TaskRescheduled&from=2025-01-01&to=2025-01-31
```

### Bulk Trigger
```json
POST /api/v1/tasks/bulk/trigger

{
  "tasks": [
    { "taskId": "task-1", "context": { "priority": "high" } },
    { "taskId": "task-2", "conditions": { "dependenciesComplete": ["task-1"] } },
    { "taskId": "task-3" }
  ],
  "allOrNothing": false
}
```

## Event Example

```json
{
  "eventId": "evt-001",
  "eventType": "TaskTriggered",
  "taskId": "task-123",
  "timestamp": "2025-01-02T10:00:00Z",
  "actor": "user-123",
  "previousState": "Scheduled",
  "newState": "Running",
  "metadata": {
    "triggerReason": "manual-retry",
    "idempotencyKey": "req-2025-01-02-uuid"
  }
}
```

## Common Scenarios

### Scenario 1: Retry Failed Task
```bash
# Check event history
GET /api/v1/tasks/task-123/events?types=TaskFailed

# Retry immediately
POST /api/v1/tasks/task-123/trigger

# Or retry with delay
PUT /api/v1/tasks/task-123/reschedule
  { "strategy": "delayed", "delay": "5m" }
```

### Scenario 2: Exponential Backoff Retry
```bash
# First attempt fails, create backoff
PUT /api/v1/tasks/task-456/reschedule
  { 
    "strategy": "exponential_backoff",
    "baseDelay": "1m",
    "maxDelay": "1h",
    "currentRetryCount": 0
  }
# Schedule: now + 1m

# Query event history to see retry count
GET /api/v1/tasks/task-456/events
```

### Scenario 3: Workflow Coordination
```bash
# Task A completes, trigger B and C conditionally
POST /api/v1/tasks/bulk/trigger
  {
    "tasks": [
      {
        "taskId": "task-B",
        "conditions": { "dependenciesComplete": ["task-A"] }
      },
      {
        "taskId": "task-C",
        "conditions": { "dependenciesComplete": ["task-A"] }
      }
    ]
  }
```

## Debugging Tips

### Find Failed Tasks
```bash
GET /api/v1/tasks/task-123/events?types=TaskFailed
```

### Trace Task Lifecycle
```bash
GET /api/v1/tasks/task-123/events
# Shows: Created → Scheduled → Triggered → Running → Failed → Rescheduled
```

### Check Dependency Status
```bash
POST /api/v1/tasks/task-123/trigger
# Response shows condition evaluation details if conditional triggering failed
```

### Monitor Event Rate
```bash
GET /api/v1/tasks/task-123/events?from=<1-hour-ago>&to=<now>
# Count events to see activity rate
```

## Integration Points

- **Event Bus**: MediatR for domain event publishing
- **Notifications**: Event handlers trigger alerts
- **Monitoring**: Structured logging with correlation IDs
- **Multi-Tenancy**: Tenant context in all operations
- **Authorization**: RBAC enforcement on all APIs

## Error Responses

| Status | Meaning | Example |
|--------|---------|---------|
| 202 | Trigger accepted | Task queued for execution |
| 400 | Bad request | Invalid rescheduling strategy |
| 403 | Forbidden | Missing `task:trigger` permission |
| 404 | Not found | Task doesn't exist |
| 409 | Conflict | Task already running |
| 429 | Rate limited | User exceeded rate limit |
| 500 | Server error | Event storage failure |

## Related Documentation

- 📖 [Full PRD](./prd.md)
- 🏛️ [Event-Driven Architecture](./../../../.github/instructions/event-driven-architecture.md)
- 🎯 [CQRS & MediatR](./../../../.github/instructions/cqrs-mediatr.md)
- 🧩 [DDD Aggregates](./../../../.github/instructions/ddd-aggregates.md)
- 🔐 [Input Validation](./../../../.github/instructions/input-validation.md)

---

**Last Updated**: January 2, 2025
**Feature Status**: Ready for Implementation (Issue #58)
