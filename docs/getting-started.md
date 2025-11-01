# Getting Started with AVRO

This guide will help you get started with the AVRO platform quickly.

## Quick Start

### Prerequisites

Ensure you have the following installed:
- .NET 8.0 SDK or later ([Download](https://dotnet.microsoft.com/download))
- Node.js 20+ and pnpm ([Download Node.js](https://nodejs.org/))
- Git
- Docker (optional, for containerized deployment)

### Installation

1. **Clone the repository**
   ```bash
   git clone https://github.com/ihorhc/avro.git
   cd avro
   ```

2. **Install backend dependencies**
   ```bash
   cd backend
   dotnet restore
   ```

3. **Install frontend dependencies**
   ```bash
   cd ../frontend
   pnpm install
   ```

## Running the Platform

### Backend (Development)

```bash
cd backend
dotnet build
dotnet run --project core/Platform.Core
```

The backend will start on `http://localhost:5000` (default).

### Frontend (Development)

```bash
cd frontend
pnpm dev
```

The frontend will start on `http://localhost:3000` (default).

### Using Docker

```bash
# From the root directory
docker-compose up
```

This will start both backend and frontend services.

## Project Structure

```
avro/
├── backend/              # .NET 8 backend
│   ├── core/            # Core services
│   ├── services/        # Microservices
│   └── shared/          # Shared libraries
├── frontend/            # TypeScript/React frontend
│   ├── apps/           # Applications
│   └── packages/       # Shared packages
├── ai/                 # AI configurations
│   ├── agents/         # Agent definitions
│   ├── context/        # Context files
│   └── workflows/      # Workflow templates
├── projects/           # Project registry
├── tasks/              # Task registry
└── docs/               # Documentation
```

## Key Concepts

### Task Registry
The task registry (`tasks/registry.json`) manages 5K+ structured tasks:
- View all tasks
- Filter by status, category, priority
- Track dependencies
- View AI metadata

### Project Registry
The project registry (`projects/registry.json`) tracks 100+ projects:
- Project dependencies
- Build configurations
- AI context metadata
- Autonomy levels

### AI Agents
AI agents automate common workflows:
- **Issue-to-PR**: Convert issues to pull requests
- **Code Review**: Automated code reviews
- **Task Breakdown**: Decompose large tasks
- **Dependency Updates**: Manage dependencies

## Common Tasks

### Adding a New Task

1. Open `tasks/registry.json`
2. Add a new task entry:
   ```json
   {
     "id": "TASK-XXXX",
     "title": "Your task title",
     "description": "Task description",
     "category": "backend",
     "priority": "high",
     "status": "pending",
     "assignedProject": "backend.core.platform",
     "dependencies": [],
     "aiMetadata": {
       "complexity": "medium",
       "estimatedEffort": "4h",
       "requiresHumanReview": false,
       "contextFiles": []
     }
   }
   ```

### Creating a New Project

1. Create project directory:
   ```bash
   mkdir -p backend/services/MyService
   ```

2. Add project file (`.csproj` for .NET or `package.json` for Node)

3. Update `projects/registry.json`:
   ```json
   {
     "id": "backend.services.myservice",
     "name": "My Service",
     "type": "backend",
     "language": "csharp",
     "path": "backend/services/MyService",
     "status": "active",
     "dependencies": [],
     "aiContext": {
       "autonomyLevel": "supervised"
     }
   }
   ```

### Using AI Workflows

#### Trigger Issue-to-PR Workflow
```bash
# In a GitHub issue, add label or comment:
/ai implement
```

#### Trigger Task Breakdown
```bash
# In a GitHub issue, add label:
ai:breakdown
```

#### Trigger Code Review
Opens automatically on pull request creation.

## Development Workflow

### Backend Development

1. **Create a new feature branch**
   ```bash
   git checkout -b feature/my-feature
   ```

2. **Make changes**
   - Edit code in `backend/`
   - Follow Clean Architecture patterns
   - Add unit tests

3. **Build and test**
   ```bash
   cd backend
   dotnet build
   dotnet test
   ```

4. **Commit and push**
   ```bash
   git add .
   git commit -m "feat: add new feature"
   git push origin feature/my-feature
   ```

### Frontend Development

1. **Create a new feature branch**
   ```bash
   git checkout -b feature/my-feature
   ```

2. **Make changes**
   - Edit code in `frontend/`
   - Use shadcn/ui components
   - Add component tests

3. **Build and test**
   ```bash
   cd frontend
   pnpm build
   pnpm test
   ```

4. **Commit and push**
   ```bash
   git add .
   git commit -m "feat: add new feature"
   git push origin feature/my-feature
   ```

## Testing

### Backend Tests
```bash
cd backend
dotnet test --verbosity normal
```

### Frontend Tests
```bash
cd frontend
pnpm test
```

### All Tests
```bash
# Backend
cd backend && dotnet test

# Frontend
cd frontend && pnpm test
```

## Building for Production

### Backend
```bash
cd backend
dotnet build --configuration Release
dotnet publish --configuration Release --output ./publish
```

### Frontend
```bash
cd frontend
pnpm build
```

### Docker
```bash
docker-compose build
docker-compose up -d
```

## Configuration

### Backend Configuration
Edit `backend/core/Platform.Core/appsettings.json` or use environment variables.

### Frontend Configuration
Edit `frontend/apps/*/next.config.js` or use `.env.local` files.

## Troubleshooting

### Backend won't build
- Ensure .NET 8 SDK is installed: `dotnet --version`
- Restore packages: `dotnet restore`
- Check project references in `.csproj` files

### Frontend won't build
- Ensure Node.js 20+ is installed: `node --version`
- Install pnpm: `npm install -g pnpm`
- Clear cache: `pnpm store prune`

### Docker issues
- Ensure Docker is running
- Check Docker version: `docker --version`
- Rebuild images: `docker-compose build --no-cache`

## Next Steps

- Read the [Architecture Guide](architecture.md)
- Learn about [AI Agents](ai-agents.md)
- Review [Contributing Guidelines](../CONTRIBUTING.md)
- Explore the task registry
- Try the AI workflows

## Getting Help

- Check documentation in `/docs`
- Review examples in the codebase
- Open an issue on GitHub
- Join discussions

## Resources

- [.NET Documentation](https://docs.microsoft.com/dotnet/)
- [React Documentation](https://react.dev/)
- [Next.js Documentation](https://nextjs.org/docs)
- [shadcn/ui Documentation](https://ui.shadcn.com/)
- [Turborepo Documentation](https://turbo.build/repo/docs)
