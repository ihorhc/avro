# docs/.agents/tech-writer/glossary.md

## Avro Platform Terms

### Core Platform Components
- **Avro Platform**: AI-first Software Development Lifecycle (SDLC) automation platform
- **Avro CLI**: Command-line interface for platform operations and development workflows
- **Avro Services**: Core microservices architecture (auth, billing, workflow, notifications)
- **Avro Agents**: AI-powered automation components (architect, tech-writer, code-reviewer)
- **Avro Infra**: Infrastructure as Code components (Terraform, Kubernetes manifests, AWS resources)
- **Avro Web**: Frontend applications and web interfaces
- **Avro SDK**: Software Development Kits for various programming languages

### Service Categories
- **avro.cli**: Command-line tool and utilities
- **avro.svc.***: Backend microservices
  - `avro.svc.auth`: Authentication and authorization service
  - `avro.svc.billing`: Billing and subscription management
  - `avro.svc.workflow`: CI/CD pipeline orchestration
  - `avro.svc.notifications`: Email, SMS, and webhook notifications
  - `avro.svc.storage`: File and document storage service
- **avro.web.***: Frontend applications
  - `avro.web.dashboard`: Main platform dashboard
  - `avro.web.api`: Public API gateway
  - `avro.web.docs`: Documentation site
- **avro.infra.***: Infrastructure components
  - `avro.infra.terraform`: AWS infrastructure definitions
  - `avro.infra.k8s`: Kubernetes deployment manifests
  - `avro.infra.monitoring`: Observability and alerting setup
- **avro.agents.***: AI agent implementations
  - `avro.agents.architect`: Design validation and consistency
  - `avro.agents.techwriter`: Documentation generation
  - `avro.agents.reviewer`: Code review automation

### Development Concepts
- **SDLC Platform**: Software Development Lifecycle automation system
- **CI/CD Pipeline**: Continuous Integration/Continuous Deployment workflows
- **Agent Workflow**: AI-powered automation sequences
- **Feature Flag**: Runtime configuration for gradual feature rollout
- **Deployment Strategy**: Blue-green, canary, or rolling deployment approaches
- **Service Mesh**: Inter-service communication and observability layer
- **GitOps**: Git-based infrastructure and application management

### API and Integration Terms
- **API Gateway**: Centralized entry point for all external API requests
- **Webhook**: HTTP callbacks for real-time event notifications
- **Rate Limiting**: Request throttling to prevent abuse
- **Circuit Breaker**: Fault tolerance pattern for service failures
- **Health Check**: Endpoint monitoring for service availability
- **Idempotency**: Safe retry behavior for API operations
- **Pagination**: Large dataset splitting across multiple requests
- **Versioning**: API compatibility management across releases

### Infrastructure Terms
- **Multi-AZ**: AWS Multi-Availability Zone deployment for high availability
- **Auto Scaling**: Dynamic resource adjustment based on demand
- **Load Balancer**: Traffic distribution across multiple service instances
- **Container Registry**: Docker image storage and distribution
- **Secret Management**: Secure storage and rotation of sensitive configuration
- **Backup Strategy**: Data protection and disaster recovery procedures
- **Monitoring Stack**: Observability tools (Datadog, CloudWatch, Prometheus)

### Documentation Types
- **API Reference**: Technical specification for endpoints and schemas
- **User Guide**: Step-by-step instructions for end users
- **Developer Guide**: Integration instructions for developers
- **Runbook**: Operational procedures for system administration
- **Architecture Decision Record (ADR)**: Documentation of significant design decisions
- **Migration Guide**: Instructions for upgrading between versions
- **Troubleshooting Guide**: Common issues and resolution steps
- **Release Notes**: Summary of changes and improvements in each version

### Quality and Testing
- **Unit Test**: Individual component testing
- **Integration Test**: Multi-component interaction testing
- **End-to-End Test**: Complete user workflow testing
- **Performance Test**: Load and stress testing
- **Security Scan**: Vulnerability assessment and compliance check
- **Code Coverage**: Percentage of code executed by tests
- **SLA**: Service Level Agreement for availability and performance
- **SLO**: Service Level Objective for internal quality targets

### Security and Compliance
- **Multi-Factor Authentication (MFA)**: Enhanced login security
- **Role-Based Access Control (RBAC)**: Permission management system
- **Audit Log**: Security and compliance activity tracking
- **Encryption at Rest**: Data protection in storage
- **Encryption in Transit**: Data protection during transmission
- **CVE**: Common Vulnerabilities and Exposures identifier
- **Penetration Testing**: Security assessment by simulated attacks
- **Compliance Framework**: Regulatory requirement adherence (SOC2, GDPR)
