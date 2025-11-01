# GitHub Copilot Instructions

This directory contains the complete structure for GitHub Copilot instructions for the Avro platform.

## Directory Structure

```
.github/
├── copilot-instructions.md          # Main entry point
├── instructions/                     # Topic-specific instructions
│   ├── modern-csharp-features.md
│   ├── nullable-reference-types.md
│   ├── async-await-patterns.md
│   ├── cqrs-mediatr.md
│   ├── event-driven-architecture.md
│   ├── microservices-communication.md
│   ├── api-gateway-patterns.md
│   ├── saga-pattern.md
│   ├── ef-core-patterns.md
│   ├── repository-pattern.md
│   ├── middleware-pipeline.md
│   ├── rate-limiting.md
│   ├── api-versioning.md
│   ├── input-validation.md
│   ├── sql-injection-prevention.md
│   ├── secrets-management.md
│   ├── unit-testing.md
│   ├── integration-testing.md
│   ├── architecture-collection.md   # Architecture aggregator
│   ├── security-collection.md       # Security aggregator
│   └── testing-collection.md        # Testing aggregator
├── collections/
│   └── .index.yaml                  # YAML index for collections
└── prompts/                         # On-demand prompts
    ├── api-review.prompt.md
    ├── performance-tuning.prompt.md
    └── security-audit.prompt.md
```

## How It Works

### 1. Main Entry Point
`copilot-instructions.md` is the universal entry point that GitHub Copilot reads first. It contains:
- High-priority project rules
- General coding standards
- Navigation links to all topics

### 2. Topic-Specific Instructions
Each `.md` file in `instructions/` focuses on a specific topic with:
- **Path-based scoping** via `applyTo` header
- **Specific guidelines** with code examples
- **Anti-patterns** to avoid

Example:
```markdown
---
applyTo: "src/**/*.cs"
---

# Modern C# Features
...
```

### 3. Collections
Collections aggregate related topics for comprehensive reviews:
- **Architecture Collection**: CQRS, events, microservices
- **Security Collection**: Validation, injection prevention, secrets
- **Testing Collection**: Unit and integration testing

### 4. On-Demand Prompts
Prompt files for specialized tasks:
- **API Review**: Endpoint review checklist
- **Performance Tuning**: Optimization strategies
- **Security Audit**: Vulnerability assessment

### 5. Collections Index
`.index.yaml` provides programmatic access to collections for automation and tooling.

## Usage Examples

### In Code Editor
When editing `src/Services/OrderService.cs`, Copilot automatically loads:
- `copilot-instructions.md`
- `modern-csharp-features.md` (matches `src/**/*.cs`)
- `nullable-reference-types.md` (matches `src/**/*.cs`)
- `async-await-patterns.md` (matches `src/**/*.cs`)

### In Copilot Chat

**Use a collection:**
```
@workspace Use the architecture collection to design a new payment service
```

**Use a prompt:**
```
@workspace /api-review Review the OrdersController
```

**Reference specific instruction:**
```
@workspace Follow the CQRS patterns from .github/instructions/cqrs-mediatr.md
```

### In Pull Requests

Add to PR description:
```markdown
## Review Checklist
- [ ] Follows [Security Collection](.github/instructions/security-collection.md)
- [ ] Passes [API Review](.github/prompts/api-review.prompt.md)
```

## Maintenance

### Adding New Topics
1. Create new `.md` file in `instructions/`
2. Add `applyTo` header for path scoping
3. Update relevant collection file
4. Update `.index.yaml`
5. Add link in `copilot-instructions.md`

### Updating Collections
1. Edit the collection `.md` file
2. Update `.index.yaml` if structure changes
3. Test with Copilot Chat

### Best Practices
- Keep individual instruction files focused (5-15 lines of guidance)
- Use code examples liberally
- Update anti-patterns based on code reviews
- Reference official documentation when applicable
- Keep `copilot-instructions.md` under 3KB

## Integration with CI/CD

You can use the collections in automated checks:

```yaml
# .github/workflows/code-quality.yml
- name: Validate Security Practices
  run: |
    # Check that no secrets are committed
    # Validate input validation is present
    # Run security linters
```

## Tips for Developers

1. **Before starting work**: Review relevant collection
2. **During development**: Copilot uses scoped instructions automatically
3. **Before PR**: Run through relevant prompt checklist
4. **During review**: Reference specific instruction files in comments

## Resources

- [GitHub Copilot Documentation](https://docs.github.com/en/copilot)
- [Copilot Instructions Format](https://docs.github.com/en/copilot/customizing-copilot/adding-custom-instructions-for-github-copilot)
- [Path-based Scoping](https://docs.github.com/en/copilot/customizing-copilot/adding-custom-instructions-for-github-copilot#scope-instructions-to-specific-files)