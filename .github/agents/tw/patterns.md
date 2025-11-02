# Tech Writer Agent - Documentation Templates<img src="https://r2cdn.perplexity.ai/pplx-full-logo-primary-dark%402x.png" style="height:64px;margin-right:32px"/>



This file contains reusable templates for generating consistent technical documentation across the Avro platform.# write again \# docs/.agents/tech-writer/patterns.md in codeblock please



## Changelog Entry Template```markdown

# docs/.agents/tech-writer/patterns.md

### Frontmatter

## Changelog Entry Template

```yaml

---```

title: 2025.11.0

date: 2025-11-02

type: minor***

prs:title: 2025.11.0

components: [avro.cli, avro.svc.billing]date: 2025-11-02

breaking: falsetype: minor

security: falseprs:

---components: [avro.cli, avro.svc.billing]

```breaking: false

security: false

### Changelog Format***



- **Summary**: Brief 2-3 sentence overview of the release## Summary

- **Added**: New features and capabilities

- **Changed**: Modifications to existing functionalityBrief 2-3 sentence overview of the release highlighting key user-facing changes and impact.

- **Fixed**: Bug fixes and corrections

- **Breaking**: Breaking changes requiring migration## Added

- **Security**: Security updates and vulnerability fixes

- **Deprecated**: Features marked for removal- **New feature**: Description of functionality (avro.cli) (\#3421)

- **Infrastructure**: Infrastructure and deployment changes- **API endpoint**: `/api/v2/billing/invoices` with pagination support (avro.svc.billing) (\#3428)

- **Credits**: Contributor acknowledgments- **CLI command**: `avro deploy --dry-run` for deployment preview (avro.cli) (\#3430)



### Example Entry

## Changed

```markdown

## Added- **Improved performance**: Database query optimization reduces response time by 40% (avro.svc.billing) (\#3425)

