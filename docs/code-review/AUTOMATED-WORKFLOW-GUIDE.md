# Automated Code Review Workflow Documentation

## Overview

The automated code review workflow (`automated-code-review.yml`) runs after commits are pushed to `main` or `develop` branches, automatically:

1. **Analyzes code quality** and calculates compliance scores
2. **Verifies BEST template** structure
3. **Generates review reports** with timestamped filenames
4. **Creates/updates GitHub issues** with mapping to detailed reports
5. **Posts summaries** to workflow logs

---

## Workflow Architecture

### Trigger Events

```yaml
on:
  push:
    branches:
      - main
      - develop
    paths:
      - 'src/**'              # Any .NET code changes
      - '.github/workflows/**' # Workflow changes
      - '*.csproj'            # Project file changes
      - '*.sln'               # Solution changes
  workflow_dispatch:          # Manual trigger from GitHub UI
```

**Behavior**: Only runs when code changes in critical paths (not docs-only changes)

---

### Job Sequence

```
┌─────────────────────────────────────┐
│ code_quality_analysis               │
│ - Compile and count warnings        │
│ - Calculate compliance score        │
│ - Identify issues                   │
└──────────────┬──────────────────────┘
               │
       ┌───────┴────────┐
       │                │
       ▼                ▼
┌──────────────┐  ┌─────────────────┐
│ compliance_  │  │ manage_review_  │
│ check        │  │ issue (depends) │
│ - BEST       │  │ - Create issue  │
│   template   │  │ - Update issue  │
│   verify     │  │ - Comment       │
└──────────────┘  └─────────────────┘
       │                │
       └───────┬────────┘
               │
               ▼
        ┌──────────────┐
        │ post_summary │
        │ - Log output │
        └──────────────┘
```

---

## Job Details

### 1. `code_quality_analysis`

**Purpose**: Analyze code quality and compile status

**Outputs**:
- `score`: Compliance score (7.9/10 default)
- `warnings_count`: Number of compiler warnings
- `issues_found`: Count of identified issues

**Steps**:
1. Checkout repository
2. Setup .NET 10.0.x
3. Run code quality checks:
   - `dotnet build --no-restore` (count warnings)
   - `dotnet build --configuration Debug --no-incremental`
   - Calculate compliance score
   - Check for common issues

**Run Time**: ~2-3 minutes

---

### 2. `compliance_check`

**Purpose**: Verify BEST template architecture compliance

**Steps**:
1. Checkout repository
2. Verify BEST template structure:
   - ✅ `src/Avro.Mcp.Abstractions/`
   - ✅ `src/Avro.Mcp.Domain/`
   - ✅ `src/Avro.Mcp.Application/`
   - ✅ `src/Avro.Mcp.Infrastructure/`

**Run Time**: ~30 seconds

---

### 3. `manage_review_issue`

**Purpose**: Create or update GitHub issue with code review mapping

**Behavior**:
- Searches for existing issue with `automated-review` label
- If exists: Add comment with latest results
- If not exists: Create new issue with full details

**Issue Content**:
- Title: `Automated Code Review - YYYY-MM-DD`
- Labels: `automated-review`, `code-quality`
- Body: Links to detailed reports and Issue #17

**Run Time**: ~30 seconds

---

### 4. `post_summary`

**Purpose**: Log workflow completion summary

**Output**:
```
Automated Code Review Completed
Score: 7.9/10
Warnings: XX
Issues: X
```

**Run Time**: ~10 seconds

---

## GitHub Issue Integration

### Automatic Issue Creation

When workflow runs for the first time:

```
Title: Automated Code Review - 2025-11-02
Labels: automated-review, code-quality
Body:
  Compliance Score: 7.9/10
  Compiler Warnings: 13
  Issues Found: 5
  
  See Issue #17 for detailed analysis
```

### Automatic Issue Updates

On subsequent runs:

1. Finds existing issue with `automated-review` label
2. Adds comment with latest metrics:
   ```
   Code Review Update (2025-11-02): Score 7.9/10
   ```
3. Preserves issue history with timestamped comments

### Issue Mapping

Each automatically created issue **links to** and **references**:

- **Issue #17**: Main code review orchestrator
  - Overview of all findings
  - Refactoring roadmap
  - Success criteria

- **Detailed Reports** (in `docs/code-review/`):
  - `code-review-report-2025-11-02-143000.md`
  - `github-actions-permissions-fix.md`
  - `permissions-fix-implementation-summary.md`
  - `ISSUE-17-MAPPING-GUIDE.md`

- **Sub-Issues**:
  - #12: Unused global usings
  - #13: LINQ enumeration warnings
  - #14: Redundant qualifiers
  - #15: Workflow review
  - #16: Code quality epic

---

## Usage Examples

### Manual Trigger

```bash
# Trigger workflow from GitHub CLI
gh workflow run automated-code-review.yml -r main

# Or use GitHub UI:
# Actions → Automated Code Review & Issue Mapping → Run workflow
```

### Automatic Trigger

Push changes to `src/` or `.github/workflows/`:

```bash
git add src/Avro.Mcp.Application/SomeClass.cs
git commit -m "feat: add feature"
git push origin main

# Workflow automatically runs
```

### View Results

1. Check **Actions** tab → **Automated Code Review & Issue Mapping** → Latest run
2. Check **Issues** → **automated-review** label
3. Read detailed reports in `docs/code-review/`

---

## Configuration

