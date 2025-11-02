# Automated Code Review Workflow - Recovery and Troubleshooting Guide

**Version**: 1.0  
**Last Updated**: November 2, 2025  
**For Workflow**: `automated-code-review.yml`

---

## Overview

This guide provides step-by-step instructions for troubleshooting and
recovering the automated code review workflow when issues occur. Use
this as your primary resource for diagnosing and fixing workflow
problems.

---

## Table of Contents

1. [Quick Diagnostics](#quick-diagnostics)
2. [Common Issues](#common-issues)
3. [Recovery Procedures](#recovery-procedures)
4. [Validation Steps](#validation-steps)
5. [Emergency Procedures](#emergency-procedures)

---

## Quick Diagnostics

### Check Workflow Status

```bash
# View recent workflow runs
gh run list --workflow=automated-code-review.yml --limit 10

# View specific run details
gh run view <run-id>

# View run logs
gh run view <run-id> --log

# Download run logs
gh run download <run-id>
```

### Common Symptoms

| Symptom | Likely Cause | Quick Fix |
|---------|-------------|-----------|
| Workflow doesn't trigger | Path filters don't match | Check push paths |
| Job fails immediately | YAML syntax error | Validate YAML |
| No outputs available | Analysis step failed | Check build logs |
| Issue not created | Permissions missing | Check repo settings |
| Duplicate issues | Label logic broken | Run cleanup script |
| Slow execution | Build cache missing | Clear and rebuild |

---

## Common Issues

### Issue 1: Workflow Not Triggering

**Symptom**: Push to `main`/`develop` doesn't start workflow

**Diagnosis**:
```bash
# Check last commit
git log --oneline -1

# Check modified paths
git show --name-only HEAD

# Verify workflow file exists
ls -la .github/workflows/automated-code-review.yml
```

**Root Causes**:
1. Changes don't match path filters
2. Workflow is disabled
3. YAML syntax error preventing workflow from loading

**Fix 1 - Path Filter Mismatch**:
```bash
# Workflow only runs if changes affect:
# - src/**
# - .github/workflows/**
# - *.csproj
# - *.sln

# If you changed docs/, README, etc., workflow won't run
# Solution: Manually trigger workflow
gh workflow run automated-code-review.yml
```

**Fix 2 - Workflow Disabled**:
```bash
# Check workflow status on GitHub
gh workflow list

# Enable workflow if disabled
gh workflow enable automated-code-review.yml
```

**Fix 3 - YAML Syntax Error**:
```bash
# Validate YAML locally
python3 -c "import yaml; yaml.safe_load(open('.github/workflows/automated-code-review.yml'))"

# Or use yamllint
yamllint .github/workflows/automated-code-review.yml

# Fix syntax errors and commit
git add .github/workflows/automated-code-review.yml
git commit -m "fix: correct workflow YAML syntax"
git push
```

---

### Issue 2: Job Outputs Not Available

**Symptom**: `post-summary` or `manage-review-issue` jobs show
`score: N/A` or missing values

**Diagnosis**:
```bash
# View specific job logs
gh run view <run-id> --job <job-id> --log

# Look for output writing in analysis job
grep "GITHUB_OUTPUT" <log-file>
```

**Root Causes**:
1. `code-quality-analysis` job failed early
2. Build error prevented output setting
3. `GITHUB_OUTPUT` file not writable

**Fix 1 - Job Failed Early**:

The workflow now uses `continue-on-error: true` for analysis step:

```yaml
- name: run-code-quality-checks
  id: analysis
  continue-on-error: true  # Ensures outputs are always set
```

Outputs are set even on failure, with fallback values:
- `score = "N/A"` if calculation fails
- `warnings_count = "0"` if count fails
- `issues_found = "0"` if detection fails

**Fix 2 - Build Error**:
```bash
# Check build locally
cd /path/to/repo
dotnet restore
dotnet build --configuration Debug --no-incremental

# Fix build errors and push
git add .
git commit -m "fix: resolve build errors"
git push
```

**Fix 3 - Output File Not Writable**:

This should never happen in GitHub-hosted runners, but if it does:

```yaml
# Add debug step before analysis
- name: debug-environment
  run: |
    echo "GITHUB_OUTPUT=$GITHUB_OUTPUT"
    touch "$GITHUB_OUTPUT"
    echo "test=value" >> "$GITHUB_OUTPUT"
    cat "$GITHUB_OUTPUT"
```

---

### Issue 3: Duplicate Issues Created

**Symptom**: Multiple issues with `automated-review` label for same day

**Diagnosis**:
```bash
# List automated review issues
gh issue list --label automated-review --state open

# Check for today's date label
gh issue list --label "review-$(date +%Y-%m-%d)" --state open
```

**Root Cause**: Multiple workflow runs executed simultaneously,
creating issues before uniqueness check completed

**Immediate Fix - Manual Cleanup**:
```bash
# Close duplicate issues, keeping the most recent
gh issue list --label automated-review --state open --json number,createdAt

# Close older duplicates (keep newest)
gh issue close <older-issue-number> \
  --comment "Closing duplicate, see #<newer-issue-number>"
```

**Permanent Fix**: Workflow now uses unique date-based labels:

```javascript
// In manage-review-issue job
const uniqueLabel = `review-${dateStr}`;  // e.g., review-2025-11-02

// Check for existing issue with this label
const { data: existingIssues } = await github.rest.issues.listForRepo({
  labels: [uniqueLabel],
  state: 'open',
  per_page: 1
});

// Only create if none exist with this label
if (existingIssues.length === 0) {
  await github.rest.issues.create({
    labels: ['automated-review', 'code-quality', uniqueLabel]
  });
}
```

**Prevention**:
The workflow now automatically closes old review issues when creating
a new one:

```javascript
// Close old open reviews to prevent clutter
for (const oldIssue of openReviews) {
  if (oldIssue.number !== newIssue.data.number) {
    await github.rest.issues.update({
      issue_number: oldIssue.number,
      state: 'closed'
    });
  }
}
```

---

### Issue 4: GitHub Issue Not Created

**Symptom**: Workflow completes but no issue appears

**Diagnosis**:
```bash
# Check manage-review-issue job logs
gh run view <run-id> --job <job-id> --log | grep -A 20 "create-or-update-code-review-issue"

# Look for API errors
grep "HttpError" <log-file>
```

**Root Causes**:
1. Missing `issues: write` permission
2. Repository settings block issue creation
3. GitHub API rate limit exceeded

**Fix 1 - Missing Permissions**:

Verify workflow has correct permissions:

```yaml
permissions:
  contents: read
  issues: write        # Required for issue creation
  pull-requests: read
  checks: write
```

**Fix 2 - Repository Settings**:

Check and update repository settings:

```bash
# Via GitHub UI:
# 1. Go to Settings → Actions → General
# 2. Workflow permissions section
# 3. Select "Read and write permissions"
# 4. Check "Allow GitHub Actions to create and approve pull requests"
# 5. Save

# Verify settings via API (requires admin token)
gh api repos/:owner/:repo/actions/permissions
```

**Fix 3 - Rate Limit**:
```bash
# Check rate limit status
gh api rate_limit

# Wait for reset or use GitHub App token instead of GITHUB_TOKEN
```

---

### Issue 5: Workflow Runs Too Slow

**Symptom**: Workflow takes >10 minutes to complete

**Diagnosis**:
```bash
# Check job timings
gh run view <run-id> --json jobs | jq '.jobs[] | {name, conclusion, startedAt, completedAt}'

# Identify slowest job
```

**Root Causes**:
1. Full rebuild without caching
2. Large test suite execution
3. Network latency for package restore

**Fix 1 - Enable Build Caching**:

Add caching step before build:

```yaml
- name: cache-dependencies
  uses: actions/cache@v3
  with:
    path: |
      ~/.nuget/packages
      **/obj
    key: ${{ runner.os }}-nuget-${{ hashFiles('**/*.csproj') }}
    restore-keys: |
      ${{ runner.os }}-nuget-
```

**Fix 2 - Optimize Test Execution**:
```yaml
# Run only fast unit tests, skip integration tests
- name: run-tests
  run: dotnet test --filter "Category!=Integration" --no-build
```

**Fix 3 - Use Conditional Execution**:
```yaml
# Skip compliance check if only docs changed
compliance-check:
  if: contains(github.event.head_commit.modified, 'src/')
```

---

### Issue 6: YAML Syntax Errors

**Symptom**: Workflow doesn't appear in Actions tab, or shows syntax
error

**Diagnosis**:
```bash
# Validate YAML syntax
yamllint .github/workflows/automated-code-review.yml

# Parse YAML
python3 -c "import yaml; print(yaml.safe_load(open('.github/workflows/automated-code-review.yml')))"

# Check for common issues
grep -n "  " .github/workflows/automated-code-review.yml | grep -v "^#"
```

**Common YAML Errors**:

1. **Trailing Spaces**:
```yaml
# Bad (trailing space after colon)
name: my-job  
# Good
name: my-job
```

2. **Incorrect Indentation**:
```yaml
# Bad (inconsistent indentation)
jobs:
  my-job:
   runs-on: ubuntu-latest
    steps:
# Good (consistent 2-space indentation)
jobs:
  my-job:
    runs-on: ubuntu-latest
    steps:
```

3. **YAML Anchors in Strings**:
```yaml
# Bad (YAML interprets ** as anchor)
body: **Run**: #${runId}

# Good (escape in JavaScript string)
body: "Run: #" + runId
```

**Auto-Fix Script**:
```bash
#!/bin/bash
# fix-workflow-yaml.sh

# Remove trailing spaces
sed -i 's/[[:space:]]*$//' .github/workflows/automated-code-review.yml

# Validate
yamllint .github/workflows/automated-code-review.yml
python3 -c "import yaml; yaml.safe_load(open('.github/workflows/automated-code-review.yml'))"

echo "✅ YAML fixed and validated"
```

---

## Recovery Procedures

### Procedure 1: Complete Workflow Reset

When to use: Workflow is completely broken and needs full reset

```bash
# 1. Backup current workflow
cp .github/workflows/automated-code-review.yml \
   .github/workflows/automated-code-review.yml.backup

# 2. Download known-good version from main branch
git checkout origin/main -- .github/workflows/automated-code-review.yml

# 3. Validate
yamllint .github/workflows/automated-code-review.yml
python3 -c "import yaml; yaml.safe_load(open('.github/workflows/automated-code-review.yml'))"

# 4. Test manually
gh workflow run automated-code-review.yml

# 5. Monitor run
gh run watch

# 6. If successful, commit
git add .github/workflows/automated-code-review.yml
git commit -m "fix: reset workflow to known-good state"
git push

# 7. If failed, restore backup and investigate
git checkout .github/workflows/automated-code-review.yml.backup
```

---

### Procedure 2: Clear Workflow Cache

When to use: Workflow is stuck with old cached data

```bash
# 1. View caches
gh api repos/:owner/:repo/actions/caches

# 2. Delete all caches for workflow
gh api -X DELETE repos/:owner/:repo/actions/caches

# 3. Trigger fresh run
gh workflow run automated-code-review.yml

# 4. Monitor for completion
gh run watch
```

---

### Procedure 3: Fix Duplicate Issues

When to use: Multiple automated review issues exist for same period

```bash
#!/bin/bash
# cleanup-duplicate-issues.sh

# Get all open automated review issues
ISSUES=$(gh issue list --label automated-review --state open --json number,createdAt --jq '.[] | "\(.number) \(.createdAt)"')

# Sort by date (newest first)
SORTED=$(echo "$ISSUES" | sort -k2 -r)

# Keep first (newest), close rest
KEEP_ISSUE=$(echo "$SORTED" | head -1 | awk '{print $1}')
echo "Keeping issue #$KEEP_ISSUE (newest)"

echo "$SORTED" | tail -n +2 | while read issue_num created_at; do
  echo "Closing duplicate issue #$issue_num"
  gh issue close "$issue_num" \
    --comment "Closing duplicate, see #$KEEP_ISSUE for latest review"
done

echo "✅ Cleanup complete"
```

---

### Procedure 4: Restore Missing Outputs

When to use: Workflow completed but outputs are missing/undefined

**Manual Re-run**:
```bash
# Re-run just the failed jobs
gh run rerun <run-id> --failed

# Or re-run entire workflow
gh run rerun <run-id>
```

**Force Output Values**:

If re-run doesn't work, manually create issue:

```bash
# Calculate values manually
WARNING_COUNT=$(git diff main | grep -c "warning")
SCORE="7.9"  # Use baseline
ISSUES="0"

# Create issue manually
gh issue create \
  --title "Automated Code Review - $(date +%Y-%m-%d)" \
  --label automated-review,code-quality \
  --body "## Automated Code Review Results

- Compliance Score: ${SCORE}/10
- Compiler Warnings: ${WARNING_COUNT}
- Issues Found: ${ISSUES}

For detailed analysis see Issue #17"
```

---

## Validation Steps

### After Making Changes

Always validate changes before pushing:

```bash
# 1. Validate YAML syntax
yamllint .github/workflows/automated-code-review.yml

# 2. Parse YAML structure
python3 -c "
import yaml, json
content = yaml.safe_load(open('.github/workflows/automated-code-review.yml'))
print('✅ YAML valid')
print('Jobs:', list(content['jobs'].keys()))
"

# 3. Check for required components
grep -q "code-quality-analysis" .github/workflows/automated-code-review.yml && echo "✅ Has code-quality-analysis job"
grep -q "manage-review-issue" .github/workflows/automated-code-review.yml && echo "✅ Has manage-review-issue job"
grep -q "if: always()" .github/workflows/automated-code-review.yml && echo "✅ Has conditional execution"

# 4. Verify permissions
grep -A 5 "^permissions:" .github/workflows/automated-code-review.yml | grep "issues: write" && echo "✅ Has issue write permission"

# 5. Test locally (build portion)
dotnet build --configuration Debug --no-incremental
```

---

### Post-Deployment Validation

After pushing workflow changes:

```bash
# 1. Verify workflow loads
gh workflow view automated-code-review.yml

# 2. Manually trigger
RUN_ID=$(gh workflow run automated-code-review.yml --ref $(git branch --show-current) --json | jq -r '.id')

# 3. Watch execution
gh run watch $RUN_ID

# 4. Check outputs
gh run view $RUN_ID --json jobs | jq '.jobs[] | {name, conclusion, steps: [.steps[] | {name, conclusion}]}'

# 5. Verify issue created
gh issue list --label automated-review --limit 1

# 6. Check for errors
gh run view $RUN_ID --log | grep -i "error\|failed\|exception"
```

---

## Emergency Procedures

### Emergency 1: Workflow Blocking All PRs

**Symptom**: Required status check failing, blocking all PR merges

**Immediate Action**:
```bash
# 1. Remove required status check (requires admin)
# Via GitHub UI:
# Settings → Branches → Edit protection rule
# Uncheck "Require status checks to pass"

# 2. Or bypass check for specific PRs
gh pr merge <pr-number> --admin --squash
```

**Long-term Fix**:
```bash
# 1. Fix workflow issue
# 2. Test workflow on separate branch
# 3. Re-enable required status check
```

---

### Emergency 2: Workflow Creating Spam Issues

**Symptom**: Workflow creating many issues rapidly

**Immediate Action**:
```bash
# 1. Disable workflow immediately
gh workflow disable automated-code-review.yml

# 2. Close all spam issues
for issue in $(gh issue list --label automated-review --limit 100 --json number --jq '.[].number'); do
  gh issue close $issue --comment "Closing due to workflow malfunction"
done

# 3. Investigate root cause in workflow logs
gh run list --workflow=automated-code-review.yml --limit 10

# 4. Fix issue
# 5. Test on separate branch
# 6. Re-enable workflow
gh workflow enable automated-code-review.yml
```

---

### Emergency 3: Workflow Consuming Excessive Actions Minutes

**Symptom**: Workflow running continuously, burning Actions quota

**Immediate Action**:
```bash
# 1. Cancel all running workflows
for run in $(gh run list --workflow=automated-code-review.yml --status in_progress --json databaseId --jq '.[].databaseId'); do
  gh run cancel $run
done

# 2. Disable workflow
gh workflow disable automated-code-review.yml

# 3. Check for trigger loops
git log --oneline -20 | grep "Automated commit"

# 4. Fix trigger conditions
# 5. Add safeguards (max runs per day)
# 6. Re-enable carefully
```

---

## Monitoring and Alerts

### Set Up Monitoring

```bash
# Monitor workflow failure rate
gh run list --workflow=automated-code-review.yml --limit 50 \
  --json conclusion --jq 'group_by(.conclusion) | map({conclusion: .[0].conclusion, count: length})'

# Alert on consecutive failures
FAILURES=$(gh run list --workflow=automated-code-review.yml --limit 5 --json conclusion --jq '[.[] | select(.conclusion == "failure")] | length')
if [ $FAILURES -ge 3 ]; then
  echo "⚠️ ALERT: 3+ consecutive workflow failures"
  # Send notification
fi
```

### Health Check Script

```bash
#!/bin/bash
# workflow-health-check.sh

echo "🔍 Checking automated code review workflow health..."

# Check last run
LAST_RUN=$(gh run list --workflow=automated-code-review.yml --limit 1 --json conclusion,startedAt --jq '.[0]')
CONCLUSION=$(echo "$LAST_RUN" | jq -r '.conclusion')
STARTED=$(echo "$LAST_RUN" | jq -r '.startedAt')

echo "Last run: $STARTED - $CONCLUSION"

# Check for recent issues
RECENT_ISSUES=$(gh issue list --label automated-review --limit 7 --json number,createdAt --jq 'length')
echo "Review issues (last 7 days): $RECENT_ISSUES"

# Check permissions
PERMS=$(grep -A 5 "^permissions:" .github/workflows/automated-code-review.yml)
echo "Permissions configured: $(echo "$PERMS" | grep -c "write")"

# Overall health
if [ "$CONCLUSION" = "success" ] && [ $RECENT_ISSUES -gt 0 ]; then
  echo "✅ Workflow is healthy"
  exit 0
else
  echo "⚠️ Workflow may have issues"
  exit 1
fi
```

---

## Support and Escalation

### Self-Service Resources

1. **This Document**: Primary troubleshooting guide
2. **Workflow Guide**: `docs/code-review/AUTOMATED-WORKFLOW-GUIDE.md`
3. **Scoring Thresholds**: `docs/code-review/scoring-thresholds.md`
4. **GitHub Docs**: https://docs.github.com/actions

### Escalation Path

1. **Level 1** (Self-Service): Use this guide + workflow logs
2. **Level 2** (Team): Post in team Slack channel with run ID
3. **Level 3** (DevOps): Create issue with `devops` label
4. **Level 4** (Emergency): Contact on-call engineer

### When to Escalate

- Workflow broken for >4 hours
- Affecting multiple developers/PRs
- Security implications
- Data loss or corruption

---

## Changelog

### Version 1.0 (2025-11-02)

Initial release with:
- ✅ Workflow troubleshooting procedures
- ✅ Common issue resolutions
- ✅ Emergency procedures
- ✅ Validation steps
- ✅ Monitoring guidance

---

## Quick Reference

```bash
# View workflow status
gh workflow view automated-code-review.yml

# Run manually
gh workflow run automated-code-review.yml

# View recent runs
gh run list --workflow=automated-code-review.yml --limit 10

# View specific run
gh run view <run-id>

# View run logs
gh run view <run-id> --log

# Cancel run
gh run cancel <run-id>

# Re-run failed jobs
gh run rerun <run-id> --failed

# List review issues
gh issue list --label automated-review

# Close duplicate issues (keep newest)
gh issue close <issue-number>
```

---

**Status**: ✅ Active  
**Maintained By**: DevOps Team  
**Last Tested**: November 2, 2025
