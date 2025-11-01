# Contributing to AVRO

Thank you for your interest in contributing to AVRO! This guide will help you get started.

## Code of Conduct

We are committed to providing a welcoming and inclusive environment. Please be respectful and professional.

## Getting Started

### Prerequisites
- .NET 8.0 SDK or later
- Node.js 20+ and pnpm
- Git
- Your favorite IDE (VS Code, Visual Studio, Rider)

### Setup
1. Fork the repository
2. Clone your fork: `git clone https://github.com/your-username/avro.git`
3. Navigate to the project: `cd avro`
4. Install backend dependencies: `cd backend && dotnet restore`
5. Install frontend dependencies: `cd frontend && pnpm install`

## Development Workflow

### 1. Create a Branch
```bash
git checkout -b feature/your-feature-name
# or
git checkout -b bugfix/your-bugfix-name
```

### 2. Make Changes
- Follow coding standards
- Write tests
- Update documentation
- Commit frequently with clear messages

### 3. Run Tests
```bash
# Backend
cd backend
dotnet test

# Frontend
cd frontend
pnpm test
```

### 4. Submit Pull Request
1. Push to your fork
2. Create PR against `main` branch
3. Fill out PR template
4. Wait for review

## Coding Standards

### Backend (C#/.NET)
- Follow Clean Architecture principles
- Use PascalCase for public members
- Use _camelCase for private fields
- Add XML documentation comments
- Use async/await for I/O operations
- Follow SOLID principles

Example:
```csharp
/// <summary>
/// Service for managing tasks
/// </summary>
public class TaskService : ITaskService
{
    private readonly ILogger<TaskService> _logger;

    public TaskService(ILogger<TaskService> logger)
    {
        _logger = logger;
    }

    public async Task<Result<Task>> GetTaskAsync(string taskId)
    {
        // Implementation
    }
}
```

### Frontend (TypeScript/React)
- Use functional components with hooks
- Use TypeScript for all files
- Follow component structure conventions
- Use Tailwind CSS for styling
- Add proper type definitions

Example:
```typescript
interface TaskCardProps {
  task: Task
  onComplete: (id: string) => void
}

export function TaskCard({ task, onComplete }: TaskCardProps) {
  return (
    <Card>
      <CardHeader>
        <CardTitle>{task.title}</CardTitle>
      </CardHeader>
      <CardContent>
        {task.description}
      </CardContent>
    </Card>
  )
}
```

### General
- Write clear, self-documenting code
- Keep functions small and focused
- Avoid magic numbers and strings
- Handle errors appropriately
- Use meaningful variable names

## Commit Messages

Use Conventional Commits format:
```
<type>(<scope>): <description>

[optional body]

[optional footer]
```

Types:
- `feat`: New feature
- `fix`: Bug fix
- `docs`: Documentation
- `style`: Formatting
- `refactor`: Code restructuring
- `test`: Adding tests
- `chore`: Maintenance

Examples:
```
feat(tasks): add filtering by priority
fix(api): resolve null reference in task service
docs(readme): update installation instructions
```

## Testing

### Backend Tests
- Unit tests for business logic
- Integration tests for services
- Use xUnit and FluentAssertions
- Aim for 80%+ coverage

```csharp
[Fact]
public async Task GetTaskAsync_WithValidId_ReturnsTask()
{
    // Arrange
    var service = new TaskService(_logger);
    var taskId = "TASK-0001";

    // Act
    var result = await service.GetTaskAsync(taskId);

    // Assert
    result.Should().NotBeNull();
    result.Id.Should().Be(taskId);
}
```

### Frontend Tests
- Component tests with React Testing Library
- Use Jest for test runner
- Test user interactions
- Test edge cases

```typescript
describe('TaskCard', () => {
  it('renders task title', () => {
    const task = { id: '1', title: 'Test Task' }
    render(<TaskCard task={task} />)
    expect(screen.getByText('Test Task')).toBeInTheDocument()
  })
})
```

## Documentation

### Code Documentation
- Add XML docs for C# public APIs
- Add JSDoc for TypeScript public APIs
- Document complex algorithms
- Explain why, not just what

### Project Documentation
- Update README.md if needed
- Add docs to /docs folder
- Include examples
- Keep docs current

## AI Agent Contributions

When working with AI agents:
1. Update task registry with new tasks
2. Update project registry for new projects
3. Maintain context files
4. Follow autonomy rules
5. Request review for supervised changes

## Pull Request Process

### PR Template
Fill out all sections:
- Description of changes
- Related issues
- Type of change
- Testing done
- Checklist completion

### Review Process
1. Automated checks run
2. Code review by maintainers
3. Address feedback
4. Approval required
5. Merge to main

### PR Guidelines
- Keep PRs focused and small
- Link related issues
- Provide clear description
- Include tests
- Update documentation

## Project Structure

### Adding a New Backend Project
1. Create project directory under appropriate category
2. Add .csproj file
3. Add to solution file
4. Update project registry
5. Add to dependency graph

### Adding a New Frontend Package
1. Create package directory
2. Add package.json
3. Add to workspace
4. Update references
5. Update project registry

### Adding a New Task
1. Add to tasks/registry.json
2. Link to project
3. Define dependencies
4. Set AI metadata
5. Assign priority

## Issue Reporting

### Bug Reports
Include:
- Clear description
- Steps to reproduce
- Expected behavior
- Actual behavior
- Environment details
- Screenshots if applicable

### Feature Requests
Include:
- Problem description
- Proposed solution
- Alternatives considered
- Additional context

## Community

### Getting Help
- Check documentation
- Search existing issues
- Ask in discussions
- Contact maintainers

### Stay Updated
- Watch repository
- Follow releases
- Read changelog

## License

By contributing, you agree that your contributions will be licensed under the MIT License.

## Questions?

Feel free to open an issue or discussion if you have questions!
