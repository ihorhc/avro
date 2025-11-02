# Automated Merge Conflict Resolution - Design Document

**Issue**: [#73](https://github.com/ihorhc/avro/issues/73)
**Status**: Planning
**Last Updated**: November 2, 2025

## Overview

This document outlines the design for an automated merge conflict resolution system using a dedicated AI agent integrated into the Avro platform's CI/CD pipeline.

## Current State

- **Problem**: Merge conflicts block PR workflows and require manual intervention
- **Impact**: Slows development velocity, context-switching, potential for incorrect resolutions
- **Current Process**: Manual resolution by developers

## Proposed Solution

### Architecture Components

```
┌─────────────────────────────────────────────────────────────────┐
│ GitHub PR with Conflicts Created                                │
└──────────────────────┬──────────────────────────────────────────┘
                       │
                       ▼
┌─────────────────────────────────────────────────────────────────┐
│ conflict-detection.yml Workflow Triggered                       │
│ - Detects conflict markers in diff                              │
│ - Analyzes file types and change context                        │
└──────────────────────┬──────────────────────────────────────────┘
                       │
                       ▼
┌─────────────────────────────────────────────────────────────────┐
│ Conflict Resolver Agent Analysis                                │
│ - Reads PR metadata and diff                                    │
│ - Applies resolution strategies                                 │
│ - Calculates confidence score                                   │
└──────────────────────┬──────────────────────────────────────────┘
                       │
         ┌─────────────┴─────────────┐
         │                           │
    Confidence >= 85%          Confidence < 85%
         │                           │
         ▼                           ▼
   Auto-Commit              Post Analysis Comment
   Resolution               + Recommendations
         │                           │
         └─────────────┬─────────────┘
                       │
                       ▼
             PR Updated & Tests Run
```

### Agent Definition

**File**: `.github/agents/conflict-resolver.agent.md`

**Capabilities**:
- Conflict detection and categorization
- Multi-strategy resolution (LWW, Merge-Base, CQRS, AST-aware, Pattern-matching)
- Confidence scoring
- Detailed analysis and recommendations
- Rollback procedures

**Inputs**:
- PR diff and metadata
- Conflict markers and context
- Changed files and file types
- Git blame information
- Component/service boundaries

**Outputs**:
- Resolution strategy applied
- Resolved file contents
- Confidence score (0-100%)
- Analysis comment (if manual review needed)
- Metrics and audit log

### GitHub Actions Workflow

**File**: `.github/workflows/conflict-detection.yml`

**Triggers**:
- `pull_request` event (conflict status)
- `pull_request` label added: `conflict`, `merge-conflict`
- Scheduled check on open PRs (daily)

**Steps**:
1. Check for merge conflict markers
2. Invoke Conflict Resolver Agent
3. Evaluate resolution confidence
4. Auto-commit (if confidence >= 85%) OR post comment (if < 85%)
5. Log metrics and results

## Resolution Strategies

### Strategy 1: Last-Writer-Wins (LWW)
**Use Case**: Documentation, changelog, non-critical configuration
**Logic**: Accept incoming (PR) branch changes
**Confidence**: High (90%+)

### Strategy 2: Merge-Base Preference
**Use Case**: Core logic, main branch priority
**Logic**: Keep main branch for critical sections, merge PR additions
**Confidence**: High (85-95%)

### Strategy 3: CQRS/Domain Segregation
**Use Case**: .NET services with CQRS pattern
**Logic**: Separate by command/query, apply by DDD boundaries
**Confidence**: Very High (90-98%)

### Strategy 4: AST-Aware Resolution
**Use Case**: Source code conflicts (C#, JavaScript, Python)
**Logic**: Parse and understand code structure, resolve at function/class level
**Confidence**: Very High (92-98%)

### Strategy 5: Pattern Matching
**Use Case**: Special file types (changelog, version, lock files)
**Logic**: 
- Changelog/version: Merge both entries with sorting
- Lock files: Regenerate or use base version
- Config: Merge with strategy priorities
**Confidence**: Medium-High (80-90%)

### Strategy 6: Manual Review Flag
**Use Case**: Complex, ambiguous, or unsupported conflicts
**Logic**: Generate analysis, flag for human review
**Confidence**: N/A (deferred to human)

## Resolution Confidence Scoring

```
Confidence Score = Base Score × Type Factor × Context Factor × Complexity Penalty

Where:
- Base Score: 50-100% depending on strategy
- Type Factor: File type multiplier (0.8-1.0)
- Context Factor: Surrounding code/blame analysis (0.8-1.0)
- Complexity Penalty: Number of conflicts, interleaving (0.5-1.0)

Auto-commit threshold: >= 85%
Manual review threshold: 50-85%
Escalation threshold: < 50%
```

## File Type Handling

| File Type | Strategy | Confidence | Notes |
|-----------|----------|-----------|-------|
| `*.cs` | AST-aware + CQRS | 92-98% | Parse C# AST, apply DDD |
| `*.js/.ts` | AST-aware | 88-95% | Parse JS/TS, understand scopes |
| `*.json` | Pattern match + merge | 85-92% | Special handling for package.json, tsconfig.json |
| `*.md` | Pattern match + LWW | 80-90% | Merge documentation sections |
| `CHANGELOG.md` | Pattern match | 90%+ | Merge entries with date sorting |
| `package.json` / `*.lock` | Regenerate | 85%+ | Lock file regeneration |
| Binary files | Manual | N/A | Always escalate to human |

## Integration Points

### 1. GitHub Actions
- Workflow: `.github/workflows/conflict-detection.yml`
- Permissions: `contents: write`, `pull-requests: write`
- Frequency: On PR conflict, daily check on open PRs

### 2. Agent Framework
- Agent: `.github/agents/conflict-resolver.agent.md`
- Dependencies: architect.agent.md (consultation), review.agent.md (validation)
- Invoke: Via GitHub Actions, triggered by workflow

### 3. CI/CD Pipeline
- Pre-merge gate: Conflicts must be resolved (automated or manual)
- Status check: `conflict-resolution/success` or `conflict-resolution/review-needed`
- Metrics: Track resolution rate, time, and accuracy

### 4. Observability
- Logging: All resolutions logged to `/logs/conflict-resolution/`
- Metrics: Prometheus metrics for dashboard
- Audit: Git commit messages include resolution strategy and confidence

## Metrics & Monitoring

### Key Metrics

| Metric | Target | Collection |
|--------|--------|-----------|
| Auto-resolution rate | 80%+ | Conflicts auto-resolved / total |
| Resolution time | <30s | Time from detection to commit |
| Accuracy rate | 98%+ | Successful resolutions / total |
| User satisfaction | 4.0/5.0 | Team survey |
| Time saved | 2+ hrs/week | Developer time previously spent |

### Monitoring Dashboard

Track in Prometheus/Grafana:
- Conflict resolution success rate (by strategy)
- Average resolution time (by file type)
- Confidence distribution (histogram)
- Manual review rate (escalation metrics)
- PR merge time improvement

## Rollback Procedures

### Emergency Rollback (< 5 minutes)

1. **Revert conflicting commit**:
   ```bash
   git revert <conflict-resolution-commit>
   ```

2. **Restore to merge-base**:
   ```bash
   git reset --hard <merge-base>
   ```

3. **Notify team**: Post comment on PR

### Recovery Procedures

1. **Investigate resolution**:
   - Check conflict analysis log
   - Review confidence score reasoning
   - Analyze actual vs. expected

2. **Determine root cause**:
   - Strategy failure
   - Context misunderstanding
   - Unexpected edge case

3. **Improve agent**:
   - Update strategy patterns
   - Adjust confidence thresholds
   - Add new conflict type handling

## Configuration

### Per-Project Settings

**File**: `.github/conflict-resolution.yml`

```yaml
conflict-resolution:
  enabled: true
  auto-commit: true
  confidence-threshold: 85
  
  strategies:
    - cqrs:
        enabled: true
        confidence: 95
    - pattern-match:
        enabled: true
        confidence: 90
    - ast-aware:
        enabled: true
        confidence: 92
  
  file-types:
    "*.cs":
      strategy: cqrs
      threshold: 92
    "*.md":
      strategy: pattern-match
      threshold: 85
    "*.json":
      strategy: pattern-match
      threshold: 88
```

## Implementation Phases

### Phase 1: Foundation (Week 1-2)
- [ ] Create Conflict Resolver Agent definition
- [ ] Implement conflict-detection.yml workflow
- [ ] Support LWW and Merge-Base strategies
- [ ] Basic logging and metrics

### Phase 2: Intelligence (Week 3-4)
- [ ] Add AST-aware resolution for C#
- [ ] Implement pattern matching for configs
- [ ] Add confidence scoring
- [ ] Post detailed analysis comments

### Phase 3: Integration (Week 5-6)
- [ ] Integrate with CI/CD pipelines
- [ ] Add configuration system
- [ ] Dashboard and monitoring
- [ ] Team training and documentation

### Phase 4: Optimization (Week 7+)
- [ ] ML-based pattern learning
- [ ] Feedback loop and improvements
- [ ] Performance optimization
- [ ] Advanced rollback strategies

## Success Criteria

- ✅ 80%+ of conflicts auto-resolved without human intervention
- ✅ Resolution completes in <30 seconds
- ✅ 98%+ accuracy (no failed resolutions causing test failures)
- ✅ 4.0/5.0 team satisfaction score
- ✅ 50% reduction in developer context-switching
- ✅ 2+ hours/week saved in team time

## Next Steps

1. **Review & Approve** - Technical review by architecture team
2. **Create Agent Definition** - Implement `.github/agents/conflict-resolver.agent.md`
3. **Create Workflow** - Implement `.github/workflows/conflict-detection.yml`
4. **Testing** - Test with sample conflict scenarios
5. **Deployment** - Roll out to team with documentation
6. **Monitoring** - Track metrics and iterate

## References

- Issue: [#73 - Automated merge conflict resolution](https://github.com/ihorhc/avro/issues/73)
- Related: Issue #17 (Review Agent), Issue #20 (Testing Agent)
- Epic: Automated AI Development Workflow
- Docs: [Conflict Resolution Decision Tree](./conflict-resolution-decision-tree.md)

---

**Note**: This is a planning document. Implementation will proceed in phases once design is approved.