- **Updated dependency**: Upgraded to .NET 8.0.8 for security patches (platform-wide) (\#3426)

- **New feature**: Description of functionality (avro.cli) (#3421)

- **API endpoint**: `/api/v2/billing/invoices` with pagination support (avro.svc.billing) (#3428)

- **CLI command**: `avro deploy --dry-run` for deployment preview (avro.cli) (#3430)## Fixed



## Changed- **Bug fix**: Resolved memory leak in background job processor (avro.svc.workflow) (\#3422)

- **CLI issue**: Fixed `avro status` command hanging on network timeout (avro.cli) (\#3424)

- **Improved performance**: Database query optimization reduces response time by 40% (avro.svc.billing) (#3425)- **API error**: Corrected 500 error when processing empty request bodies (avro.svc.auth) (\#3427)

- **Updated dependency**: Upgraded to .NET 8.0.8 for security patches (platform-wide) (#3426)



## Fixed## Breaking



- **Bug fix**: Resolved memory leak in background job processor (avro.svc.workflow) (#3422)- **API change**: Removed deprecated `/api/v1/users` endpoint. Use `/api/v2/users` instead

- **CLI issue**: Fixed `avro status` command hanging on network timeout (avro.cli) (#3424)    - **Migration**: [See migration guide](../migrations/2025.11.0-users-api-v2.md)

- **API error**: Corrected 500 error when processing empty request bodies (avro.svc.auth) (#3427)    - **Timeline**: v1 endpoint will return 404 starting 2025-11-15

- **CLI change**: `avro init` now requires `--template` parameter

## Breaking    - **Migration**: Replace `avro init myproject` with `avro init myproject --template basic`



- **API change**: Removed deprecated `/api/v1/users` endpoint. Use `/api/v2/users` instead

    - **Migration**: [See migration guide](../migrations/2025.11.0-users-api-v2.md)## Security

    - **Timeline**: v1 endpoint will return 404 starting 2025-11-15

- **CVE-2024-12345**: Updated OpenSSL dependency to address certificate validation vulnerability

## Security    - **Risk**: Medium - affects TLS connections to external APIs

    - **Action**: Automatic with deployment, no user action required

- **CVE-2024-12345**: Updated OpenSSL dependency to address certificate validation vulnerability- **Dependency update**: Upgraded System.Text.Json to patch deserialization vulnerability

    - **Risk**: Medium - affects TLS connections to external APIs    - **Impact**: Prevents potential RCE in JSON processing endpoints

    - **Action**: Automatic with deployment, no user action required

```

## Deprecated

---

- **Feature flag**: `legacy_auth_flow` will be removed in v2026.1.0

## Release Notes Template    - **Replacement**: Use `modern_auth_flow` feature flag

    - **Timeline**: Migration required by 2025-12-31

### Frontmatter- **CLI option**: `--legacy-format` in `avro export` command

    - **Replacement**: Default JSON format or use `--format json`

```yaml    - **Removal**: Planned for v2025.12.0

---

title: Release 2025.11.0 - Enhanced Billing & CLI Improvements

date: 2025-11-02## Infrastructure

version: 2025.11.0

type: minor- **AWS update**: Migrated RDS instances to Multi-AZ for improved availability

audience: [developers, operators, end-users]- **Kubernetes**: Updated cluster to v1.28 with enhanced security policies

rollout_strategy: blue-green- **Monitoring**: Added Datadog APM for distributed tracing across services

---

```

## Credits

### Sections to Include

Thanks to @contributor1, @contributor2, and @external-contributor for their contributions to this release.

1. **What's New** - High-level overview of major features by component

2. **Breaking Changes** - Required actions and migration paths```

3. **Upgrade Notes** - Segmented by audience (Operators, Developers, End Users)

4. **Known Issues** - Tracked issues with workarounds and timelines## Release Notes Template

5. **Performance Benchmarks** - Before/after metrics in table format

6. **Verification Checklist** - Automated and manual checks with checkboxes```

7. **Rollback Procedures** - Emergency and full rollback steps with commands

8. **Support & Resources** - Documentation links and contact information

***

### Example Structuretitle: Release 2025.11.0 - Enhanced Billing \& CLI Improvements

date: 2025-11-02

- What's Newversion: 2025.11.0

  - Billing Service Enhancementstype: minor

  - CLI Developer Experienceaudience: [developers, operators, end-users]

  - Security & Performancerollout_strategy: blue-green

- Breaking Changes***

- Upgrade Notes

  - For Operators## What's New

  - For Developers

  - For End Users### Billing Service Enhancements

- Known Issues

- Performance Benchmarks (table)The billing service now supports advanced invoice management with automated reconciliation and multi-currency support. New features include:

- Verification Checklist

- Rollback Procedures- Real-time payment processing with webhook notifications

- Support & Resources- Automated dunning management for overdue accounts

- Enhanced reporting dashboard with custom date ranges

---



## Architecture Decision Record (ADR) Template### CLI Developer Experience



### FrontmatterThe Avro CLI has been significantly improved with better error messages, command auto-completion, and deployment previews:



```yaml- Interactive command wizard for complex operations

---- Built-in validation for configuration files

adr: ADR-20251102-api-versioning-strategy- Progress indicators for long-running operations

status: Proposed

date: 2025-11-02

authors: [tech-writer-agent]### Security \& Performance

reviewers: [architect-agent, @lead-developer]

related_prs: [#3421, #3428]- 40% improvement in API response times through query optimization

components: [avro.svc.*, avro.web.api]- Enhanced authentication flow with MFA support

---- Comprehensive audit logging for compliance requirements

```



### ADR Structure## Breaking Changes



1. **Context** - Problem statement and current state### API Versioning Changes

2. **Business Requirements** - What we need to achieve

3. **Decision** - The chosen approach with key principlesThe legacy v1 Users API has been removed. All integrations must migrate to v2 by November 15, 2025.

4. **Implementation Plan** - Phased approach to implementation

5. **Consequences** - Positive, negative impacts, and risks**Required Actions:**

6. **Alternatives Considered** - Other options evaluated with pros/cons

7. **Implementation Details** - Technical specifications and examples1. Update API endpoints from `/api/v1/users` to `/api/v2/users`

8. **Success Metrics** - How we measure success2. Modify request/response handling for new schema format

9. **References** - External and internal documentation links3. Test integration thoroughly in staging environment



### Decision Status Values**Migration Support:**



- `Proposed` - Under consideration- [Detailed migration guide](../migrations/2025.11.0-users-api-v2.md)

- `Accepted` - Approved and ready for implementation- Compatibility shim available until December 2025

- `Implemented` - Currently in use- Support team available for complex migrations

- `Superseded` - Replaced by newer ADR

- `Deprecated` - No longer applicable

### CLI Command Changes

---

The `avro init` command now requires explicit template selection for better project scaffolding.

## Migration Guide Template

**Before:** `avro init myproject`

### Frontmatter**After:** `avro init myproject --template basic`



```yaml## Upgrade Notes

---

title: Users API v1 to v2 Migration### For Operators

version: 2025.11.0

slug: users-api-v2-migration1. **Database Migration**: Run `avro db migrate --to 2025.11.0` during maintenance window

date: 2025-11-022. **Configuration Update**: New environment variables required for MFA feature

audience: [developers, integration-partners]3. **SSL Certificates**: Rotate certificates affected by OpenSSL vulnerability

impact: breaking-change4. **Monitoring**: Update Datadog agent configuration for new APM features

components: [avro.svc.auth, avro.web.api]

estimated_effort: 2-4 hours### For Developers

---

```1. **API Clients**: Update SDK versions to support v2 endpoints

2. **CLI Tools**: Run `avro update` to get latest CLI with new features

### Migration Guide Structure3. **Environment Setup**: Review `.env.example` for new configuration options

4. **Testing**: Update integration tests for API schema changes

1. **Overview** - Why the migration is necessary and timeline

2. **Scope** - Affected endpoints and services### For End Users

3. **Key Changes** - What's different in the new version

   - Request/Response format changes1. **Password Reset**: One-time password reset required for MFA enrollment

   - Pagination changes2. **Browser Support**: Chrome 118+ or Firefox 119+ required for new UI features

   - Error response format changes3. **Mobile App**: Update to version 3.2.0 for compatibility

4. **Step-by-Step Migration** - Detailed migration instructions

   - Update API base URLs## Known Issues

   - Update data models

   - Update pagination handling- **Issue \#3441**: Intermittent timeout on large file uploads (>100MB)

   - Update error handling    - **Workaround**: Use chunked upload via CLI: `avro upload --chunk-size 10MB`

   - Test integration    - **Resolution**: Targeted for hotfix 2025.11.1

5. **Validation & Testing** - Pre and post-migration checklists- **Issue \#3442**: Safari browser compatibility with new authentication flow

6. **Rollback Plan** - Emergency and full rollback procedures    - **Workaround**: Clear browser cache and cookies, then retry

7. **Support Resources** - Documentation, tools, and help channels    - **Resolution**: Fix scheduled for 2025.11.2



### Timeline Format

## Performance Benchmarks

```markdown

**Timeline:**| Metric | Before | After | Improvement |

| :-- | :-- | :-- | :-- |

- **2025-11-02**: v2 API available, v1 deprecated| API Response Time (p95) | 850ms | 510ms | 40% faster |

- **2025-11-15**: v1 API returns 410 Gone status| Database Query Time | 120ms | 75ms | 37% faster |

- **2025-12-01**: v1 endpoints permanently removed| CLI Command Execution | 3.2s | 2.1s | 34% faster |

```| Memory Usage (billing svc) | 2.1GB | 1.8GB | 14% reduction |



---## Verification Checklist



## Style Guide Standards### Automated Checks



### Changelog Entry Format- [ ] All services report healthy status

- [ ] API endpoints return expected response codes

```markdown- [ ] Database migrations completed successfully

- **Component**: Brief description (service/module) (#PR-NUMBER)- [ ] SSL certificates valid and properly configured

```- [ ] Load balancer health checks passing



### Breaking Change Notification

### Manual Verification

```markdown

- **Breaking**: Description of breaking change- [ ] Login flow works with new MFA requirements

    - **Migration**: [Link to migration guide](path)- [ ] Billing dashboard displays correct data

    - **Timeline**: Specific date or version deadline- [ ] CLI commands execute without errors

```- [ ] File upload/download functionality operational

- [ ] Email notifications sending properly

### Code Examples



Use language-specific code blocks with clear before/after sections:### Performance Validation



**C# Example:**- [ ] API response times within SLA (p95 < 1000ms)

- [ ] Database connection pool stable

```markdown- [ ] Memory usage within acceptable limits

**Before (v1):**- [ ] CPU utilization normal during peak hours

\`\`\`csharp

// Old implementation

private const string BaseUrl = "https://api.avro.platform/api/v1/users";## Rollback Procedures

\`\`\`

### Emergency Rollback (< 1 hour)

**After (v2):**

\`\`\`csharp1. **Load Balancer**: Route traffic back to previous version

// New implementation

private const string BaseUrl = "https://api.avro.platform/api/v2/users";```bash

\`\`\`kubectl patch service avro-api -p '{"spec":{"selector":{"version":"2025.10.2"}}}'

``````



### Performance Metrics2. **Database**: Revert schema changes if necessary



Present in table format with clear before/after comparisons:```bash

avro db rollback --to 2025.10.2

```markdown```

| Metric | Before | After | Improvement |

| :-- | :-- | :-- | :-- |3. **Feature Flags**: Disable new features

| API Response Time (p95) | 850ms | 510ms | 40% faster |

| Database Query Time | 120ms | 75ms | 37% faster |```bash

```avro feature-flag disable modern_auth_flow billing_v2_api

```

### Checklists



Use markdown checkbox format for verification and testing:### Full Rollback (1-4 hours)



```markdown1. Complete container image rollback across all services

### Pre-Migration Checklist2. Restore database from pre-upgrade backup

3. Revert configuration changes in AWS Parameter Store

- [ ] Inventory all v1 API usage in codebase4. Update DNS records if load balancer configuration changed

- [ ] Review new v2 response format and error codes

- [ ] Update data models and serialization## Support \& Resources

- [ ] Prepare integration tests for v2 endpoints

- [ ] Set up staging environment with v2 API- **Documentation**: [https://docs.avro.platform/releases/2025.11.0](https://docs.avro.platform/releases/2025.11.0)

```- **Migration Guides**: [https://docs.avro.platform/migrations](https://docs.avro.platform/migrations)

- **Support Ticket**: Create via [support portal](https://support.avro.platform)

### Known Issues Format- **Emergency Contact**: +1-555-AVRO-SOS for critical production issues

- **Community**: [Discord \#releases](https://discord.gg/avro-platform) for questions and discussions

```markdown

- **Issue #XXXX**: Brief description of the issue```

    - **Workaround**: How to work around the issue

    - **Resolution**: When and how it will be fixed## ADR Template

```

```

### Affected Endpoints



```markdown***

### Affected Endpointsadr: ADR-20251102-api-versioning-strategy

status: Proposed

- `GET /api/v1/users` → `GET /api/v2/users`date: 2025-11-02

- `POST /api/v1/users` → `POST /api/v2/users`authors: [tech-writer-agent]

- `GET /api/v1/users/{id}` → `GET /api/v2/users/{id}`reviewers: [architect-agent, @lead-developer]

```related_prs: [\#3421, \#3428]

components: [avro.svc.*, avro.web.api]

### Support Resources***



```markdown## Context

## Support Resources

The Avro platform currently supports multiple API versions simultaneously, leading to maintenance overhead and technical debt. With the introduction of new features requiring schema changes, we need a clear versioning strategy that balances backward compatibility with forward progress.

- **Documentation**: [Users API v2 Reference](https://docs.avro.platform/api/v2/users)

- **Support Ticket**: [Create ticket](https://support.avro.platform)**Current State:**

- **Community**: [Discord #api-migration](https://discord.gg/avro-api)

- **Emergency Contact**: +1-555-AVRO-SOS for critical issues- 3 active API versions (v1, v1.1, v2) across different services

```- No consistent deprecation timeline

- Mixed versioning strategies (URL path vs header vs query param)

---- 40% of traffic still using deprecated v1 endpoints



## Quality Checklist**Business Requirements:**



Before publishing any technical documentation:- Minimize disruption to existing integrations

- Enable rapid feature development

- [ ] All required sections included per template- Reduce infrastructure costs from maintaining old versions

- [ ] Frontmatter metadata complete and accurate- Improve developer experience with consistent API patterns

- [ ] All links are valid and point to correct resources

- [ ] Code examples tested and accurate

- [ ] Timeline dates are specific and realistic## Decision

- [ ] Audience segmentation is clear and helpful

- [ ] Rollback procedures documented and testedImplement a unified API versioning strategy with the following principles:

- [ ] Support contacts and resources included

- [ ] Formatting is consistent with style guide1. **Semantic Versioning**: Use semver for API versions (v2.1.0)

- [ ] No broken markdown or code fences2. **URL Path Versioning**: Version in URL path `/api/v2/resource` for clarity

- [ ] All PR numbers and commits cited correctly3. **Deprecation Timeline**: 12-month support window for major versions

- [ ] Sections follow prescribed order4. **Breaking Change Policy**: Only in major version bumps

- [ ] Tables formatted consistently5. **Sunset Communication**: 90-day notice before version removal

- [ ] Checklists use proper markdown syntax

- [ ] No grammatical or spelling errors**Implementation Plan:**



---- Phase 1: Standardize current endpoints to v2.0.0 baseline

- Phase 2: Implement deprecation headers and documentation

## Template Usage Guidelines- Phase 3: Migration tooling and compatibility layer

- Phase 4: Remove v1 endpoints after transition period

1. **Always include frontmatter** - YAML headers with metadata at the top

2. **Use consistent sections** - Follow the structure outlined for each template

3. **Link to related resources** - Cross-reference migration guides and documentation## Consequences

4. **Include specific timelines** - Be explicit about deprecation and sunset dates

5. **Segment by audience** - Provide role-specific instructions and upgrade notes### Positive

6. **Provide rollback procedures** - Include emergency and full rollback steps

7. **Add verification steps** - Include both automated and manual checks- **Reduced Maintenance**: Single active major version reduces support burden

8. **Cite sources** - Reference PR numbers, commits, and affected components- **Clear Expectations**: Well-defined deprecation timeline improves planning

9. **Use clear formatting** - Employ bold, lists, tables, and code blocks- **Better DX**: Consistent versioning improves developer experience

10. **Prioritize accuracy** - Review for precision over verbosity- **Cost Savings**: Estimated 30% reduction in API infrastructure costs



### Negative

- **Migration Effort**: Existing clients must update to new endpoints
- **Short-term Complexity**: Transition period requires supporting multiple versions
- **Breaking Changes**: Some integrations may require significant refactoring


### Risks

- **Customer Churn**: Aggressive deprecation timeline may frustrate users
- **Support Load**: Increased support tickets during migration period
- **Technical Debt**: Rushed migrations may introduce new bugs


## Alternatives Considered

### Alternative 1: Header-based Versioning

```http
GET /api/users
Accept: application/vnd.avro.v2+json
```

**Pros**: Cleaner URLs, supports content negotiation
**Cons**: Less visible, harder to debug, proxy/cache complications

### Alternative 2: No Versioning (Breaking Changes Allowed)

**Pros**: Simpler infrastructure, faster development
**Cons**: Unpredictable for clients, requires extensive testing

### Alternative 3: Feature Flags for API Changes

**Pros**: Gradual rollout, easy rollback
**Cons**: Combinatorial complexity, difficult long-term maintenance

## Implementation Details

### URL Structure

```
/api/v2/users          # Collection
/api/v2/users/123      # Resource
/api/v2/users/123/orders # Sub-resource
```


### Version Support Matrix

| Version | Status | Sunset Date | Migration Deadline |
| :-- | :-- | :-- | :-- |
| v1.0 | Deprecated | 2025-11-15 | 2025-10-15 |
| v1.1 | Deprecated | 2026-02-15 | 2026-01-15 |
| v2.0 | Active | TBD | N/A |

### Migration Support

- Automated migration tool: `avro api migrate`
- Compatibility shim for 90-day transition period
- Detailed migration guide with code examples
- Office hours for complex migration scenarios


## Success Metrics

- **Adoption Rate**: >80% of traffic on v2 within 6 months
- **Support Tickets**: <10% increase during migration period
- **Performance**: No degradation in API response times
- **Developer Satisfaction**: >4.0/5.0 in quarterly survey


## References

- [REST API Versioning Best Practices](https://restfulapi.net/versioning/)
- [GitHub API Versioning Strategy](https://docs.github.com/en/rest/overview/api-versions)
- [Stripe API Evolution](https://stripe.com/blog/api-versioning)
- **Internal**: Avro Platform Architecture Guidelines v2.1
- **Issues**: \#3421 (API Consolidation), \#3428 (Version Cleanup)

```

## Migration Guide Template

```


***
title: Users API v1 to v2 Migration
version: 2025.11.0
slug: users-api-v2-migration
date: 2025-11-02
audience: [developers, integration-partners]
impact: breaking-change
components: [avro.svc.auth, avro.web.api]
estimated_effort: 2-4 hours
***

## Overview

The Users API v1 is being retired in favor of v2, which provides improved performance, better error handling, and standardized response formats. This migration is required for all applications currently using `/api/v1/users` endpoints.

**Timeline:**

- **2025-11-02**: v2 API available, v1 deprecated
- **2025-11-15**: v1 API returns 410 Gone status
- **2025-12-01**: v1 endpoints permanently removed


## Scope

### Affected Endpoints

- `GET /api/v1/users` → `GET /api/v2/users`
- `POST /api/v1/users` → `POST /api/v2/users`
- `GET /api/v1/users/{id}` → `GET /api/v2/users/{id}`
- `PUT /api/v1/users/{id}` → `PUT /api/v2/users/{id}`
- `DELETE /api/v1/users/{id}` → `DELETE /api/v2/users/{id}`


### Services Using v1 API

Run this command to identify integrations in your codebase:

```bash
grep -r "api/v1/users" --include="*.cs" --include="*.js" --include="*.py" .
```


## Key Changes

### Request/Response Format Changes

#### User Object Schema

**v1 Format:**

```json
{
  "user_id": "123",
  "user_name": "john.doe",
  "full_name": "John Doe",
  "email_address": "john@example.com",
  "is_active": true,
  "created_at": "2025-01-15T10:30:00Z"
}
```

**v2 Format:**

```json
{
  "id": "123",
  "username": "john.doe",
  "displayName": "John Doe",
  "email": "john@example.com",
  "status": "active",
  "createdAt": "2025-01-15T10:30:00Z",
  "updatedAt": "2025-01-15T10:30:00Z",
  "profile": {
    "firstName": "John",
    "lastName": "Doe",
    "timezone": "UTC"
  }
}
```


#### Pagination Changes

**v1 Pagination:**

```json
{
  "users": [...],
  "total_count": 150,
  "page": 1,
  "per_page": 20
}
```

**v2 Pagination:**

```json
{
  "data": [...],
  "pagination": {
    "totalCount": 150,
    "pageSize": 20,
    "currentPage": 1,
    "totalPages": 8,
    "hasNext": true,
    "hasPrevious": false
  }
}
```


#### Error Response Format

**v1 Errors:**

```json
{
  "error": "User not found",
  "code": 404
}
```

**v2 Errors:**

```json
{
  "error": {
    "code": "USER_NOT_FOUND",
    "message": "User with ID '123' was not found",
    "details": {
      "userId": "123",
      "timestamp": "2025-11-02T14:30:00Z"
    }
  }
}
```


## Step-by-Step Migration

### Step 1: Update API Base URL

Replace all instances of `/api/v1/users` with `/api/v2/users` in your code.

**C\# Example:**

```csharp
// Before
private const string BaseUrl = "https://api.avro.platform/api/v1/users";

// After
private const string BaseUrl = "https://api.avro.platform/api/v2/users";
```

**JavaScript Example:**

```javascript
// Before
const apiUrl = 'https://api.avro.platform/api/v1/users';

// After
const apiUrl = 'https://api.avro.platform/api/v2/users';
```


### Step 2: Update Data Models

**C\# Model Update:**

```csharp
// Before (v1)
public class UserV1
{
    public string user_id { get; set; }
    public string user_name { get; set; }
    public string full_name { get; set; }
    public string email_address { get; set; }
    public bool is_active { get; set; }
    public DateTime created_at { get; set; }
}

// After (v2)
public class UserV2
{
    public string Id { get; set; }
    public string Username { get; set; }
    public string DisplayName { get; set; }
    public string Email { get; set; }
    public string Status { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
    public UserProfile Profile { get; set; }
}

public class UserProfile
{
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string Timezone { get; set; }
}
```


### Step 3: Update Pagination Handling

**C\# Pagination Update:**

```csharp
// Before (v1)
public class UserListResponseV1
{
    public List<UserV1> users { get; set; }
    public int total_count { get; set; }
    public int page { get; set; }
    public int per_page { get; set; }
}

// After (v2)
public class UserListResponseV2
{
    public List<UserV2> Data { get; set; }
    public PaginationInfo Pagination { get; set; }
}

public class PaginationInfo
{
    public int TotalCount { get; set; }
    public int PageSize { get; set; }
    public int CurrentPage { get; set; }
    public int TotalPages { get; set; }
    public bool HasNext { get; set; }
    public bool HasPrevious { get; set; }
}
```


### Step 4: Update Error Handling

**C\# Error Handling:**

```csharp
// Before (v1)
if (response.StatusCode == HttpStatusCode.NotFound)
{
    var error = JsonSerializer.Deserialize<V1Error>(content);
    throw new UserNotFoundException(error.error);
}

// After (v2)
if (response.StatusCode == HttpStatusCode.NotFound)
{
    var errorResponse = JsonSerializer.Deserialize<V2ErrorResponse>(content);
    throw new UserNotFoundException(errorResponse.Error.Message, errorResponse.Error.Code);
}
```


### Step 5: Test Integration

Create comprehensive tests to verify the migration:

```csharp
[Test]
public async Task GetUser_ValidId_ReturnsUserV2Format()
{
    // Arrange
    var userId = "test-user-123";

    // Act
    var user = await userService.GetUserAsync(userId);

    // Assert
    Assert.IsNotNull(user);
    Assert.AreEqual(userId, user.Id);
    Assert.IsNotNull(user.Profile);
    Assert.IsTrue(user.CreatedAt <= DateTime.UtcNow);
}

[Test]
public async Task GetUsers_WithPagination_ReturnsCorrectFormat()
{
    // Act
    var response = await userService.GetUsersAsync(page: 1, pageSize: 10);

    // Assert
    Assert.IsNotNull(response.Data);
    Assert.IsNotNull(response.Pagination);
    Assert.AreEqual(1, response.Pagination.CurrentPage);
    Assert.LessOrEqual(response.Data.Count, 10);
}
```


## Validation \& Testing

### Pre-Migration Checklist

- [ ] Inventory all v1 API usage in codebase
- [ ] Review new v2 response format and error codes
- [ ] Update data models and serialization
- [ ] Prepare integration tests for v2 endpoints
- [ ] Set up staging environment with v2 API


### Migration Validation

```bash
# Test v2 endpoint connectivity
curl -H "Authorization: Bearer $TOKEN" \
     https://api.avro.platform/api/v2/users/me

# Verify pagination works
curl -H "Authorization: Bearer $TOKEN" \
     "https://api.avro.platform/api/v2/users?page=1&pageSize=5"

# Test error handling
curl -H "Authorization: Bearer $TOKEN" \
     https://api.avro.platform/api/v2/users/nonexistent-id
```


### Post-Migration Verification

- [ ] All API calls return 200/201 status codes
- [ ] Response data matches expected v2 format
- [ ] Pagination works correctly with new structure
- [ ] Error handling processes v2 error format
- [ ] Performance meets or exceeds v1 benchmarks
- [ ] Integration tests pass in production environment


## Rollback Plan

If issues are discovered after migration:

### Immediate Rollback (< 15 minutes)

1. **Revert API URLs**: Change back to `/api/v1/users`
2. **Deploy Previous Version**: Use last known good deployment
3. **Monitor**: Check error rates and user reports

### Code Rollback (< 1 hour)

1. **Git Revert**: Revert migration commits
2. **Redeploy**: Deploy reverted code to all environments
3. **Validate**: Run smoke tests to ensure v1 functionality

### Emergency Contacts

- **API Team**: @api-team (Slack), api-support@avro.platform
- **On-Call Engineer**: +1-555-AVRO-API
- **Incident Response**: Use PagerDuty escalation policy


## Support Resources

### Documentation

- [Users API v2 Reference](https://docs.avro.platform/api/v2/users)
- [Authentication Guide](https://docs.avro.platform/auth)
- [Error Code Reference](https://docs.avro.platform/errors)


### Migration Tools

- **Migration Script**: `avro api migrate --from v1 --to v2`
- **Validation Tool**: `avro api validate --endpoint users`
- **Compatibility Checker**: `avro api check-compatibility`


### Getting Help

- **Office Hours**: Tuesdays 2-4 PM EST for migration support
- **Support Ticket**: [Create ticket](https://support.avro.platform)
- **Community**: [Discord \#api-migration](https://discord.gg/avro-api)
- **FAQ**: [Migration FAQ](https://docs.avro.platform/faq/api-migration)

```
```

