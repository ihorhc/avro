# Code Review Scoring Thresholds and Quality Gates

**Version**: 1.0  
**Last Updated**: November 2, 2025  
**Applies To**: Automated Code Review Workflow

---

## Overview

The automated code review workflow calculates a compliance score to
measure code quality and adherence to Avro platform standards. This
document defines the scoring methodology, quality thresholds, and their
impact on merge decisions.

---

## Compliance Score Calculation

### Current Implementation

The workflow currently uses a **static score** of `7.9/10` as a
baseline. This will be enhanced to dynamic scoring based on multiple
factors.

### Planned Dynamic Scoring (Future Enhancement)

```typescript
function calculateComplianceScore(): number {
  let score = 10.0;

  // Deduct points for compiler warnings (max -2.0 points)
  const warningPenalty = Math.min(warningCount * 0.1, 2.0);
  score -= warningPenalty;

  // Deduct points for code coverage (max -2.0 points)
  const targetCoverage = 80;
  if (codeCoverage < targetCoverage) {
    const coverageGap = targetCoverage - codeCoverage;
    const coveragePenalty = Math.min(coverageGap / 10, 2.0);
    score -= coveragePenalty;
  }

  // Deduct points for architectural violations (max -2.0 points)
  score -= Math.min(architecturalViolations * 0.5, 2.0);

  // Deduct points for security issues (max -3.0 points)
  score -= Math.min(securityIssues * 1.0, 3.0);

  // Deduct points for code complexity (max -1.0 point)
  if (averageCyclomaticComplexity > 10) {
    score -= 1.0;
  }

  return Math.max(score, 0);
}
```

---

## Scoring Breakdown

### Score Components

| Component | Weight | Description |
|-----------|--------|-------------|
| **Compiler Warnings** | 2.0 pts | Each warning = -0.1 pts (max -2.0) |
| **Test Coverage** | 2.0 pts | Below 80% coverage deducts points |
| **Architecture** | 2.0 pts | BEST template violations = -0.5 pts each |
| **Security** | 3.0 pts | Security issues = -1.0 pts each |
| **Complexity** | 1.0 pt | Average cyclomatic complexity >10 |

### Current Metrics (Baseline)

- **Overall Score**: 7.9/10
- **Compiler Warnings**: ~13 warnings
- **Test Coverage**: Not yet measured
- **Architecture**: 9/10 (BEST template compliant)
- **Security**: No critical issues detected

---

## Quality Thresholds

### Score Ranges and Actions

| Score Range | Grade | Action | Auto-Merge |
|-------------|-------|--------|------------|
| **9.0 - 10.0** | ✅ Excellent | Approved, auto-merge allowed | Yes |
| **8.0 - 8.9** | ✅ Good | Approved, manual review recommended | Conditional |
| **7.0 - 7.9** | ⚠️ Acceptable | Review required, improvements needed | No |
| **6.0 - 6.9** | ⚠️ Below Target | Blocked, must address issues | No |
| **0.0 - 5.9** | ❌ Critical | Blocked, requires major refactoring | No |

### Threshold Details

#### Excellent (9.0 - 10.0)

**Criteria**:
- ≤5 compiler warnings
- ≥80% test coverage
- 0 architectural violations
- 0 security issues
- Low code complexity

**Actions**:
- ✅ PR auto-merge allowed (if `auto-merge` label present)
- ✅ Automatic deployment to staging
- ✅ Fast-track to production

#### Good (8.0 - 8.9)

**Criteria**:
- ≤10 compiler warnings
- ≥75% test coverage
- ≤2 architectural violations
- 0 critical security issues
- Moderate code complexity

**Actions**:
- ✅ PR approval with recommendation for cleanup
- ⚠️ Manual review before merge (recommended)
- ⚠️ Conditional auto-merge (team decision)

#### Acceptable (7.0 - 7.9) - **Current State**

**Criteria**:
- ≤20 compiler warnings
- ≥70% test coverage
- ≤4 architectural violations
- 0 critical security issues
- Acceptable code complexity

**Actions**:
- ⚠️ Review required before merge
- ⚠️ Document technical debt items
- ❌ No auto-merge allowed
- 📝 Create follow-up issues for improvements

#### Below Target (6.0 - 6.9)

**Criteria**:
- >20 compiler warnings
- <70% test coverage
- >4 architectural violations
- Minor security issues present
- High code complexity

**Actions**:
- ❌ PR blocked until improvements made
- 📋 Required action items before merge
- 🔍 Mandatory code review by senior engineer
- 📝 Refactoring plan required

