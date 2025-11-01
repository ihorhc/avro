# AI Agents Guide

This guide explains how AI agents work within the AVRO platform and how to leverage them effectively.

## Overview

AVRO is designed to enable autonomous AI agent workflows while maintaining human architectural oversight. The platform provides:

1. **Structured Context**: Optimized context files for efficient navigation
2. **Autonomy Levels**: Clear rules for autonomous vs. supervised changes
3. **Workflow Automation**: Pre-built workflows for common tasks
4. **Safety Guardrails**: Critical path protection and human approval gates

## Context Optimization

AI agents should load context files at initialization:

### 1. Navigation Context
Load `ai/context/navigation.json` to understand:
- Directory structure
- Key files and entry points
- Quick navigation paths
- Where to find specific functionality

### 2. Architectural Patterns
Load `ai/context/patterns.json` to learn:
- Naming conventions
- Architecture patterns
- Code structure
- Testing conventions

### 3. Dependency Graph
Load `ai/context/dependency-graph.json` to understand:
- Project dependencies
- Impact analysis
- Critical nodes
- Change propagation

### 4. Autonomy Rules
Load `ai/context/autonomy-rules.json` to know:
- When autonomous changes are allowed
- When human review is required
- Critical paths requiring oversight
- Safety checks

## Autonomy Levels

### Autonomous
AI agents can make changes without human review when:
- Project autonomyLevel is "autonomous"
- Changes are within non-critical paths
- No public API changes
- All tests pass
- No security vulnerabilities

**Examples:**
- Adding unit tests
- Refactoring internal implementations
- Documentation updates
- Code formatting

### Supervised
Changes require human review when:
- Project autonomyLevel is "supervised"
- Changes affect critical paths
- Public API modifications
- Complex business logic changes

**Examples:**
- Core business logic
- API signature changes
- Database schema changes
- Security modifications

### Manual
Human implementation required when:
- Project autonomyLevel is "manual"
- Architectural decisions needed
- Security-critical changes
- Breaking changes

**Examples:**
- Major refactoring
- Security vulnerability fixes
- Production data migrations
- Compliance changes

## Workflows

### Issue-to-PR Workflow

The primary autonomous workflow converts issues to pull requests:

1. **Trigger**: Issue labeled with `ai:auto-implement` or comment `/ai implement`
2. **Analysis**: Extract requirements and constraints
3. **Context**: Gather relevant code and patterns
4. **Autonomy Check**: Determine if autonomous or supervised
5. **Design**: Create solution design
6. **Implementation**: Make code changes
7. **Validation**: Run tests and security checks
8. **PR Creation**: Submit pull request
9. **Review**: Wait for approval if supervised

### Using the Workflow

```bash
# In a GitHub issue, add label or comment:
/ai implement

# The AI agent will:
# 1. Analyze the issue
# 2. Check if it can proceed autonomously
# 3. Create a solution
# 4. Submit a PR
# 5. Request review if needed
```

## Best Practices

### For AI Agents

1. **Always load context first** - Don't navigate blindly
2. **Check autonomy level** - Know when review is needed
3. **Follow patterns** - Use established conventions
4. **Run tests frequently** - Validate changes early
5. **Request help when uncertain** - Use escape hatches

### For Humans

1. **Review supervised changes** - Check AI-generated PRs
2. **Update context files** - Keep them current
3. **Refine autonomy rules** - Adjust based on experience
4. **Monitor workflows** - Track success rates
5. **Provide feedback** - Help AI agents improve

## Task Registry Integration

AI agents should:
- Check task metadata for complexity and requirements
- Use context files referenced in tasks
- Update task status as work progresses
- Link PRs to tasks

## Project Registry Integration

AI agents should:
- Check project autonomy level
- Understand dependencies
- Follow build configurations
- Respect critical paths

## Safety Mechanisms

### Pre-Commit Checks
- Run all tests
- Security vulnerability scan
- Schema validation
- Breaking change detection
- Code coverage verification

### Post-Commit Checks
- Create detailed PR description
- Tag appropriate reviewers
- Run CI/CD pipeline
- Await review for supervised changes

### Escape Hatches
When AI agents encounter issues:
- **Unclear requirements**: Request clarification
- **Conflicting constraints**: Present options
- **Unrelated test failures**: Report and skip
- **Security concerns**: Escalate to security team

## Monitoring and Metrics

Track AI agent performance:
- Execution time
- Success rate
- Test coverage
- Human intervention rate
- Error patterns

## Common Patterns

### Adding a Feature
1. Check task registry for requirements
2. Load relevant context
3. Design minimal solution
4. Implement with tests
5. Validate and submit PR

### Fixing a Bug
1. Analyze issue description
2. Find affected code via context
3. Implement fix
4. Add regression test
5. Validate and submit PR

### Refactoring
1. Check autonomy level
2. Design refactoring plan
3. Make incremental changes
4. Run tests after each change
5. Submit PR with clear rationale

## Advanced Topics

### Custom Workflows
Create custom workflows for specific needs:
- Define triggers
- Configure stages
- Set approval requirements
- Add monitoring

### Context Customization
Extend context files for your needs:
- Add project-specific patterns
- Define custom autonomy rules
- Include domain knowledge

### Integration
Integrate with external tools:
- GitHub Actions
- CI/CD pipelines
- Monitoring systems
- Communication tools

## Troubleshooting

### Agent Not Starting
- Check context file syntax
- Verify workflow triggers
- Review logs

### Incorrect Behavior
- Review context files
- Check autonomy rules
- Validate task metadata

### Failed Tests
- Check if tests were already failing
- Validate changes
- Review error messages

## Future Enhancements

Planned improvements:
- Enhanced context learning
- Multi-agent collaboration
- Advanced impact analysis
- Predictive dependency management