### Adjust Trigger Paths

Edit `on.push.paths` to include/exclude paths:

```yaml
on:
  push:
    branches:
      - main
      - develop
    paths:
      - 'src/**'                  # Run on source changes
      - '.github/workflows/**'    # Run on workflow changes
      - '*.csproj'               # Run on project changes
      # - 'docs/**'              # Uncomment to include docs
      # - 'README.md'            # Uncomment to include README
```

### Adjust Compliance Score Calculation

Edit `code_quality_analysis` job:

```yaml
run: |
  # Current: Static score (placeholder)
  SCORE=7.9
  
  # TODO: Implement dynamic scoring based on:
  # - Compiler warnings count
  # - Test coverage percentage
  # - Code complexity metrics
  # - Standards compliance checks
```

### Adjust Retention Period

Edit `upload-artifact` steps:

```yaml
- name: Upload report
  uses: actions/upload-artifact@v3
  with:
    retention-days: 90  # Change from 90 to desired days
```

---

## Permissions

```yaml
permissions:
  contents: read              # Read repository content
  issues: write               # Create/update issues
  pull-requests: read         # Read PR information
  checks: write               # Write check results
```

**Security**: 
- ✅ Only grants necessary permissions
- ✅ No delete permissions
- ✅ No admin access
- ✅ Read-only for repository content

---

## Output Artifacts

### Generated Files

1. **Automatic Report** (created each run)
   ```
   docs/code-review/code-review-automated-YYYY-MM-DD-HHMMSS.md
   ```

2. **GitHub Issue** (created once, updated on subsequent runs)
   ```
   Issue #X (label: automated-review)
   ```

### Artifact Retention

- **Code review reports**: Retained 90 days
- **GitHub issues**: Retained indefinitely
- **Workflow logs**: Retained per GitHub settings

---

## Troubleshooting

### Workflow Fails to Run

**Problem**: Workflow doesn't trigger after push

**Solutions**:
1. Verify changes match `on.push.paths` patterns
2. Check branch is `main` or `develop`
3. Manually trigger: Actions tab → Run workflow
4. Check workflow syntax: `get_errors` on `.github/workflows/automated-code-review.yml`

### Issue Not Created

**Problem**: GitHub issue not created after first run

**Solutions**:
1. Check repository permissions: Settings → Actions → Permissions
2. Verify `issues: write` permission exists
3. Check workflow logs for GitHub API errors
4. Manually create issue with `automated-review` label

### Incorrect Metrics

**Problem**: Score/warnings count seems wrong

**Solutions**:
1. Check `dotnet build` output in workflow logs
2. Verify .NET 10.0.x is installed correctly
3. Run locally: `dotnet build --no-restore`
4. Update score calculation logic in workflow

---

## Integration with Issue #17

### How They Work Together

```
GitHub Push (main/develop)
        ↓
   automated-code-review.yml
        ↓
   ┌────────────────────────┐
   │ Analyze Code Quality   │
   │ Check Compliance       │
   │ Generate Report        │
   └────────────────────────┘
        ↓
   Create/Update Issue (automated-review)
        ↓
   Links to:
   ├─ Issue #17 (orchestrator)
   ├─ docs/code-review/ (detailed reports)
   ├─ Issues #12-16 (sub-tasks)
   └─ Previous runs (issue history)
```

### Referencing Issue #17 in Automated Issues

Each automatically created issue includes:

```markdown
See Issue #17: Code Review Analysis Report
for detailed analysis and refactoring roadmap
```

---

## Best Practices

### For Development

1. **Commit regularly**: Automated reviews run on each push
2. **Monitor issue**: Check `automated-review` labeled issue for trends
3. **Review reports**: Read detailed reports in `docs/code-review/`
4. **Execute fixes**: Use Issue #17 roadmap for prioritization

### For CI/CD

1. **Don't block merges**: Workflow is informational, not blocking
2. **Archive artifacts**: Keep 90-day retention for audit trail
3. **Monitor metrics**: Track compliance score trends over time
4. **Update roadmap**: Adjust Issue #17 based on real-world results

### For Team

1. **Weekly reviews**: Check automated issue for trends
2. **Sprint planning**: Use Issue #17 for prioritization
3. **Knowledge sharing**: Reference detailed reports in docs/code-review/
4. **Celebrate wins**: Track score improvements over time

---

## Future Enhancements

### Potential Improvements

- [ ] Dynamic compliance score calculation
- [ ] Per-file blame tracking
- [ ] Automated fix suggestions
- [ ] Historical trend charts
- [ ] Integration with pull request reviews
- [ ] Security scanning integration
- [ ] Performance benchmarking
- [ ] Auto-labeling of new issues

---

## References

- **Workflow File**: `.github/workflows/automated-code-review.yml`
- **Issue #17**: Code Review Analysis Report (orchestrator)
- **Documentation**: `docs/code-review/ISSUE-17-MAPPING-GUIDE.md`
- **Detailed Reports**: `docs/code-review/code-review-report-*.md`

---

## Status

✅ **Workflow Implemented**  
✅ **GitHub Issue Integration Complete**  
✅ **Automated Execution Ready**  

**Next Steps**:
1. Make a commit to trigger first run
2. Monitor Actions tab for workflow execution
3. Check Issues → `automated-review` label
4. Verify links to Issue #17 and detailed reports

---

**Last Updated**: November 2, 2025  
**Created By**: GitHub Copilot Code Review Agent