#### Critical (0.0 - 5.9)

**Criteria**:
- Excessive compiler warnings (>50)
- Very low test coverage (<50%)
- Major architectural violations
- Critical security issues
- Very high code complexity

**Actions**:
- ❌ PR immediately blocked
- 🚨 Alert team leads
- 📋 Comprehensive refactoring plan required
- 🔍 Architecture review mandatory
- ⏸️ Feature development paused until resolved

---

## Quality Gate Configuration

### PR Merge Requirements

```yaml
# .github/workflows/automated-code-review.yml
quality-gate:
  score-threshold: 8.0  # Minimum score for auto-merge
  blocking-threshold: 7.0  # Below this blocks PR merge
  warning-threshold: 7.5  # Shows warning but allows merge
```

### Override Mechanisms

#### Manual Override

Team leads can override quality gates with justification:

```bash
# Add label to PR
gh pr edit <pr-number> --add-label "quality-gate-override"

# Must include justification in PR comment
gh pr comment <pr-number> --body "Override reason: [explanation]"
```

#### Emergency Hotfix

Critical production issues may bypass quality gates:

```yaml
# Add to PR description
Emergency: true
Justification: Production outage affecting customers
Follow-up Issue: #XXX
```

---

## Metric Definitions

### Compiler Warnings

**What**: Issues detected by C# compiler that don't prevent compilation

**Severity Levels**:
- **CS0168**: Variable declared but never used
- **CS0219**: Variable assigned but never used
- **CS8602**: Possible null reference dereference
- **CS8604**: Possible null reference argument
- **IDE0051**: Private member is unused
- **IDE0052**: Private member value never read

**Target**: ≤5 warnings for excellent score

### Test Coverage

**What**: Percentage of code lines executed during unit tests

**Measurement**:
```bash
dotnet test /p:CollectCoverage=true /p:CoverletOutputFormat=cobertura
```

**Target**: ≥80% coverage for excellent score

### Architectural Violations

**What**: Deviations from BEST template structure

**Checks**:
- ✅ Proper layering (Abstractions, Domain, Application, Infrastructure)
- ✅ Dependency flow (outer layers depend on inner)
- ✅ CQRS pattern implementation
- ✅ MediatR usage for commands/queries
- ✅ Domain event implementation

**Target**: 0 violations for excellent score

### Security Issues

**What**: Vulnerabilities detected by security scanners

**Severity Levels**:
- **Critical**: Immediate fix required (SQL injection, XSS, etc.)
- **High**: Fix before merge (insecure crypto, auth bypass)
- **Medium**: Fix in follow-up PR (weak crypto, info disclosure)
- **Low**: Technical debt item (deprecated API usage)

**Target**: 0 critical/high issues for excellent score

### Code Complexity

**What**: Cyclomatic complexity - number of independent paths through code

**Thresholds**:
- **1-5**: Simple, easy to understand ✅
- **6-10**: Moderate complexity ✅
- **11-20**: High complexity ⚠️
- **21+**: Very high complexity ❌

**Target**: Average complexity ≤10 for excellent score

---

## Workflow Behavior

### Score Output

The workflow always emits score outputs, even on failure:

```yaml
outputs:
  score: ${{ steps.analysis.outputs.score }}
  warnings_count: ${{ steps.analysis.outputs.warnings_count }}
  issues_found: ${{ steps.analysis.outputs.issues_found }}
```

**Fallback Values**:
- If analysis fails: `score = "N/A"`
- If warnings can't be counted: `warnings_count = "0"`
- If issues check fails: `issues_found = "0"`

### Conditional Execution

Jobs use `if: always()` to ensure execution even when dependencies fail:

```yaml
manage-review-issue:
  needs: [code-quality-analysis, compliance-check]
  if: always()  # Runs even if previous jobs fail
```

### Issue Creation

Issues are created/updated based on score:

- **Score ≥8.0**: "✅ Code quality excellent"
- **Score 7.0-7.9**: "⚠️ Code quality acceptable"
- **Score <7.0**: "❌ Code quality needs improvement"

---

## Improving Your Score

### Quick Wins (30 minutes)

1. **Fix compiler warnings** (+1.0 pts)
   - Remove unused usings
   - Fix LINQ enumeration warnings
   - Remove redundant qualifiers

2. **Add missing null checks** (+0.5 pts)
   - Use nullable reference types
   - Add null-conditional operators
   - Validate method parameters

### Medium Effort (2-3 hours)

3. **Increase test coverage** (+2.0 pts)
   - Add unit tests for uncovered methods
   - Test edge cases and error conditions
   - Reach 80% coverage threshold

