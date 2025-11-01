# Deployment Guide

This guide covers deploying the AVRO platform to various environments.

## Deployment Options

### 1. Docker Compose (Recommended for Development)

The simplest way to deploy the platform locally or on a single server.

#### Prerequisites
- Docker 20.10+
- Docker Compose 2.0+

#### Steps

1. **Build images**
   ```bash
   docker-compose build
   ```

2. **Start services**
   ```bash
   docker-compose up -d
   ```

3. **View logs**
   ```bash
   docker-compose logs -f
   ```

4. **Stop services**
   ```bash
   docker-compose down
   ```

#### Environment Variables

Create a `.env` file in the root directory:

```env
# Backend
ASPNETCORE_ENVIRONMENT=Production
ASPNETCORE_URLS=http://+:5000
DATABASE_CONNECTION_STRING=your_connection_string

# Frontend
NODE_ENV=production
NEXT_PUBLIC_API_URL=http://backend:5000
```

### 2. Kubernetes

For production deployments at scale.

#### Prerequisites
- Kubernetes cluster (1.25+)
- kubectl configured
- Helm 3+

#### Deployment

1. **Create namespace**
   ```bash
   kubectl create namespace avro
   ```

2. **Deploy backend**
   ```bash
   kubectl apply -f k8s/backend/
   ```

3. **Deploy frontend**
   ```bash
   kubectl apply -f k8s/frontend/
   ```

4. **Verify deployment**
   ```bash
   kubectl get pods -n avro
   ```

#### Configuration

Create ConfigMaps and Secrets:

```bash
# Backend configuration
kubectl create configmap backend-config \
  --from-file=appsettings.json \
  -n avro

# Database credentials
kubectl create secret generic db-credentials \
  --from-literal=connection-string='your_connection_string' \
  -n avro
```

### 3. Cloud Platforms

#### Azure

**Azure App Service + Azure SQL**

1. **Create resources**
   ```bash
   az group create --name avro-rg --location eastus
   az appservice plan create --name avro-plan --resource-group avro-rg --sku B1
   az webapp create --name avro-backend --plan avro-plan --resource-group avro-rg
   az staticwebapp create --name avro-frontend --resource-group avro-rg
   ```

2. **Configure deployment**
   - Set up GitHub Actions for CI/CD
   - Configure app settings in Azure Portal
   - Set connection strings

3. **Deploy**
   ```bash
   az webapp deployment source config-zip \
     --resource-group avro-rg \
     --name avro-backend \
     --src backend.zip
   ```

#### AWS

**ECS + RDS**

1. **Create ECR repositories**
   ```bash
   aws ecr create-repository --repository-name avro-backend
   aws ecr create-repository --repository-name avro-frontend
   ```

2. **Push images**
   ```bash
   docker build -t avro-backend ./backend
   docker tag avro-backend:latest {account}.dkr.ecr.{region}.amazonaws.com/avro-backend:latest
   docker push {account}.dkr.ecr.{region}.amazonaws.com/avro-backend:latest
   ```

3. **Create ECS cluster and services**
   ```bash
   aws ecs create-cluster --cluster-name avro-cluster
   aws ecs create-service --cluster avro-cluster --service-name backend --task-definition avro-backend
   ```

#### Google Cloud Platform

**Cloud Run + Cloud SQL**

1. **Build and push containers**
   ```bash
   gcloud builds submit --tag gcr.io/{project-id}/avro-backend ./backend
   gcloud builds submit --tag gcr.io/{project-id}/avro-frontend ./frontend
   ```

2. **Deploy to Cloud Run**
   ```bash
   gcloud run deploy avro-backend \
     --image gcr.io/{project-id}/avro-backend \
     --platform managed \
     --region us-central1
   
   gcloud run deploy avro-frontend \
     --image gcr.io/{project-id}/avro-frontend \
     --platform managed \
     --region us-central1
   ```

## Database Setup

### PostgreSQL (Recommended)

1. **Create database**
   ```sql
   CREATE DATABASE avro;
   CREATE USER avro_user WITH PASSWORD 'secure_password';
   GRANT ALL PRIVILEGES ON DATABASE avro TO avro_user;
   ```

2. **Run migrations**
   ```bash
   cd backend
   dotnet ef database update
   ```

### SQL Server

1. **Create database**
   ```sql
   CREATE DATABASE AVRO;
   CREATE LOGIN avro_user WITH PASSWORD = 'SecurePassword123!';
   USE AVRO;
   CREATE USER avro_user FOR LOGIN avro_user;
   ALTER ROLE db_owner ADD MEMBER avro_user;
   ```

2. **Configure connection string**
   ```json
   {
     "ConnectionStrings": {
       "DefaultConnection": "Server=localhost;Database=AVRO;User Id=avro_user;Password=SecurePassword123!;"
     }
   }
   ```

## CI/CD Pipeline

### GitHub Actions

The repository includes a CI/CD workflow at `.github/workflows/ci.yml`.

#### Enable Deployment

1. **Add secrets to GitHub repository**
   - `DOCKER_USERNAME`
   - `DOCKER_PASSWORD`
   - `AZURE_CREDENTIALS` (for Azure)
   - `AWS_ACCESS_KEY_ID` (for AWS)
   - `GCP_SA_KEY` (for GCP)

2. **Update workflow**
   - Uncomment deployment steps in `ci.yml`
   - Configure target environment

3. **Trigger deployment**
   - Push to `main` branch
   - Create a release tag

