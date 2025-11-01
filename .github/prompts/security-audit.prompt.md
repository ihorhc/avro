# Security Audit Prompt

Use this prompt to conduct comprehensive security audits of code and infrastructure.

## Security Audit Checklist

### 1. Authentication & Authorization

- [ ] **Identity Management**
  - JWT tokens used with proper expiration
  - Refresh token rotation implemented
  - Multi-factor authentication available
  - Password policies enforced
  - Account lockout after failed attempts

- [ ] **Authorization**
  - Role-based access control (RBAC) implemented
  - Policy-based authorization used
  - Principle of least privilege followed
  - API endpoints properly secured with `[Authorize]`

### 2. Input Validation & Sanitization

- [ ] **Validation**
  - All inputs validated server-side
  - FluentValidation or Data Annotations used
  - File uploads validated (type, size, content)
  - No client-side validation relied upon

- [ ] **Injection Prevention**
  - SQL injection: Parameterized queries only
  - XSS: Output encoding applied
  - Command injection: Input sanitized
  - Path traversal: File paths validated
  - LDAP injection: LDAP queries parameterized

### 3. Secrets & Configuration

- [ ] **Secrets Management**
  - No hardcoded credentials
  - Azure Key Vault or equivalent used
  - Connection strings externalized
  - API keys rotated regularly
  - `.gitignore` includes secrets files

- [ ] **Configuration**
  - Sensitive config encrypted
  - Environment-specific settings separated
  - Production secrets never in source control

### 4. Data Protection

- [ ] **Encryption**
  - HTTPS enforced (TLS 1.2+)
  - Sensitive data encrypted at rest
  - Database connections encrypted
  - PII properly protected

- [ ] **Data Exposure**
  - No sensitive data in logs
  - No PII in error messages
  - Stack traces hidden in production
  - Database IDs not exposed (use GUIDs)

### 5. API Security

- [ ] **Rate Limiting**
  - Rate limits configured per endpoint
  - DDoS protection in place
  - Throttling for expensive operations

- [ ] **CORS**
  - Specific origins whitelisted (not *)
  - Credentials handled properly
  - Preflight requests supported

- [ ] **Headers**
  - Security headers configured:
    - X-Content-Type-Options: nosniff
    - X-Frame-Options: DENY
    - Content-Security-Policy
    - Strict-Transport-Security

### 6. Dependency Security

- [ ] **Package Management**
  - Dependencies regularly updated
  - Known vulnerabilities patched
  - `dotnet list package --vulnerable` clean
  - Dependency scanning in CI/CD

- [ ] **Third-party Libraries**
  - Only trusted packages used
  - Minimal dependencies
  - License compliance verified

### 7. Error Handling & Logging

- [ ] **Error Handling**
  - Global exception handler configured
  - Generic error messages to users
  - Detailed errors logged securely
  - No sensitive data in exceptions

- [ ] **Logging**
  - Security events logged
  - Audit trail for sensitive operations
  - Log aggregation configured
  - PII not logged

### 8. Session Management

- [ ] Secure session cookies (HttpOnly, Secure, SameSite)
- [ ] Session timeout configured
- [ ] Session fixation prevented
- [ ] Logout invalidates session

## OWASP Top 10 Coverage

1. **Broken Access Control** ✓
   - Authorization checks at API boundaries
   - Direct object reference protection

2. **Cryptographic Failures** ✓
   - Strong encryption algorithms
   - Proper key management

3. **Injection** ✓
   - Parameterized queries
   - Input validation

4. **Insecure Design** ✓
   - Threat modeling performed
   - Security by design

5. **Security Misconfiguration** ✓
   - Secure defaults
   - Unnecessary features disabled

6. **Vulnerable Components** ✓
   - Dependencies updated
   - Vulnerability scanning

7. **Authentication Failures** ✓
   - Strong password policies
   - MFA available

8. **Data Integrity Failures** ✓
   - Input validation
   - Digital signatures

9. **Logging Failures** ✓
   - Security events logged
   - Audit trail maintained

10. **SSRF** ✓
    - URL validation
    - Network segmentation

## Security Testing

### Automated Security Scans
```bash
# Dependency vulnerability scan
dotnet list package --vulnerable --include-transitive

# Static code analysis
dotnet build /p:EnableNETAnalyzers=true /p:EnforceCodeStyleInBuild=true

# OWASP Dependency Check
dependency-check --scan . --format HTML
```

### Manual Security Testing
- [ ] SQL injection attempts
- [ ] XSS payload testing
- [ ] Authentication bypass attempts
- [ ] Authorization escalation testing
- [ ] Session management testing
- [ ] File upload vulnerabilities

## Security Code Review Example

### ❌ Vulnerable Code
```csharp
// SQL Injection vulnerability
var sql = $"SELECT * FROM Users WHERE Username = '{username}'";
var user = await _context.Users.FromSqlRaw(sql).FirstOrDefaultAsync();

// XSS vulnerability
return Content($"<h1>Welcome {username}</h1>", "text/html");

// Hardcoded secret
var apiKey = "sk-1234567890abcdef";

// Missing authorization
[HttpGet]
public async Task<IActionResult> GetUserData(int userId)
{
    return Ok(await _context.Users.FindAsync(userId));
}
```

### ✅ Secure Code
```csharp
// Parameterized query
var user = await _context.Users
    .Where(u => u.Username == username)
    .FirstOrDefaultAsync();

// Encoded output
return Content($"<h1>Welcome {HtmlEncoder.Default.Encode(username)}</h1>", "text/html");

// Secret from Key Vault
var apiKey = _configuration["ApiKey"];

// Proper authorization
[HttpGet]
[Authorize]
public async Task<IActionResult> GetUserData(int userId)
{
    if (userId != User.GetUserId())
        return Forbid();
    
    return Ok(await _context.Users.FindAsync(userId));
}
```

## Usage

In Copilot Chat:
```
@workspace /security-audit Perform a security review of the authentication system
```

## Incident Response

If a security vulnerability is found:
1. Assess severity (use CVSS scoring)
2. Create security incident ticket
3. Patch immediately if critical
4. Update security documentation
5. Review related code areas
6. Add regression tests