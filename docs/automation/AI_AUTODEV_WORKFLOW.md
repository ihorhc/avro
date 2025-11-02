# AI Auto-Development Workflow

## Overview

The AI auto-development workflow (`ai-autodev.yml`) implements an automated
pipeline for autonomous feature development using specialized AI agents. This
workflow enables GitHub issues labeled with `ai-ready` to automatically trigger
a complete development cycle, from architecture analysis through pull request
creation.

## Workflow Architecture

### Pipeline Stages

The workflow consists of the following stages:

1. **Trigger Check** - Validates issue is ready for AI processing
2. **Architecture Analysis** - Creates feature branch and validates design
3. **Parallel Implementation** - Runs implementation, testing, and DevOps agents
   simultaneously
4. **Code Review** - Executes automated quality checks and linting
5. **Pull Request Creation** - Generates PR with all changes
6. **Auto-Approve** (optional) - Automatically approves and merges if
   `auto-merge` label is present

### Job Flow Diagram

```
Issue (ai-ready label)
  ↓
check_trigger
  ↓
analyze_architecture (creates feature branch)
  ↓
parallel_agents (implementation, testing, devops)
  ↓
code_review
  ↓
create_pull_request
  ↓
auto_approve (if auto-merge label present)
```

## Prerequisites

### Required Secrets

| Secret Name | Description | Required |
|-------------|-------------|----------|
| `GITHUB_TOKEN` | GitHub API token for workflow operations | Yes |

**Note:** `GITHUB_TOKEN` is automatically provided by GitHub Actions and does
not need to be manually configured.

### Required Permissions

The workflow requires the following permissions in the repository:

- `contents: write` - For creating and pushing branches
- `issues: write` - For adding labels and comments to issues
- `pull-requests: write` - For creating and managing pull requests
- `checks: write` - For code quality checks
- `statuses: write` - For status updates

### Repository Configuration

1. **Branch Protection**: Ensure the `main` branch allows force pushes from
   GitHub Actions if using auto-merge
2. **Labels**: The following labels must exist in the repository:
   - `ai-ready` - Triggers the workflow
   - `ai-processing` - Added during workflow execution
   - `ai-completed` - Added upon successful completion
   - `ai-failed` - Added if the workflow fails
   - `ai-generated` - Applied to generated pull requests
   - `ready-for-review` - Applied to generated pull requests
   - `auto-merge` (optional) - Enables automatic PR approval and merge

## Usage

### Triggering the Workflow

1. Create a new issue with detailed requirements
2. Add the `ai-ready` label to the issue
3. The workflow will automatically trigger and begin processing

### Adding Auto-Merge

To enable automatic approval and merge after all checks pass:

1. Add both `ai-ready` and `auto-merge` labels to the issue
2. The workflow will automatically approve and merge the generated PR after
   successful validation

### Issue Format

For best results, structure your issue as follows:

```markdown
Title: Clear, concise feature description

Description:
## Requirements
- Specific requirement 1
- Specific requirement 2

## Acceptance Criteria
- [ ] Criterion 1
- [ ] Criterion 2

## Technical Details
- Architecture pattern to follow
- Integration points
- Dependencies
```

## Workflow Outputs

### Job Outputs

Each job produces outputs that downstream jobs can consume:

#### `check_trigger`

- `should_process`: Boolean indicating if processing should continue
- `issue_number`: GitHub issue number
- `issue_title`: Issue title
- `issue_body`: Full issue description

#### `analyze_architecture`

- `architecture_result`: JSON string with architecture analysis
- `branch_name`: Name of the created feature branch
- `branch_created`: Boolean indicating successful branch creation

#### `create_pull_request`

- `pr_number`: Generated pull request number
- `status`: Outcome of PR creation step

## Agent Integration

### Current State (Framework)

The workflow currently implements the framework structure with placeholder
steps. Actual Copilot agent invocation requires GitHub Copilot API access.

### Agent Types

The workflow coordinates three parallel agents:

1. **Implementation Agent** - Writes production code
2. **Testing Agent** - Creates unit and integration tests
3. **DevOps Agent** - Configures CI/CD and infrastructure

Each agent operates independently on the same feature branch.

### Future Integration

When GitHub Copilot API becomes available, replace placeholder steps with:

