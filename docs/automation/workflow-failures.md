# AI Autodev Workflow Failure Troubleshooting Guide

## Overview

This document provides comprehensive troubleshooting guidance for the AI autodev workflow (`ai-autodev.yml`). It covers common failure scenarios, diagnostic procedures, and resolution steps.

## Quick Diagnosis

When a workflow fails, the `cleanup-on-failure` job provides immediate diagnostic information:

1. **Check the failure comment** on the issue - it lists which jobs failed and which were skipped
2. **Review the workflow run logs** - link provided in the failure comment
3. **Examine the diagnostic output** in the `cleanup-on-failure` job's `diagnose-failure` step

## Common Failure Scenarios

### 1. Check-Trigger Job Failures

**Symptoms:**
- Workflow starts but immediately fails
- No branch is created
- `ai-processing` label may not be added

**Possible Causes:**
- Issue missing `ai-ready` label
- GitHub API rate limit exceeded
- Permissions issue with adding labels

**Resolution:**
1. Verify issue has `ai-ready` label
2. Check GitHub API rate limits: `gh api rate_limit`
3. Verify workflow permissions in `.github/workflows/ai-autodev.yml`:
   ```yaml
   permissions:
     contents: write
     issues: write
     pull-requests: write
   ```

**Diagnostic Commands:**
```bash
# Check if label exists in repository
gh label list | grep ai-ready

# View workflow permissions
gh workflow view ai-autodev --yaml | grep -A5 permissions
```

### 2. Branch Creation Failures

**Symptoms:**
- `analyze-architecture` job fails at `create-feature-branch` step
- Error: "Failed to push branch after X attempts"
- Branch exists locally but not on remote

**Possible Causes:**
- Branch protection rules preventing GitHub Actions from pushing
- Token permissions insufficient
- Network connectivity issues
- Branch already exists with different commits

**Resolution:**
1. **Check branch protection rules:**
   - Navigate to Settings → Branches → Branch protection rules
   - Ensure GitHub Actions is allowed to push to feature branches
   - Pattern `ai-feature/*` should allow force pushes from GitHub Actions

2. **Verify token permissions:**
   ```yaml
   permissions:
     contents: write  # Required for branch creation
   ```

3. **Manual branch cleanup (if needed):**
   ```bash
   # Delete stuck branch
   git push origin --delete ai-feature/issue-X
   ```

**Prevention:**
- Branch creation step includes retry logic (3 attempts with exponential backoff)
- Idempotent design: safely handles existing branches

### 3. Missing or Invalid Outputs

**Symptoms:**
- Jobs skip unexpectedly
- `parallel-agents` job doesn't run
- Error: "branch_name is empty"

**Possible Causes:**
- Upstream job failed silently
- Output variable not set correctly
- YAML syntax error in output reference

**Resolution:**
1. **Check output definitions in workflow:**
   ```yaml
   outputs:
     branch_name: ${{ steps.create_branch.outputs.branch_name }}
   ```

2. **Verify step ID matches:**
   - Step `id: create_branch` must match reference in outputs
   - Common typo: `check_readiness` vs `check-readiness`

3. **Review workflow logs** for the failing step:
   ```bash
   gh run view <run-id> --log
   ```

**Enhanced Logging:**
- All critical steps now use `::group::` and `::notice::` annotations
- Output values logged before being set
- Branch verification after creation

### 4. Parallel Agents Job Failures

**Symptoms:**
- One or more agent jobs fail
- Branch mismatch errors
- Checkout fails with "ref not found"

**Possible Causes:**
- Branch not pushed to remote before agents start
- Branch name output not propagated correctly
- Concurrent modifications to branch

**Resolution:**
1. **Verify branch exists remotely:**
   ```bash
   git ls-remote --heads origin ai-feature/issue-X
   ```

2. **Check job dependencies:**
   ```yaml
   needs: [check-trigger, analyze-architecture]
   ```

3. **Review agent execution logs:**
   - Look for "Branch mismatch" errors
   - Verify `BRANCH_NAME` environment variable

**Configuration:**
- `fail-fast: false` ensures all agents complete even if one fails
- Each agent verifies branch before executing

### 5. Code Review Job Failures

**Symptoms:**
- Build errors after agent changes
- Linter failures
- Security scan failures

**Possible Causes:**
- Generated code doesn't compile
- Code style violations
- Known vulnerabilities in dependencies

**Resolution:**
1. **Review build errors:**
   ```bash
   dotnet build --no-incremental
   ```

2. **Run linter locally:**
   ```bash
   dotnet format --verify-no-changes
   ```

3. **Check for security issues:**
   ```bash
   dotnet list package --vulnerable --include-transitive
   ```

**Note:** Code review checks are currently commented out in framework mode. Enable when agent-generated code is present.

### 6. Pull Request Creation Failures

**Symptoms:**
- PR not created
- Error: "No commits between main and feature branch"
- Permissions error

**Possible Causes:**
- No changes committed to feature branch
- Token lacks `pull-requests: write` permission
- Network/API issues

**Resolution:**
1. **Verify commits exist on branch:**
   ```bash
   git log main..ai-feature/issue-X
   ```

2. **Check PR creation permissions:**
   ```yaml
   create-pull-request:
     permissions:
       contents: read
       pull-requests: write
   ```

3. **Manual PR creation (fallback):**
   ```bash
   gh pr create --base main --head ai-feature/issue-X \
     --title "AI: <issue title>" \
     --body "Automated changes for issue #X"
   ```