4. **Reduce code complexity** (+1.0 pt)
   - Extract complex methods
   - Simplify nested conditionals
   - Use early returns

### Long-term (4-6 hours)

5. **Fix architectural issues** (+2.0 pts)
   - Align with BEST template
   - Implement proper layering
   - Use CQRS pattern consistently

6. **Address security issues** (+3.0 pts)
   - Fix SQL injection risks
   - Implement input validation
   - Use secure crypto APIs

---

## Monitoring and Trends

### Score History

Track score trends over time:

```bash
# View automated review issues
gh issue list --label automated-review

# Check score trends
gh issue view <issue-number> --comments
```

### Dashboard (Future)

Planned dashboard features:
- Score trend chart (last 30 days)
- Breakdown by component (warnings, coverage, etc.)
- Comparison with team average
- Improvement recommendations

---

## Automated Actions

### GitHub Issue Creation

**Trigger**: Every code review run

**Behavior**:
- Creates one issue per day (with `review-YYYY-MM-DD` label)
- Updates existing issue if already created today
- Closes old open review issues to prevent clutter
- Links to Issue #17 and detailed reports

**Example Issue**:
```markdown
Title: Automated Code Review - 2025-11-02
Labels: automated-review, code-quality, review-2025-11-02

## Automated Code Review Results

Run: #12345678 | Commit: abc1234

- Compliance Score: 7.9/10
- Compiler Warnings: 13
- Issues Found: 5

For detailed analysis see:
- Issue #17: Code Review Analysis Report
- docs/code-review/code-review-report-2025-11-02-143000.md
```

### PR Status Checks

**Planned Feature**:
```yaml
- name: Update PR status
  if: github.event_name == 'pull_request'
  uses: actions/github-script@v7
  with:
    script: |
      const score = parseFloat('${{ steps.analysis.outputs.score }}');
      const state = score >= 8.0 ? 'success' : 'failure';
      const description = `Compliance Score: ${score}/10`;

      await github.rest.repos.createCommitStatus({
        owner: context.repo.owner,
        repo: context.repo.repo,
        sha: context.sha,
        state: state,
        context: 'code-quality/compliance-score',
        description: description
      });
```

---

## Best Practices

### For Developers

1. **Run checks locally before pushing**:
   ```bash
   dotnet build --no-incremental
   dotnet test /p:CollectCoverage=true
   ```

2. **Monitor automated review issues** for your PRs

3. **Address warnings incrementally** rather than all at once

4. **Aim for 8.0+ score** before requesting review

### For Reviewers

1. **Check compliance score** in automated issue

2. **Review detailed reports** in `docs/code-review/`

3. **Verify test coverage** meets threshold

4. **Ensure security issues** are addressed

### For Team Leads

1. **Track score trends** across sprints

2. **Set team targets** (e.g., maintain 8.5+ average)

3. **Allocate time** for technical debt reduction

4. **Celebrate improvements** in score over time

---

## FAQs

### Why is my score lower than expected?

Check the automated review issue for breakdown:
- Count of compiler warnings
- Test coverage percentage
- Specific violations listed

### Can I override the quality gate?

Yes, with team lead approval. Add `quality-gate-override` label and
justification comment.

### How often does the workflow run?

On every push to `main` or `develop` branches that affects code in:
- `src/**`
- `.github/workflows/**`
- `*.csproj` or `*.sln`

### What if the workflow fails?

Check the workflow run logs:
- Actions tab → Automated Code Review → Failed run
- Review job logs for specific errors
- Outputs are still emitted even on failure

### How do I see my score history?

```bash
# List all automated review issues
gh issue list --label automated-review

# View specific issue with comments
gh issue view <issue-number> --comments
```

---

## References

- **Workflow File**: `.github/workflows/automated-code-review.yml`
- **Main Documentation**: `docs/code-review/AUTOMATED-WORKFLOW-GUIDE.md`
- **Recovery Guide**: `docs/code-review/workflow-recovery.md`
- **Issue Mapping**: `docs/code-review/ISSUE-17-MAPPING-GUIDE.md`

---

## Next Steps

1. ✅ Read this document
2. ✅ Check your current compliance score
3. 📋 Identify quick wins to improve score
4. 🎯 Set personal target (e.g., reach 8.5/10)
5. 🔄 Monitor progress in automated issues
6. 🎉 Celebrate when reaching Excellent range!

---

**Status**: ✅ Active  
**Maintained By**: DevOps Team  
**Review Cycle**: Quarterly
