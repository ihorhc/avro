# Security Policy

## Supported Versions

Security updates are provided for the following versions of Avro and its microservices. Legacy versions do not receive security patches.

| Version | Supported          | .NET Target | End of Support |
| ------- | ------------------ | ----------- | -------------- |
| 2.x.x   | :white_check_mark: | .NET 8      | 2027-11-10     |
| 1.5.x   | :white_check_mark: | .NET 6      | 2026-06-30     |
| 1.0.x   | :x:                | .NET 6      | 2024-12-31     |
| < 1.0   | :x:                | .NET 5      | EOL            |

**Note:** In this monorepo, each microservice may have its own versioning scheme. Security patches are coordinated across all services in the same release cycle.

## Reporting a Vulnerability

We take security seriously and appreciate responsible disclosure. If you discover a security vulnerability in Avro or any service within this monorepo, **please report it privately** rather than using public issues.

### How to Report

1. **Email:** security@[your-org].com (replace with your security contact)
2. **GitHub Private Vulnerability Reporting:** Use the ["Report a vulnerability"](https://docs.github.com/en/code-security/security-advisories/guidance-on-reporting-and-writing-information-about-vulnerabilities/privately-reporting-a-security-vulnerability) button in the Security tab
3. **Include in your report:**
   - Affected version(s) and microservice(s)
   - Vulnerability type and severity (CVSS score if available)
   - Proof of concept or steps to reproduce
   - Potential impact assessment
   - Suggested remediation (if available)

### Response Timeline

- **Initial Response:** Within 48 hours of receipt
- **Status Update:** Every 7 days during investigation
- **Patch Release:** Within 30 days for critical vulnerabilities, 90 days for others (when feasible)
- **Public Disclosure:** Coordinated with you before announcement

### Scope

Security vulnerabilities covered by this policy include:

- Authentication/authorization flaws
- Injection attacks (SQL, command, etc.)
- Sensitive data exposure
- Broken access controls
- Cryptographic failures
- Infrastructure vulnerabilities in AWS integration
- Dependency vulnerabilities in NuGet packages

### What to Expect

- **Accepted:** We'll create a GitHub Security Advisory and coordinate a fix release
- **Declined:** We'll explain why and provide guidance if it's a non-security issue
- **CVE Assignment:** For critical vulnerabilities, a CVE will be requested through MITRE

## Security Best Practices

### For Contributors

- Use **Dependabot alerts** to track vulnerable dependencies
- Sign commits with GPG keys (`git config user.signingkey`)
- Never commit credentials, API keys, or connection strings
- Use AWS Secrets Manager for production secrets
- Scan for secrets using `git-secrets` or similar tools before pushing

### For Maintainers

- Enable branch protection rules (require reviews, status checks)
- Use code scanning with GitHub Advanced Security or CodeQL
- Audit dependencies with `dotnet list package --vulnerable`
- Rotate PATs every 90 days, use GitHub Apps where possible [web:5]
- Review GitHub audit logs monthly for suspicious activity

## Dependency Management

Avro uses automated dependency scanning:

- **NuGet:** Monitored via `dotnet list package --outdated`
- **Dependabot:** Enabled for automated PR creation on vulnerable packages
- **CodeQL:** Scans C# code for common security patterns

## Security Advisories

Publicly disclosed vulnerabilities are published in our [GitHub Security Advisories page](link-to-your-advisories).

## Acknowledgments

We recognize and thank security researchers who responsibly report vulnerabilities. With your permission, we'll acknowledge your contribution in the advisory.