```bash
gh copilot run --agent ".github/agents/{agent}.agent.md" \
  --context "${ISSUE_TITLE}: ${ISSUE_BODY}"
```

Or integrate with custom Copilot solutions that can execute agent instructions.

## Defensive Features

### Branch Creation

The workflow includes defensive checks when creating branches:

- Validates branch creation before proceeding
- Publishes success/failure to workflow summary
- Sets `branch_created` output for downstream jobs
- Fails early if branch cannot be created or pushed

### Error Handling

- **Parallel agents** use `fail-fast: false` to allow all agents to complete
  even if one fails
- **Cleanup job** runs on failure and:
  - Posts failure comment to issue
  - Removes `ai-processing` label
  - Adds `ai-failed` label

### Workflow Summary

Critical operations publish to the GitHub Actions workflow summary for easy
troubleshooting:

- Branch creation success/failure
- Agent execution results
- Quality check outcomes

## Testing the Workflow

### Dry Run Testing

To test the workflow without making permanent changes:

1. Create a test issue in a forked repository
2. Add the `ai-ready` label
3. Monitor the workflow execution in the Actions tab
4. Review the created branch and PR (do not merge)
5. Clean up by closing the PR and deleting the branch

### Validation

Before deploying changes to the workflow:

1. **Quick Validation** (recommended):
   ```bash
   ./docs/automation/validate-workflow.sh
   ```

2. **Manual YAML Validation**:
   ```bash
   yamllint .github/workflows/ai-autodev.yml
   ```

3. **Manual Workflow Validation**:
   ```bash
   actionlint .github/workflows/ai-autodev.yml
   ```

The validation script (`docs/automation/validate-workflow.sh`) performs
comprehensive checks including:
- YAML syntax validation
- GitHub Actions workflow validation
- Job naming consistency
- Required job and output verification
- Documentation completeness

## Troubleshooting

### Common Issues

1. **Branch creation fails**
   - Check that the workflow has `contents: write` permission
   - Verify no branch protection rules prevent GitHub Actions from pushing

2. **PR creation fails**
   - Ensure `GITHUB_TOKEN` has `pull-requests: write` permission
   - Check that there are actual changes on the feature branch

3. **Labels not being added**
   - Verify all required labels exist in the repository
   - Check `issues: write` permission is granted

4. **Auto-merge not working**
   - Confirm `auto-merge` label is present on the issue
   - Verify branch protection rules allow auto-merge
   - Check that all required status checks pass

## Monitoring

### Workflow Status

Monitor workflow execution through:

- GitHub Actions tab in the repository
- Issue comments posted by the workflow
- Labels applied to the issue (`ai-processing`, `ai-completed`, `ai-failed`)

### Outputs and Logs

- **Workflow Summary**: Check the summary tab in each workflow run for
  high-level status
- **Job Logs**: Review individual job logs for detailed execution information
- **PR Description**: Generated PRs include a summary of all changes made

## Maintenance

### Updating the Workflow

When making changes to the workflow:

1. Edit `.github/workflows/ai-autodev.yml`
2. Validate with `yamllint` and `actionlint`
3. Test in a development branch before merging
4. Update this documentation to reflect changes

### Agent Updates

To modify agent behavior:

1. Edit agent definition files in `.github/agents/`
2. Test with a sample issue
3. Review generated code and tests
4. Iterate on agent instructions as needed

## Security Considerations

### Secret Management

- Never commit secrets to the workflow file
- Use GitHub Secrets for sensitive data
- `GITHUB_TOKEN` permissions are scoped to the minimum required

### Code Review

Even with automated agents, human review is recommended:

- Review generated code before merging
- Validate test coverage and quality
- Check for security vulnerabilities
- Ensure compliance with coding standards

### Access Control

- Limit who can add the `ai-ready` and `auto-merge` labels
- Use CODEOWNERS to require review for generated PRs
- Monitor workflow execution for unexpected behavior

## References

- [GitHub Actions Documentation](https://docs.github.com/en/actions)
- [GitHub Actions Workflow Syntax](https://docs.github.com/en/actions/using-workflows/workflow-syntax-for-github-actions)
- [GitHub Copilot Documentation](https://docs.github.com/en/copilot)