## Diagnostic Tools

### Workflow Summary

Each run now includes workflow summary annotations:

```bash
# View workflow summary
gh run view <run-id>
```

Look for:
- `::notice::` - Successful operations
- `::warning::` - Non-critical issues
- `::error::` - Critical failures
- `::group::` - Collapsible log sections

### Job Status Inspection

```bash
# List all jobs for a run
gh run view <run-id> --json jobs --jq '.jobs[] | {name, status, conclusion}'

# View specific job logs
gh run view <run-id> --log --job <job-id>
```

### Output Variables

The `diagnose-failure` step in `cleanup-on-failure` logs all output variables:

```
Check-trigger status: success
Analyze-architecture status: failure
Parallel-agents status: skipped
...
Outputs:
- should_process: true
- issue_number: 123
- branch_name: ai-feature/issue-123
- architecture_result: {...}
```

## Preventive Measures

### 1. Enhanced Logging

All critical operations now include:
- Entry/exit logging with `::group::`
- Success notifications with `::notice::`
- Error context with `::error::`
- Variable value logging

### 2. Defensive Checks

- `set -e` in all bash scripts (fail fast on errors)
- Branch verification after creation
- Output validation before use
- Null checks in JavaScript

### 3. Retry Logic

- Branch push: 3 attempts with exponential backoff
- Commit/push: Graceful handling of "no changes" scenarios

### 4. Idempotency

- Safe to re-run workflow on same issue
- Handles existing branches gracefully
- Concurrent run protection via concurrency group

## Escalation Path

If standard troubleshooting doesn't resolve the issue:

1. **Collect diagnostic information:**
   ```bash
   gh run view <run-id> --log > workflow-failure.log
   gh run view <run-id> --json > workflow-failure.json
   ```

2. **Check GitHub Status:**
   - Visit https://www.githubstatus.com
   - Look for API or Actions outages

3. **Review recent changes:**
   ```bash
   git log --oneline .github/workflows/ai-autodev.yml -10
   ```

4. **Create debug issue:**
   - Include workflow run link
   - Attach logs
   - Describe expected vs actual behavior

## Testing the Workflow

### Dry Run Test

1. Create a test issue:
   ```markdown
   Title: Test AI Workflow
   
   This is a test issue to verify the AI autodev workflow.
   
   Expected: Workflow creates branch, runs agents, creates PR.
   ```

2. Add `ai-ready` label

3. Monitor workflow execution:
   ```bash
   gh run watch
   ```

4. Verify outputs:
   - Branch created: `git ls-remote --heads origin ai-feature/issue-*`
   - Labels updated: Check issue has `ai-processing` or `ai-completed`
   - PR created: `gh pr list --head ai-feature/issue-*`

### Validation Script

Use the provided validation script:

```bash
./docs/automation/validate-workflow.sh
```

This checks:
- YAML syntax validity
- Job naming consistency
- Required outputs defined
- Permissions correctly set

## Workflow Configuration

### Required Secrets

| Secret | Description | Required |
|--------|-------------|----------|
| `GITHUB_TOKEN` | Automatically provided by GitHub Actions | Yes |

**Note:** `GITHUB_TOKEN` is scoped to the repository and has limited permissions. For cross-repo operations, create a Personal Access Token (PAT).

### Required Labels

The workflow expects these labels to exist:

| Label | Color | Description |
|-------|-------|-------------|
| `ai-ready` | #0E8A16 | Triggers workflow |
| `ai-processing` | #FBCA04 | Active processing |
| `ai-completed` | #0E8A16 | Successfully completed |
| `ai-failed` | #D93F0B | Pipeline failed |
| `ai-generated` | #1D76DB | Applied to PRs |
| `ready-for-review` | #0E8A16 | Applied to PRs |

Create missing labels:

```bash
gh label create ai-ready --color 0E8A16 --description "Issue is ready for AI-powered development pipeline processing"
# ... repeat for other labels
```

### Timeout Configuration

Current timeouts:

- `check-trigger`: 5 minutes
- `analyze-architecture`: 10 minutes
- `parallel-agents`: 30 minutes (per agent)
- `code-review`: 20 minutes
- `create-pull-request`: 10 minutes
- `auto-approve`: 5 minutes
- `cleanup-on-failure`: 5 minutes

Adjust based on your repository size and complexity.

## Known Issues

### Issue #1: Branch Protection Rules

**Problem:** Default branch protection prevents GitHub Actions from pushing.

**Workaround:** Configure branch protection to allow GitHub Actions:
1. Settings → Branches → Add rule
2. Branch name pattern: `ai-feature/*`
3. Enable "Allow force pushes" for GitHub Actions

### Issue #2: Rate Limiting

**Problem:** High frequency of workflow runs may hit GitHub API rate limits.

**Workaround:** 
- Use concurrency groups (already configured)
- Implement caching for API calls
- Consider GitHub Apps for higher rate limits

## Related Documentation

- [AI Autodev Workflow Guide](./AI_AUTODEV_WORKFLOW.md)
- [GitHub Actions Documentation](https://docs.github.com/en/actions)
- [Workflow Syntax Reference](https://docs.github.com/en/actions/using-workflows/workflow-syntax-for-github-actions)

## Changelog

### 2025-11-02
- Initial troubleshooting guide created
- Added diagnostic procedures for all job types
- Documented common failure scenarios
- Added preventive measures and enhanced logging

---

**Maintained by:** DevOps Team  
**Last Updated:** 2025-11-02  
**Version:** 1.0