### GitLab CI

Create `.gitlab-ci.yml`:

```yaml
stages:
  - build
  - test
  - deploy

build-backend:
  stage: build
  script:
    - cd backend
    - dotnet build

test-backend:
  stage: test
  script:
    - cd backend
    - dotnet test

deploy-production:
  stage: deploy
  script:
    - docker-compose build
    - docker-compose up -d
  only:
    - main
```

## Environment Configuration

### Development
```env
ASPNETCORE_ENVIRONMENT=Development
LOG_LEVEL=Debug
ENABLE_SWAGGER=true
```

### Staging
```env
ASPNETCORE_ENVIRONMENT=Staging
LOG_LEVEL=Information
ENABLE_SWAGGER=true
```

### Production
```env
ASPNETCORE_ENVIRONMENT=Production
LOG_LEVEL=Warning
ENABLE_SWAGGER=false
USE_HTTPS=true
ENFORCE_HTTPS=true
```

## Security Considerations

### SSL/TLS

1. **Obtain certificates**
   - Let's Encrypt (free)
   - Commercial CA

2. **Configure HTTPS**
   ```bash
   # Backend (Kestrel)
   dotnet dev-certs https --trust
   
   # Frontend (Next.js)
   # Use reverse proxy (nginx/Caddy) with SSL
   ```

### Secrets Management

#### Azure Key Vault
```bash
az keyvault create --name avro-vault --resource-group avro-rg
az keyvault secret set --vault-name avro-vault --name DbPassword --value "SecurePassword123!"
```

#### AWS Secrets Manager
```bash
aws secretsmanager create-secret \
  --name avro/db-password \
  --secret-string "SecurePassword123!"
```

#### HashiCorp Vault
```bash
vault kv put secret/avro/db password="SecurePassword123!"
```

### Firewall Rules

Configure network security:
- Allow HTTPS (443) from internet
- Allow HTTP (80) for redirect
- Restrict database access to backend only
- Block direct access to internal services

## Monitoring

### Application Insights (Azure)

```csharp
services.AddApplicationInsightsTelemetry(Configuration["ApplicationInsights:InstrumentationKey"]);
```

### CloudWatch (AWS)

```bash
aws logs create-log-group --log-group-name /avro/backend
```

### Cloud Logging (GCP)

Automatically enabled for Cloud Run deployments.

### Custom Monitoring

Install monitoring stack:
```bash
# Prometheus + Grafana
docker-compose -f docker-compose.monitoring.yml up -d
```

## Scaling

### Horizontal Scaling

**Kubernetes:**
```bash
kubectl scale deployment backend --replicas=3 -n avro
```

**Docker Swarm:**
```bash
docker service scale avro_backend=3
```

### Vertical Scaling

Update resource limits in deployment configuration:
```yaml
resources:
  limits:
    cpu: "2000m"
    memory: "4Gi"
  requests:
    cpu: "1000m"
    memory: "2Gi"
```

### Auto-scaling

**Kubernetes HPA:**
```yaml
apiVersion: autoscaling/v2
kind: HorizontalPodAutoscaler
metadata:
  name: backend-hpa
spec:
  scaleTargetRef:
    apiVersion: apps/v1
    kind: Deployment
    name: backend
  minReplicas: 2
  maxReplicas: 10
  metrics:
  - type: Resource
    resource:
      name: cpu
      target:
        type: Utilization
        averageUtilization: 70
```

## Backup and Recovery

### Database Backups

**PostgreSQL:**
```bash
pg_dump -U avro_user avro > backup_$(date +%Y%m%d).sql
```

**Automated backups:**
```bash
# Add to crontab
0 2 * * * /usr/bin/pg_dump -U avro_user avro > /backups/avro_$(date +\%Y\%m\%d).sql
```

### Disaster Recovery

1. Document recovery procedures
2. Test recovery process regularly
3. Store backups in multiple locations
4. Maintain configuration backups

## Health Checks

### Backend
```csharp
app.MapHealthChecks("/health");
```

Access at: `http://backend:5000/health`

### Frontend
Create `pages/api/health.ts`:
```typescript
export default function handler(req, res) {
  res.status(200).json({ status: 'ok' })
}
```

Access at: `http://frontend:3000/api/health`

## Troubleshooting

### Backend Issues

**Container won't start:**
```bash
docker logs avro-backend
dotnet --info
```

**Database connection:**
```bash
# Test connection
docker exec -it avro-backend dotnet ef database update --dry-run
```

### Frontend Issues

**Build failures:**
```bash
docker logs avro-frontend
pnpm cache clean
```

**Module not found:**
```bash
pnpm install --force
```

## Performance Optimization

### Backend
- Enable response compression
- Use output caching
- Configure connection pooling
- Enable async operations

### Frontend
- Enable static generation
- Optimize images
- Use code splitting
- Enable CDN

## Rollback Procedures

### Docker
```bash
# Tag and push before deployment
docker tag avro-backend:latest avro-backend:v1.0.0
docker tag avro-backend:latest avro-backend:rollback

# Rollback
docker-compose down
docker tag avro-backend:rollback avro-backend:latest
docker-compose up -d
```

### Kubernetes
```bash
# Rollback to previous revision
kubectl rollout undo deployment/backend -n avro

# Rollback to specific revision
kubectl rollout undo deployment/backend --to-revision=2 -n avro
```

## Support

For deployment issues:
- Check documentation
- Review logs
- Open GitHub issue
- Contact platform team
