# Automation Documentation

This directory contains documentation for automated workflows and processes in
the Avro platform repository.

## Contents

- [AI Auto-Development Workflow](./AI_AUTODEV_WORKFLOW.md) - Complete guide to
  the autonomous AI development pipeline
- [CodeQL Security Scanning](./CODEQL_WORKFLOW.md) - Code security analysis and
  vulnerability scanning

## Overview

The Avro platform uses GitHub Actions workflows to automate various development
tasks. This documentation provides detailed information about each workflow,
including prerequisites, usage, and troubleshooting.

## Available Workflows

### AI Auto-Development

**File**: `.github/workflows/ai-autodev.yml`

Autonomous development pipeline that uses AI agents to implement features from
GitHub issues.

**Key Features**:
- Automatic feature branch creation
- Parallel agent execution (implementation, testing, DevOps)
- Automated code review and quality checks
- Pull request generation
- Optional auto-merge

**See**: [AI_AUTODEV_WORKFLOW.md](./AI_AUTODEV_WORKFLOW.md)

### CodeQL Security Scanning

**File**: `.github/workflows/codeql.yml`

Automated security analysis using GitHub's CodeQL engine to detect
vulnerabilities and security issues in the codebase.

**Key Features**:
- Multi-language analysis (C#, JavaScript/TypeScript)
- Automated vulnerability detection
- Scheduled weekly scans
- Integration with GitHub Security tab

**See**: [CODEQL_WORKFLOW.md](./CODEQL_WORKFLOW.md)

## Quick Start

### Using AI Auto-Development

1. Create an issue with detailed requirements
2. Add the `ai-ready` label
3. Monitor the workflow in GitHub Actions
4. Review and merge the generated PR

For more details, see the [AI Auto-Development Workflow
documentation](./AI_AUTODEV_WORKFLOW.md).

## Contributing

When adding new workflows or modifying existing ones:

1. Update or create documentation in this directory
2. Include prerequisites, usage examples, and troubleshooting
3. Validate workflow YAML files before committing
4. Test workflows in a development branch first

## Validation Tools

Use these tools to validate workflows before deploying:

```bash
# Quick validation script (recommended)
./docs/automation/validate-workflow.sh

# Manual validation with individual tools:

# YAML syntax validation
yamllint .github/workflows/*.yml

# GitHub Actions workflow validation
actionlint .github/workflows/*.yml
```

## Support

For questions or issues with workflows:

1. Check the specific workflow documentation in this directory
2. Review workflow run logs in GitHub Actions
3. Create an issue with the `automation` label
