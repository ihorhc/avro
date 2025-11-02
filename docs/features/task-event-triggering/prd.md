# Product Requirements Document: Enhanced Task Event Triggering & Manual Rescheduling

## 1. Feature Name

**Enhanced Task Event Triggering & Manual Rescheduling System**

---

## 2. Epic

**Parent Epic**: [Avro Platform - AI-First Monorepo SDLC Platform](https://github.com/ihorhc/avro/issues/1)

**Related Milestones**:
- [Milestone 2: Task Management Enhancements](https://github.com/ihorhc/avro/milestone/2)
- Part of TaskRegistry service expansion

**Related Documentation**:
- [Multi-Agent Orchestration Framework](./../../../COORDINATION_WORKFLOW.md)
- [Architecture Collection](./../../../.github/instructions/architecture-collection.md)
- [Event-Driven Architecture](./../../../.github/instructions/event-driven-architecture.md)

---

## 3. Goal

### Problem

Currently, the Avro platform's task management system has limitations in handling task lifecycle events:

1. **Manual triggering is cumbersome**: Tasks cannot easily be triggered manually by users, requiring backend intervention
2. **Rescheduling limitations**: Tasks can only be rescheduled through database modifications or administrative intervention, not through user-friendly APIs
3. **Event visibility gaps**: Task event history and transitions are not easily accessible or auditable
4. **Workflow inflexibility**: Complex workflows requiring task retries, delays, or conditional execution lack proper support
5. **AI agent coordination**: The autonomous development workflow needs reliable task event management for agent coordination and status tracking

### Solution

Implement a comprehensive **Task Event Triggering & Rescheduling System** that:

- Provides explicit APIs for manually triggering task execution
- Enables flexible task rescheduling with multiple scheduling strategies (immediate, delayed, conditional)
- Maintains complete event audit trail for compliance and debugging
- Supports event filtering, querying, and conditional execution
- Integrates with the existing Domain-Driven Design and event-driven architecture
- Enables AI agents to orchestrate complex workflows with reliable task coordination

### Impact

**Expected Outcomes**:

- **Developer Experience**: 40% reduction in time to reschedule/retry failed tasks
- **Reliability**: 99.5% success rate for critical AI agent coordination tasks
- **Auditability**: 100% event trail coverage with searchable event history
- **Throughput**: Support for 5,000+ concurrent task scheduling operations
- **Flexibility**: Enable complex multi-step workflows with conditional branching

**Key Metrics**:
- Task retry success rate: >95%
- Manual trigger latency: <500ms
- Event storage efficiency: >90% indexing coverage
- API availability: 99.9% uptime

---

## 4. User Personas

### 4.1 DevOps Engineer
- **Goal**: Monitor and reschedule failed deployment tasks
- **Pain Point**: Currently must use database tools to retry failed deployments
- **Value**: Quick rescheduling UI reduces MTTR and operational overhead

### 4.2 AI Agent Orchestrator
- **Goal**: Coordinate multi-step feature implementation workflows
- **Pain Point**: Workflow dependency management and conditional execution not supported
- **Value**: Reliable task event system enables autonomous agent coordination

### 4.3 Platform Administrator
- **Goal**: Audit task execution history and troubleshoot workflow issues
- **Pain Point**: Limited visibility into why tasks were rescheduled or triggered
- **Value**: Complete event audit trail enables rapid issue diagnosis

### 4.4 Feature Development Team
- **Goal**: Debug complex feature workflows with multiple interdependent tasks
- **Pain Point**: Limited visibility into task state transitions and event history
- **Value**: Event querying and filtering enables efficient debugging

---

## 5. User Stories

### 5.1 Primary User Stories

#### US-01: Manual Task Triggering
**As a** DevOps engineer,
**I want to** manually trigger a task execution immediately,
**so that** I can retry failed tasks without waiting for scheduled execution.

#### US-02: Task Rescheduling with Delay
**As a** DevOps engineer,
**I want to** reschedule a failed task with a specific delay (e.g., 5 minutes, 1 hour),
**so that** I can handle transient failures with exponential backoff.

#### US-03: Conditional Task Triggering
**As an** AI orchestrator,
**I want to** trigger a task only if its dependencies are complete,
**so that** I can build complex multi-step workflows reliably.

#### US-04: Event History Querying
**As a** platform administrator,
**I want to** query task event history with filters (status, timestamp, trigger reason),
**so that** I can audit task execution and troubleshoot issues.

#### US-05: Event-Driven Notifications
**As a** DevOps engineer,
**I want to** receive notifications when tasks are manually triggered or rescheduled,
**so that** I can track changes to critical tasks in real-time.

#### US-06: Scheduled Task Rescheduling
**As a** platform administrator,
**I want to** reschedule already-scheduled tasks before they execute,
**so that** I can adjust execution timing based on system load.

#### US-07: Bulk Task Triggering
**As an** AI orchestrator,
**I want to** trigger multiple related tasks as a batch with transactional guarantees,
**so that** I can coordinate multi-phase workflows reliably.

### 5.2 Edge Cases & Error Scenarios

#### US-08: Prevent Duplicate Triggering
**As a** task execution engine,
**I want to** prevent duplicate triggers within a time window,
**so that** I can avoid executing the same task multiple times due to network retries.

#### US-09: Handle Missing Dependencies
**As a** task scheduler,
**I want to** fail fast when triggering a task with unmet dependencies,
**so that** I can provide immediate feedback instead of queuing invalid tasks.

#### US-10: Reschedule Completed Tasks
**As a** DevOps engineer,
**I want to** reschedule a completed task with a new execution context,
**so that** I can rerun tasks with updated parameters or different environments.

---

## 6. Requirements

### 6.1 Functional Requirements

#### 6.1.1 Manual Task Triggering

- **FR-01**: Provide POST `/api/v1/tasks/{taskId}/trigger` endpoint to manually trigger a task
  - Accept optional `context` parameter for execution context override
  - Return task execution confirmation with estimated completion time
  - Validate task state (e.g., prevent triggering running tasks)
  - Support trigger reason metadata (e.g., "manual-retry", "dependency-completion")

- **FR-02**: Implement idempotent trigger operations
  - Support idempotency key to prevent duplicate executions
  - Return same result if called twice with same idempotency key
  - Enforce reasonable key retention period (e.g., 24 hours)

- **FR-03**: Validate trigger preconditions
  - Verify task exists and is in valid state (Draft, Scheduled, Failed)
  - Check authorization: only task owner or admins can trigger
  - Validate task doesn't have conflicting running executions
  - Check system resources before accepting trigger

#### 6.1.2 Task Rescheduling

- **FR-04**: Provide PUT `/api/v1/tasks/{taskId}/reschedule` endpoint for task rescheduling
  - Support scheduling strategies:
    - `immediate`: Trigger as soon as possible
    - `delayed`: Schedule after specified delay (e.g., `{ delay: "5m", unit: "minutes" }`)
    - `cron`: Cron expression for recurring schedules
    - `conditional`: Trigger only when conditions are met
  - Return updated task schedule with next execution timestamp
  - Support partial rescheduling (reschedule without modifying other properties)

- **FR-05**: Handle rescheduling state transitions
  - Allow rescheduling of: Scheduled, Failed, Completed tasks
  - Prevent rescheduling of Running tasks (unless `force: true`)
  - Clear previous schedule before applying new one
  - Preserve task execution history

- **FR-06**: Support exponential backoff rescheduling
  - Calculate backoff multiplier based on retry count
  - Default formula: `delay = baseDelay * (2 ^ retryCount)`
  - Configurable base delay and max delay cap (e.g., 24 hours)
  - Include jitter to prevent thundering herd

#### 6.1.3 Task Event Management

- **FR-07**: Capture task lifecycle events
  - Event types: `TaskCreated`, `TaskTriggered`, `TaskRescheduled`, `TaskStarted`, `TaskCompleted`, `TaskFailed`, `TaskCancelled`
  - Each event includes:
    - Timestamp (UTC)
    - Event type and subtype (e.g., trigger reason)
    - Actor (user ID or system)
    - Previous state and new state
    - Metadata (context, parameters)
  - Immutable event log (append-only)

- **FR-08**: Publish task events through event bus
  - Emit domain events through MediatR notification system
  - Async event handlers for notifications and webhooks
  - Event schema versioning for backward compatibility
  - Dead letter queue for failed event handlers

- **FR-09**: Provide event history query API
  - GET `/api/v1/tasks/{taskId}/events` with filtering:
    - Event type filter (e.g., `?types=TaskTriggered,TaskRescheduled`)
    - Date range filter (e.g., `?from=2025-01-01&to=2025-01-31`)
    - Actor filter (e.g., `?actor=user123`)
    - Pagination support (cursor-based for large result sets)
  - Return events in chronological order with audit trail
  - Support exporting event history as CSV or JSON

#### 6.1.4 Dependency & Conditional Triggering

- **FR-10**: Support conditional task triggering
  - Evaluate conditions before triggering:
    - All dependencies completed successfully: `dependenciesComplete: true`
    - At least N dependencies completed: `dependenciesCompleted: { min: 2 }`
    - Specific dependencies met: `dependenciesCompleted: ["task-123", "task-456"]`
  - Fail fast if conditions unmet
  - Return condition evaluation details in response

- **FR-11**: Implement dependency resolution
  - Build task dependency graph at trigger time
  - Detect circular dependencies and prevent triggering
  - Support transitive dependency checking
  - Return dependency status summary with trigger attempt

#### 6.1.5 Bulk Operations

- **FR-12**: Provide bulk task triggering
  - POST `/api/v1/tasks/bulk/trigger` to trigger multiple tasks
  - Accept array of `{ taskId, context?, conditions? }`
  - Support `allOrNothing` transactional mode for consistency
  - Return per-task execution result with success/failure reason
  - Enforce rate limits on bulk operations

- **FR-13**: Provide bulk rescheduling
  - PUT `/api/v1/tasks/bulk/reschedule` to reschedule multiple tasks
  - Apply same schedule to all tasks or per-task schedules
  - Return per-task result with new scheduled time
  - Support filtering criteria for bulk operations

### 6.2 Non-Functional Requirements

#### 6.2.1 Performance

- **NFR-01**: Event storage and retrieval
  - Query event history: <100ms for 1000 events
  - Event creation: <50ms latency
  - Support indexing on timestamp, event type, actor for query optimization
  - Archive old events (>90 days) to cold storage

- **NFR-02**: API response times
  - Manual trigger endpoint: <500ms P95 latency
  - Rescheduling endpoint: <300ms P95 latency
  - Event history query: <1s P95 latency for 10,000 events
  - Bulk operations: <2s P95 for 100 task batch

- **NFR-03**: Throughput and scalability
  - Support 5,000+ concurrent task scheduling operations
  - Handle 10,000+ events per minute logging rate
  - Support task queue with at least 1,000,000 pending tasks

#### 6.2.2 Reliability & Fault Tolerance

- **NFR-04**: Event durability
  - All events persisted to durable storage before acknowledgment
  - Replication factor of 2+ for critical events
  - Recovery time objective (RTO): <5 minutes
  - Recovery point objective (RPO): <1 minute

- **NFR-05**: Idempotency & deduplication
  - Idempotent trigger operations with 24-hour key retention
  - Deduplication within 5-minute time window
  - Exactly-once delivery semantics for critical events

- **NFR-06**: Deadline & timeout handling
  - Configure execution deadlines per task
  - Auto-fail tasks exceeding deadline
  - Graceful timeout handling with context cancellation

#### 6.2.3 Security

- **NFR-07**: Authorization & access control
  - Role-based access control (RBAC) for trigger/reschedule operations
  - Audit logging of all manual trigger operations
  - User identity captured for all events
  - Encrypted event storage for sensitive tasks

- **NFR-08**: Rate limiting
  - Per-user rate limit: 1000 triggers/hour
  - Per-task rate limit: 100 triggers/hour
  - Per-system rate limit: 10,000 triggers/minute
  - Graceful degradation under high load

- **NFR-09**: Data privacy
  - GDPR compliance for user identity in events
  - Support task event deletion upon request (compliance)
  - Configurable event retention policies per environment

#### 6.2.4 Observability & Monitoring

- **NFR-10**: Logging & tracing
  - Structured logging for all trigger/reschedule operations
  - Distributed tracing with correlation IDs
  - Track event processing latency and throughput
  - Error rate monitoring and alerting

- **NFR-11**: Metrics & dashboards
  - Track task trigger success/failure rates per hour
  - Monitor event processing latency percentiles (P50, P95, P99)
  - Queue depth and throughput metrics
  - Dependency resolution failure rate

- **NFR-12**: Health checks
  - Event bus health status
  - Storage layer availability
  - Queue depth warnings when >80% capacity

#### 6.2.5 Multi-Tenancy

- **NFR-13**: Tenant isolation
  - All events scoped to tenant context
  - Query filters automatically apply tenant context
  - No cross-tenant event visibility
  - Per-tenant rate limit enforcement

---

## 7. Acceptance Criteria

### 7.1 Manual Task Triggering

#### AC-01: Successful Manual Trigger
**Given** a task in "Scheduled" state,
**When** a user with sufficient permissions calls POST `/api/v1/tasks/{taskId}/trigger`,
**Then** the task transitions to "Running", an event is created, and the HTTP response contains `{ status: 202, executionId: "...", estimatedCompletion: "..." }`

#### AC-02: Idempotent Triggering
**Given** a task trigger with idempotency key "key-123",
**When** the same endpoint is called twice with identical parameters,
**Then** both calls return identical results and the task is triggered only once

#### AC-03: Prevent Invalid State Transitions
**Given** a task already in "Running" state,
**When** a user attempts to trigger it again,
**Then** the endpoint returns HTTP 409 with error message "Task is already running"

#### AC-04: Authorization Check
**Given** a user without "task:trigger" permission,
**When** attempting to trigger a task,
**Then** the endpoint returns HTTP 403 Forbidden

#### AC-05: Event Capture
**Given** a successful task trigger,
**When** the trigger completes,
**Then** an event of type "TaskTriggered" is created containing `{ timestamp, taskId, actor, triggerReason, previousState, newState }`

### 7.2 Task Rescheduling

#### AC-06: Reschedule with Delay
**Given** a task in "Failed" state,
**When** PUT `/api/v1/tasks/{taskId}/reschedule` is called with `{ strategy: "delayed", delay: "5m" }`,
**Then** the task transitions to "Scheduled" with `nextExecution = now + 5 minutes`

#### AC-07: Exponential Backoff
**Given** a task with `retryCount: 2` and `baseDelay: "1m"`,
**When** rescheduled with exponential backoff strategy,
**Then** `nextExecution = now + (1 minute * 2^2) = now + 4 minutes`

#### AC-08: Conditional Rescheduling
**Given** a task with dependencies ["task-1", "task-2"],
**When** rescheduled with conditions `{ dependenciesComplete: ["task-1"] }`,
**Then** the task is rescheduled only if "task-1" is in "Completed" state

#### AC-09: Preserve Execution History
**Given** a task with previous execution history,
**When** the task is rescheduled,
**Then** previous execution records remain unchanged and new executions append to history

#### AC-10: Event Capture
**Given** a successful task rescheduling,
**When** the reschedule completes,
**Then** an event of type "TaskRescheduled" is created with new schedule details

### 7.3 Task Event History

#### AC-11: Event Query Success
**Given** task "task-123" with 50+ events,
**When** GET `/api/v1/tasks/task-123/events` is called,
**Then** response contains all events in chronological order with `{ events: [...], total: 50, pageInfo: {...} }`

#### AC-12: Event Filtering
**Given** 20 events of mixed types (TaskTriggered, TaskRescheduled, TaskCompleted),
**When** queried with `?types=TaskTriggered,TaskRescheduled`,
**Then** only 15 events of those types are returned

#### AC-13: Date Range Filtering
**Given** events spanning 2 days,
**When** queried with `?from=2025-01-01&to=2025-01-01`,
**Then** only events from January 1st are returned

#### AC-14: Event Immutability
**Given** a persisted event,
**When** attempted to be modified or deleted,
**Then** the operation fails with HTTP 405 Method Not Allowed

#### AC-15: Audit Trail Completeness
**Given** any task state change (trigger, reschedule, completion),
**When** event history is queried,
**Then** corresponding event exists with accurate timestamp and metadata

### 7.4 Dependency & Conditional Triggering

#### AC-16: Circular Dependency Detection
**Given** tasks with circular dependencies: A → B → C → A,
**When** attempting to trigger task A with condition-based triggering,
**Then** the operation fails with HTTP 400 and error "Circular dependency detected"

#### AC-17: Dependency Validation
**Given** a task with required dependencies ["task-1", "task-2"],
**When** attempting conditional trigger without all dependencies completed,
**Then** the operation fails with details showing which dependencies are not met

#### AC-18: Transitive Dependencies
**Given** task chain: A depends on B, B depends on C,
**When** task C completes,
**Then** A is considered dependency-complete (transitive check succeeds)

### 7.5 Bulk Operations

#### AC-19: Bulk Trigger Success
**Given** a batch request with 10 tasks,
**When** POST `/api/v1/tasks/bulk/trigger` is called,
**Then** response contains per-task results showing 8 successes and 2 failures with reasons

#### AC-20: Bulk Transactional Mode
**Given** bulk request with `allOrNothing: true` containing 3 tasks,
**When** one task fails precondition check,
**Then** no tasks in the batch are triggered and HTTP response indicates transaction failure

#### AC-21: Bulk Rate Limiting
**Given** per-system limit of 10,000 triggers/minute,
**When** attempting 100 concurrent bulk requests (10,000 tasks total),
**Then** some requests are rate-limited with HTTP 429

### 7.6 Integration with Event-Driven Architecture

#### AC-22: Domain Event Publishing
**Given** a task trigger occurs,
**When** the trigger is persisted,
**Then** MediatR publishes `TaskTriggeredEvent` async for all subscribers

#### AC-23: Event Handler Resilience
**Given** one event handler fails,
**When** other handlers process the same event,
**Then** failed handler is logged, event is sent to dead letter queue, and other handlers complete successfully

#### AC-24: Notification Integration
**Given** a critical task is rescheduled,
**When** `TaskRescheduledEvent` is published,
**Then** notification handler sends alert to DevOps team via configured channel (Slack, email, etc.)

### 7.7 AI Agent Coordination

#### AC-25: Workflow Coordination
**Given** AI agent workflow with 5 sequential tasks,
**When** each task completes and triggers the next,
**Then** all tasks execute in correct order with no manual intervention

#### AC-26: Conditional Branching
**Given** AI workflow with branching logic,
**When** task completion triggers conditional task selection,
**Then** correct branch is taken based on completion criteria

#### AC-27: Workflow Rollback
**Given** AI workflow failure at step 3,
**When** operator reschedules from step 1 with `force: true`,
**Then** workflow restarts cleanly without residual state

---

## 8. Out of Scope

### 8.1 Not Included in This Feature

- **UI/Dashboard Implementation**: Task trigger/reschedule UI components (separate feature)
- **Workflow Engine**: General-purpose workflow orchestration engine (future enhancement)
- **Task Priority Changes**: Changing task priority post-creation (future feature)
- **Task Cloning**: Creating new tasks from existing task templates (future feature)
- **Advanced Scheduling**: Complex cron expressions with holiday calendars (v2.0)
- **Machine Learning**: Predictive rescheduling based on historical patterns (future)
- **Third-Party Integration**: Webhook notifications to external systems (v1.1)
- **Task Migration**: Moving tasks between services or tenants (v2.0)
- **Performance Optimization**: Query optimization for >1M events (v1.1)

### 8.2 Explicitly Out of Scope

- Manual task cancellation (separate API endpoint planned)
- Task priority queue implementation (use standard queue for v1.0)
- Dead letter queue processing UI (operational/backend only)
- Kafka/RabbitMQ integration (event bus agnostic, DB-backed for v1.0)
- GraphQL API (REST-only for v1.0)
- Real-time WebSocket updates (polling-based for v1.0)

---

## 9. Technical Implementation Context

### 9.1 Architecture Alignment

This feature aligns with Avro platform standards:

- **Clean Architecture**: Separate Domain, Application, Infrastructure layers
- **CQRS Pattern**: Distinct trigger (command) and event query (query) models
- **Event-Driven**: Domain events drive async event handlers and notifications
- **Domain-Driven Design**: Task aggregate with rich business logic for state transitions
- **Multi-Tenancy**: Tenant context propagated through all layers

### 9.2 Technology Stack

- **Backend**: .NET 10 with async/await throughout
- **Database**: Entity Framework Core with PostgreSQL/Aurora
- **Event Bus**: MediatR for domain events (future: Kafka)
- **API**: ASP.NET Core Minimal APIs with OpenAPI
- **Testing**: xUnit, Moq, FluentAssertions with >80% coverage

### 9.3 Related Documentation

- [CQRS & MediatR](./../../../.github/instructions/cqrs-mediatr.md)
- [Event-Driven Architecture](./../../../.github/instructions/event-driven-architecture.md)
- [DDD Aggregates & Domain Events](./../../../.github/instructions/ddd-aggregates.md)
- [Repository Pattern](./../../../.github/instructions/repository-pattern.md)
- [Input Validation](./../../../.github/instructions/input-validation.md)

---

## 10. Success Metrics & KPIs

### 10.1 Functional Metrics

| Metric | Target | Success Criteria |
|--------|--------|------------------|
| Task trigger success rate | >98% | 20 successful triggers out of 20 attempts |
| Manual reschedule completion | 100% | All rescheduled tasks execute as scheduled |
| Event capture completeness | 100% | Every task state change has corresponding event |
| API availability | 99.9% | <8.6 hours downtime per month |

### 10.2 Performance Metrics

| Metric | Target | Success Criteria |
|--------|--------|------------------|
| Trigger latency (P95) | <500ms | 95% of triggers complete within 500ms |
| Reschedule latency (P95) | <300ms | 95% of reschedules complete within 300ms |
| Event query latency (P95) | <1s | 95% of 10K event queries under 1 second |
| Bulk operation throughput | 5000+ tasks/min | Process 5000+ concurrent task schedules |

### 10.3 User Experience Metrics

| Metric | Target | Success Criteria |
|--------|--------|------------------|
| Task retry time reduction | 40% | Retry time drops from avg 15min to <9min |
| Operator time per incident | -30% | Reduce incident resolution time by 30% |
| Event audit trail adoption | >80% | >80% of teams use event history for debugging |
| AI workflow reliability | 99.5% | Autonomous workflows succeed 99.5% of time |

---

## 11. Rollout & Launch Plan

### 11.1 Phase 1: MVP (Weeks 1-2)
- Manual task triggering API
- Basic rescheduling (immediate only)
- Event capture and storage
- Unit tests and integration tests

### 11.2 Phase 2: Enhanced Scheduling (Week 3)
- Delayed rescheduling with exponential backoff
- Conditional task triggering
- Event query API with filtering
- Performance optimization

### 11.3 Phase 3: Operational Excellence (Week 4)
- Bulk operations (trigger and reschedule)
- Event-driven notifications
- Monitoring and alerting
- Documentation and runbooks

### 11.4 Phase 4: Production Hardening (Week 5)
- Load testing and performance tuning
- Security audit and penetration testing
- AI agent workflow integration testing
- Production deployment and monitoring

---

## 12. Dependencies & Risk Mitigation

### 12.1 Internal Dependencies
- TaskRegistry service core models and repository
- Event bus infrastructure (MediatR setup)
- Notification service for event-driven alerts
- Authorization/authentication service for RBAC

### 12.2 External Dependencies
- Database availability (PostgreSQL/Aurora)
- Event publishing infrastructure
- Notification delivery services

### 12.3 Risks & Mitigation

| Risk | Probability | Impact | Mitigation |
|------|-------------|--------|-----------|
| Event storage scale limits | Medium | High | Implement event archival strategy; test with 10M+ events |
| Event handler failures | Medium | Medium | Implement dead letter queue; async error handling |
| Circular dependency detection performance | Low | Medium | Use graph algorithm optimization; cache dependency graph |
| Rate limiting bypass | Low | High | Implement token bucket with per-user/per-system limits |
| Distributed transaction inconsistency | Medium | High | Use compensating transactions; implement idempotency keys |

---

## 13. Appendix: API Contract Examples

### Example 1: Manual Trigger Request

```json
{
  "POST /api/v1/tasks/task-123/trigger": {
    "headers": {
      "X-Idempotency-Key": "req-2025-01-02-uuid"
    },
    "body": {
      "context": {
        "env": "staging",
        "priority": "high"
      },
      "triggerReason": "manual-retry"
    }
  }
}
```

**Response (202 Accepted)**:
```json
{
  "status": 202,
  "data": {
    "executionId": "exec-abc123",
    "taskId": "task-123",
    "estimatedCompletion": "2025-01-02T10:30:00Z",
    "message": "Task trigger accepted and queued for execution"
  }
}
```

### Example 2: Reschedule with Exponential Backoff

```json
{
  "PUT /api/v1/tasks/task-456/reschedule": {
    "body": {
      "strategy": "exponential_backoff",
      "baseDelay": "1m",
      "maxDelay": "1h",
      "currentRetryCount": 2
    }
  }
}
```

**Response (200 OK)**:
```json
{
  "status": 200,
  "data": {
    "taskId": "task-456",
    "previousSchedule": "2025-01-02T10:00:00Z",
    "nextExecution": "2025-01-02T10:04:00Z",
    "delayApplied": "4m",
    "calculationDetails": {
      "formula": "baseDelay * (2 ^ retryCount)",
      "baseDelay": "1m",
      "retryCount": 2,
      "result": "4m"
    }
  }
}
```

### Example 3: Event History Query

```json
{
  "GET /api/v1/tasks/task-789/events": {
    "query": {
      "types": "TaskTriggered,TaskRescheduled",
      "from": "2025-01-01T00:00:00Z",
      "to": "2025-01-02T23:59:59Z",
      "pageSize": 50,
      "cursor": ""
    }
  }
}
```

**Response (200 OK)**:
```json
{
  "status": 200,
  "data": {
    "events": [
      {
        "eventId": "evt-001",
        "eventType": "TaskTriggered",
        "taskId": "task-789",
        "timestamp": "2025-01-02T10:00:00Z",
        "actor": "user-123",
        "previousState": "Scheduled",
        "newState": "Running",
        "metadata": {
          "triggerReason": "manual-retry",
          "idempotencyKey": "req-2025-01-02-uuid"
        }
      },
      {
        "eventId": "evt-002",
        "eventType": "TaskRescheduled",
        "taskId": "task-789",
        "timestamp": "2025-01-02T10:05:00Z",
        "actor": "user-456",
        "previousState": "Failed",
        "newState": "Scheduled",
        "metadata": {
          "strategy": "exponential_backoff",
          "newSchedule": "2025-01-02T10:10:00Z"
        }
      }
    ],
    "total": 2,
    "pageInfo": {
      "hasMore": false,
      "nextCursor": null
    }
  }
}
```

---

## Document History

| Version | Date | Author | Changes |
|---------|------|--------|---------|
| 1.0 | 2025-01-02 | Product Team | Initial PRD creation |

---

**Created**: January 2, 2025
**Last Updated**: January 2, 2025
**Status**: Ready for Architecture Review
