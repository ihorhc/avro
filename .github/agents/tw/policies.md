## Documentation Generation Policies

### ADR (Architecture Decision Record) Generation Rules

#### When to Create ADR
- **Required Labels**: PR has labels `architecture`, `design-decision`, `rfc`, `technical-debt`
- **Discussion Threshold**: PR discussion has >10 comments with architectural debate or design alternatives
- **Impact Scope**: Changes affect multiple services or have platform-wide implications
- **Technology Decisions**: Introduction of new frameworks, libraries, or architectural patterns
- **Data Architecture**: Database schema changes, data flow modifications, or storage strategy changes
- **Security Architecture**: Authentication/authorization changes, security policy updates
- **Performance Architecture**: Caching strategies, load balancing decisions, scalability approaches

#### When to Skip ADR
- **Routine Changes**: Bug fixes, dependency updates, documentation-only changes
- **Implementation Details**: Code refactoring without architectural impact
- **Configuration Changes**: Environment-specific settings or feature flag toggles
- **UI/UX Changes**: Frontend modifications without backend architectural impact
- **Test-Only Changes**: Adding or modifying tests without production code changes

#### ADR Quality Requirements
- **Decision Context**: Clear problem statement and constraints
- **Alternatives Considered**: At least 2 alternative approaches with trade-offs
- **Consequences**: Both positive and negative impacts documented
- **Implementation Timeline**: When decision takes effect and migration timeline
- **Success Metrics**: How to measure if decision was correct
- **Review Process**: Required approvers and review timeline

### Migration Guide Generation Rules

#### Required Migration Guide Scenarios
- **Breaking Changes**: `breaking-change` label present on PR
- **API Changes**: Public API signatures modified in .cs files
- **Database Schema**: Migrations affecting existing data or requiring manual steps
- **Configuration Changes**: New required environment variables or config files
- **CLI Breaking Changes**: Commands/flags removed or behavior significantly modified
- **Authentication Changes**: Login flow or security model modifications
- **Deployment Process**: Changes to CI/CD pipeline or infrastructure requirements

#### Migration Guide Content Requirements
- **Impact Assessment**: Which services, users, or integrations are affected
- **Prerequisites**: Required tools, permissions, or environmental setup
- **Step-by-Step Instructions**: Numbered, actionable steps with verification
- **Code Examples**: Before/after code samples for complex changes
- **Rollback Procedures**: How to revert changes if issues occur
- **Timeline Requirements**: Deadlines for completing migration
- **Support Resources**: Where to get help during migration
- **Risk Assessment**: Potential issues and mitigation strategies

#### Migration Guide Validation
- **Technical Review**: Must be approved by component owner
- **User Testing**: Migration steps tested in staging environment
- **Documentation Review**: Technical writing team review for clarity
- **Timeline Validation**: Realistic timeframes based on change complexity

### Version Bump Logic

#### Semantic Versioning Rules
- **Major Version (X.0.0)**:
  - `breaking-change` label present
  - `release:major` label override
  - Public API removes or significantly changes existing functionality
  - Database schema changes requiring data migration
  - Authentication/authorization model changes
  - CLI command removal or incompatible behavior changes

- **Minor Version (X.Y.0)**:
  - `feat` label present (new features)
  - New public API endpoints or methods added
  - `release:minor` label override
  - New CLI commands or optional parameters
  - New configuration options with sensible defaults
  - Performance improvements without behavior changes

- **Patch Version (X.Y.Z)**:
  - `fix` label present (bug fixes)
  - `perf` label present (performance improvements)
  - `security` label present (security patches)
  - `release:patch` label override
  - Documentation improvements
  - Dependency updates without API changes

#### Version Override Rules
- **Skip Release**: `no-release` label skips version bump entirely
- **Force Version**: `release:major/minor/patch` overrides automatic detection
- **Hotfix Process**: Emergency releases may bypass normal versioning rules
- **RC Versions**: Release candidates use suffix `-rc.1`, `-rc.2`, etc.

#### Version Validation
- **Consistency Check**: Version bump matches change impact
- **Dependency Validation**: Downstream services can handle version change
- **Breaking Change Verification**: All breaking changes have migration guides
- **Release Note Completeness**: All user-facing changes documented

