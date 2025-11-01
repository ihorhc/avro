# Security Collection

This collection aggregates security best practices and guidelines for the Avro platform.

## Included Topics

### Input Security
- [Input Validation](./input-validation.md) - Data annotations and FluentValidation
- [SQL Injection Prevention](./sql-injection-prevention.md) - Parameterized queries and safe SQL practices

### Secrets & Authentication
- [Secrets Management](./secrets-management.md) - Azure Key Vault, User Secrets, and credential management

### API Security
- [Rate Limiting](./rate-limiting.md) - Throttling and DoS prevention
- [Middleware Pipeline](./middleware-pipeline.md) - Proper middleware ordering including auth/authz

## When to Use This Collection

Reference this collection when:
- Implementing new API endpoints
- Reviewing code for security vulnerabilities
- Setting up authentication/authorization
- Handling user input or external data
- Conducting security audits
- Preparing for penetration testing
- Onboarding new developers

## Security Checklist

Before deploying to production:
- [ ] All inputs validated server-side
- [ ] No secrets in source control
- [ ] SQL injection prevention verified
- [ ] Rate limiting configured
- [ ] Authentication/authorization implemented
- [ ] HTTPS enforced
- [ ] Security headers configured
- [ ] Sensitive data properly encrypted
- [ ] Logging doesn't expose PII

## Usage

In Copilot Chat:
```
@workspace Use the security collection to review this API endpoint for vulnerabilities
```

In PR descriptions:
```
Security review required - see .github/instructions/security-collection.md
```