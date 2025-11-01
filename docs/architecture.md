# Architecture

This document describes the AVRO platform architecture.

## System Overview

AVRO is an enterprise-scale AI-driven development lifecycle platform built as a monorepo supporting 100+ interconnected projects and 5K+ structured tasks.

## Architecture Diagram

```
┌─────────────────────────────────────────────────────────────┐
│                      AVRO Platform                          │
├─────────────────────────────────────────────────────────────┤
│                                                             │
│  ┌──────────────┐  ┌──────────────┐  ┌──────────────┐     │
│  │   Frontend   │  │   Backend    │  │  AI Agents   │     │
│  │ TypeScript/  │  │   .NET 8     │  │  Workflows   │     │
│  │   React      │  │   C#         │  │  Context     │     │
│  └──────┬───────┘  └──────┬───────┘  └──────┬───────┘     │
│         │                 │                  │              │
│         └────────┬────────┴──────────────────┘              │
│                  │                                          │
│         ┌────────▼─────────┐                                │
│         │  Core Platform   │                                │
│         │  - Task Registry │                                │
│         │  - Projects      │                                │
│         │  - Dependencies  │                                │
│         └──────────────────┘                                │
│                                                             │
└─────────────────────────────────────────────────────────────┘
```

## Core Components

### 1. Backend (.NET 8)

**Platform.Core**
- Core platform services
- Configuration management
- Health checks
- Shared infrastructure

**TaskRegistry**
- Centralized task management
- 5K+ task tracking
- Metadata and dependencies
- Query and filtering

**Dependencies**
- Project dependency tracking
- Dependency graph management
- Impact analysis
- Circular dependency detection

**AI.Workflows**
- Workflow engine
- Issue-to-PR automation
- Stage execution
- Approval management

**Clean Architecture Layers:**
```
Presentation (API)
    ↓
Application (Use Cases)
    ↓
Domain (Business Logic)
    ↓
Infrastructure (Data Access)
```

### 2. Frontend (TypeScript/React)

**Dashboard App (Next.js)**
- Platform monitoring
- Task management UI
- Project overview
- Workflow status

**UI Component Library (shadcn)**
- Reusable components
- Consistent styling
- Accessible design
- Tailwind CSS

**Architecture Pattern:**
- Component-based
- React hooks for state
- TanStack Query for data
- Next.js App Router

### 3. AI System

**Context Optimization**
- Navigation hints
- Architectural patterns
- Dependency graphs
- Autonomy rules

**Workflows**
- Issue-to-PR automation
- Code review
- Task breakdown
- Dependency updates

**Agent Types:**
- Autonomous: No review needed
- Supervised: Review required
- Manual: Human implementation

## Data Models

### Task
```typescript
{
  id: string              // TASK-XXXX
  title: string
  description: string
  category: Category      // backend, frontend, etc.
  priority: Priority      // low, medium, high, critical
  status: Status          // pending, in-progress, etc.
  assignedProject: string
  dependencies: string[]
  aiMetadata: {
    complexity: string
    estimatedEffort: string
    requiresHumanReview: boolean
    contextFiles: string[]
  }
}
```

### Project
```typescript
{
  id: string
  name: string
  type: string           // backend, frontend, shared
  language: string
  framework: string
  path: string
  dependencies: string[]
  dependents: string[]
  buildConfig: {
    buildCommand: string
    testCommand: string
  }
  aiContext: {
    primaryPurpose: string
    keyFiles: string[]
    autonomyLevel: string
    criticalPaths: string[]
  }
}
```

## Technology Stack

### Backend
- **Runtime**: .NET 8
- **Language**: C# 12
- **Architecture**: Clean Architecture + CQRS
- **DI**: Microsoft.Extensions.DependencyInjection
- **Logging**: Microsoft.Extensions.Logging
- **Testing**: xUnit + FluentAssertions

### Frontend
- **Framework**: Next.js 14 (App Router)
- **Language**: TypeScript 5
- **UI Library**: shadcn/ui
- **Styling**: Tailwind CSS
- **State**: React Context + hooks
- **Data Fetching**: TanStack Query
- **Testing**: Jest + React Testing Library

### Infrastructure
- **Monorepo**: Turborepo (frontend)
- **Package Manager**: pnpm
- **Build**: .NET SDK, Next.js
- **Version Control**: Git
- **CI/CD**: GitHub Actions

## Design Principles

### 1. Standardization
- Consistent patterns across projects
- Shared conventions and tooling
- Predictable structure

### 2. Modularity
- Loosely coupled components
- Clear interfaces
- Single responsibility

### 3. AI-First
- Context-optimized structure
- Autonomy levels
- Workflow automation

### 4. Scalability
- Supports 100+ projects
- 5K+ task capacity
- Efficient dependency resolution

### 5. Quality
- Comprehensive testing
- Security scanning
- Code review (human + AI)

## Dependency Management

### Backend Dependencies
Projects reference each other via ProjectReference:
```xml
<ProjectReference Include="../Platform.Core/Platform.Core.csproj" />
```

### Frontend Dependencies
Workspace packages via package.json:
```json
{
  "dependencies": {
    "@avro/ui": "workspace:*"
  }
}
```

### Cross-Project Dependencies
Tracked in `ai/context/dependency-graph.json`:
- Direct dependencies
- Transitive dependencies
- Impact analysis
- Critical nodes

## Security

### Authentication & Authorization
- API authentication
- Role-based access
- Secure secrets management

### Code Security
- Automated vulnerability scanning
- Dependency audits
- Security-focused code review

### Data Security
- Encrypted at rest
- Encrypted in transit
- Audit logging

## Performance

### Backend
- Async/await patterns
- Efficient database queries
- Caching where appropriate

### Frontend
- Code splitting
- Lazy loading
- Image optimization
- Static generation

## Monitoring & Observability

### Metrics
- Task completion rates
- Workflow execution times
- Test coverage
- AI agent success rates

### Logging
- Structured logging
- Log levels
- Centralized collection

### Alerts
- Build failures
- Test failures
- Security issues
- Performance degradation

## Deployment

### Environments
- Development
- Staging
- Production

### CI/CD Pipeline
1. Code commit
2. Automated tests
3. Security scan
4. Build artifacts
5. Deploy to environment
6. Health checks

## Future Architecture

### Planned Enhancements
- Microservices for scalability
- Event-driven architecture
- GraphQL API
- Real-time collaboration
- Advanced AI capabilities
- Multi-tenant support

## References

- [Getting Started](getting-started.md)
- [AI Agents Guide](ai-agents.md)
- [Contributing](contributing.md)
- [API Reference](api-reference.md)