### Component Detection and Categorization

#### Path-Based Component Mapping
- **CLI Components**:
  - `src/avro.cli/` → `avro.cli`
  - `tools/avro/` → `avro.cli`
  - `cli/` → `avro.cli`

- **Service Components**:
  - `services/auth/` → `avro.svc.auth`
  - `services/billing/` → `avro.svc.billing`
  - `services/workflow/` → `avro.svc.workflow`
  - `src/avro.svc.*/` → corresponding service

- **Web Components**:
  - `web/dashboard/` → `avro.web.dashboard`
  - `src/avro.web.*/` → corresponding web app
  - `frontend/` → `avro.web`

- **Infrastructure Components**:
  - `infra/terraform/` → `avro.infra.terraform`
  - `infra/k8s/` → `avro.infra.k8s`
  - `infrastructure/` → `avro.infra`
  - `.github/workflows/` → `ci/cd`

- **Documentation Components**:
  - `docs/` → `documentation`
  - `README.md` → `documentation`
  - `.github/` → `ci/cd`

#### Multi-Component Changes
- **Cross-Service**: Changes affecting multiple services listed separately
- **Platform-Wide**: Changes affecting all components use `platform-wide` tag
- **Shared Libraries**: Changes to shared code reference all dependent components

### Content Quality Standards

#### Changelog Quality Gates
- **Summary Required**: Every release must have 2-3 sentence summary
- **User Impact**: Focus on user-visible changes, not internal refactoring
- **Component Attribution**: Every change tagged with affected component
- **PR References**: All changes cite PR numbers (#1234)
- **Author Credits**: Contributors acknowledged with GitHub handles

#### Breaking Changes Requirements
- **Clear Impact Statement**: Exactly what breaks and why
- **Migration Path**: Link to detailed migration guide
- **Timeline Communication**: When change takes effect
- **Support Information**: How to get help during migration
- **Risk Assessment**: Potential issues and mitigation strategies

#### Security Content Requirements
- **CVE References**: Official vulnerability identifiers when available
- **Risk Level**: Impact severity (Critical, High, Medium, Low)
- **Affected Versions**: Specific version ranges impacted
- **Mitigation Steps**: Required actions for users
- **Timeline**: Urgency and patch availability

### Review and Approval Process

#### Automated Quality Checks
- **Required Sections**: Changelog has Summary, component tags, PR references
- **Breaking Change Validation**: Breaking changes have migration guides
- **Security Validation**: Security fixes have proper classification
- **Link Validation**: All internal links resolve correctly
- **Format Validation**: Proper markdown formatting and YAML frontmatter

#### Human Review Requirements
- **Breaking Changes**: Require architect agent approval
- **Security Content**: Require security team review
- **Major Releases**: Require product team review
- **Customer-Facing Changes**: Require support team notification

#### Review Timeline
- **Standard Changes**: 24-hour review window
- **Breaking Changes**: 72-hour review window
- **Security Updates**: 4-hour expedited review
- **Emergency Hotfixes**: Real-time review with on-call approval

### Label Categories and Mapping

#### Change Type Labels
- `feat` → **Added** section
- `fix` → **Fixed** section
- `perf` → **Performance** section
- `security` → **Security** section
- `docs` → **Documentation** section (excluded from changelog unless user-facing)
- `refactor` → Internal improvement (excluded from changelog)
- `test` → Test improvement (excluded from changelog)
- `chore` → Maintenance (excluded from changelog)

#### Impact Labels
- `breaking-change` → **Breaking** section
- `deprecation` → **Deprecated** section
- `enhancement` → **Changed** section
- `api-change` → Requires API documentation update
- `database-change` → Requires migration guide

#### Priority Labels
- `critical` → Emergency release process
- `high-priority` → Next minor release
- `medium-priority` → Scheduled release
- `low-priority` → Future release consideration

#### Special Processing Labels
- `no-release` → Skip version bump and changelog
- `docs:auto-commit` → Allow automatic documentation commits
- `migration-required` → Force migration guide generation
- `customer-impact` → Require customer communication plan
