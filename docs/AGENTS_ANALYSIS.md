# GitHub Copilot Custom Agents - Suggested for Avro Platform

## Analysis Summary

**Repository Context:**
- **Primary Language**: C# (.NET 10.0 RC)
- **Project Type**: Microservices with MCP server orchestrator
- **Architecture**: Clean Architecture, CQRS, Domain-Driven Design
- **Key Components**: CLI tool, process orchestration, service management

**Identified Patterns:**
- Advanced C# with nullable reference types and file-scoped namespaces
- Async/await patterns throughout
- MCP server development (avro-mcp-orchestrator)
- Design pattern implementation needed

---

## Custom Agents Recommendations

| Awesome-Copilot Custom Agent | Description | Already Installed | Suggested | Rationale |
|------------------------------|-------------|-------------------|-----------|-----------|
| [C# Expert](https://github.com/github/awesome-copilot/blob/main/agents/CSharpExpert.agent.md) | An agent designed to assist with software development tasks for .NET projects. Expert in async/await, SOLID principles, design patterns, performance optimization, and testing best practices. | ❌ No | ✅ **HIGH** | Directly aligns with Avro's .NET architecture. Essential for ensuring code quality, design patterns implementation, and async programming best practices across the platform. |
| [Arm Migration Agent](https://github.com/github/awesome-copilot/blob/main/agents/arm-migration.agent.md) | Accelerates moving x86 workloads to Arm infrastructure. Scans for architecture assumptions, portability issues, container base images and dependency incompatibilities. | ❌ No | ⚠️ **MEDIUM** | Useful for future cloud deployment and infrastructure optimization. Helps ensure Avro services are portable across different architectures for AWS/Azure deployment. |
| [Dynatrace Expert](https://github.com/github/awesome-copilot/blob/main/agents/dynatrace-expert.agent.md) | Integrates observability and security capabilities. Enables development teams to investigate incidents, validate deployments, triage errors, detect performance regressions, validate releases, and manage security vulnerabilities. | ❌ No | ⚠️ **MEDIUM** | Valuable for production monitoring of microservices. Helps implement observability patterns for the orchestrator and managed services using traces, logs, and metrics. |
| [JFrog Security Agent](https://github.com/github/awesome-copilot/blob/main/agents/jfrog-sec.agent.md) | Dedicated Application Security agent for automated security remediation. Verifies package and version compliance, and suggests vulnerability fixes using JFrog security intelligence. | ❌ No | ⚠️ **MEDIUM** | Supports the project's security standards. Ensures NuGet packages are secure and compliant, particularly important for microservice architectures. |
| [Neon Migration Specialist](https://github.com/github/awesome-copilot/blob/main/agents/neon-migration-specialist.agent.md) | Safe Postgres migrations with zero-downtime using Neon's branching workflow. Test schema changes in isolated database branches, validate thoroughly, then apply to production. | ❌ No | 💡 **LOW** | Useful if Avro services use PostgreSQL. Enables safe database schema evolution for Aurora RDS workloads (Avro uses RDS per architecture guidelines). |
| [Neon Performance Analyzer](https://github.com/github/awesome-copilot/blob/main/agents/neon-optimization-analyzer.agent.md) | Identify and fix slow Postgres queries automatically using Neon's branching workflow. Analyzes execution plans and provides performance metrics with actionable code fixes. | ❌ No | 💡 **LOW** | Complements EF Core patterns used in Avro. Helps optimize database queries for microservices without affecting production. |
| [PagerDuty Incident Responder](https://github.com/github/awesome-copilot/blob/main/agents/pagerduty-incident-responder.agent.md) | Responds to PagerDuty incidents by analyzing incident context, identifying recent code changes, and suggesting fixes via GitHub PRs. | ❌ No | 💡 **LOW** | Enhances incident response workflows. Could automate remediation suggestions for production incidents in Avro services. |
| [Terraform Agent](https://github.com/github/awesome-copilot/blob/main/agents/terraform.agent.md) | Helps developers adhere to Terraform configurations, use approved modules, apply correct tags, and ensure Terraform best practices by default. | ❌ No | 💡 **LOW** | Not immediately relevant (Avro uses AWS CDK in C# per architecture). Could be useful for future infrastructure-as-code scenarios. |
| [Stackhawk Security Onboarding](https://github.com/github/awesome-copilot/blob/main/agents/stackhawk-mcp.agent.md) | Automatically set up StackHawk security testing for your repository with generated configuration and GitHub Actions workflow. | ❌ No | 💡 **LOW** | Supports security testing. Could automate API security scanning for ASP.NET Core services in Avro. |
| [Amplitude Experiment Implementation](https://github.com/github/awesome-copilot/blob/main/agents/amplitude-experiment-implementation.agent.md) | Uses Amplitude's MCP tools to deploy new experiments, enabling seamless variant testing capabilities and rollout of product features. | ❌ No | ❌ **NOT RELEVANT** | Feature flag/experimentation tool. Not applicable to Avro's current architecture focus on orchestration and microservices. |
| [Launchdarkly Flag Cleanup](https://github.com/github/awesome-copilot/blob/main/agents/launchdarkly-flag-cleanup.agent.md) | Safely automate feature flag cleanup workflows. Determines removal readiness and creates PRs that preserve production behavior. | ❌ No | ❌ **NOT RELEVANT** | Feature flag management. Not part of Avro's current architecture scope. |
| [Octopus Release Notes](https://github.com/github/awesome-copilot/blob/main/agents/octopus-deploy-release-notes-mcp.agent.md) | Generate release notes for a release in Octopus Deploy. | ❌ No | ❌ **NOT RELEVANT** | Octopus Deploy integration. Avro uses AWS CDK for infrastructure; not applicable. |
| [WinForms Expert](https://github.com/github/awesome-copilot/blob/main/agents/WinFormsExpert.agent.md) | Support development of .NET (OOP) WinForms Designer compatible Apps. | ❌ No | ❌ **NOT RELEVANT** | Desktop Windows Forms development. Avro is a backend microservices platform; no UI development. |

---

## Priority Installation Recommendations

### 🔴 **CRITICAL** - Install First
1. **C# Expert** - Essential for all .NET development on the platform
   - Validates SOLID principles adherence
   - Ensures async/await best practices
   - Design pattern implementation guidance
   - Performance optimization for microservices

### 🟡 **RECOMMENDED** - Install Soon
2. **Arm Migration Agent** - Future-proof infrastructure
   - Ensures code portability
   - Prepares for multi-architecture deployments

3. **JFrog Security Agent** - Security hardening
   - Package vulnerability scanning
   - Compliance verification

4. **Dynatrace Expert** - Observability enhancement (if adopting Dynatrace)
   - Production monitoring
   - Performance analysis

### 🟢 **OPTIONAL** - Install as Needed
5. **Neon Performance Analyzer** - Database optimization (if using PostgreSQL)
6. **PagerDuty Incident Responder** - Incident automation (if using PagerDuty)
7. **Stackhawk Security Onboarding** - API security testing

---

## Repository Integration Points

Once installed in `.github/agents/`, these agents will integrate with:
- **Project-specific instructions** in `.github/instructions/`
- **Design pattern guidelines** (dotnet-design-pattern-review.prompt.md)
- **Architecture standards** (architecture-collection.md)
- **Security practices** (security-collection.md)
- **Testing standards** (testing-collection.md)

---

## Next Steps

**To proceed with installation:**
1. ✅ No additional setup required - agents are file-based
2. Specify which agents you'd like to install
3. They will be downloaded to `.github/agents/` automatically
4. Access them through VS Code Chat interface or Copilot CCA

---

**Ready to install? Please confirm which agents you'd like to add to the Avro platform.**
