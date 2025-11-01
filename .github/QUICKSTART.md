# AI Pipeline Quick Start Guide

Get started with the AI-powered development pipeline in 5 minutes.

## 🎯 What You'll Do

1. Create an issue with detailed requirements
2. Add the `ai-ready` label
3. Watch AI agents build your feature
4. Review and merge the auto-generated PR

## 📝 Step-by-Step Guide

### Step 1: Create a New Issue

Go to the [Issues](../../issues) tab and click "New issue".

### Step 2: Write Your Requirements

Use this template:

```markdown
**Title:** Implement user profile service

**Description:**

### Overview
Create a user profile service that allows users to manage their profile information.

### Requirements
- [ ] CRUD operations for user profiles
- [ ] Profile picture upload to S3
- [ ] Email verification workflow
- [ ] Privacy settings management

### Technical Details
- **Architecture:** Clean Architecture with CQRS
- **Database:** PostgreSQL with EF Core
- **Storage:** AWS S3 for profile pictures
- **Events:** Publish UserProfileUpdated event

### Acceptance Criteria
- [ ] All CRUD endpoints working
- [ ] Profile pictures stored in S3
- [ ] Email verification sends notifications
- [ ] Unit tests >80% coverage
- [ ] Integration tests included
- [ ] API documented in Swagger

### Performance Requirements
- [ ] GET profile: <100ms p95
- [ ] UPDATE profile: <200ms p95
- [ ] Upload picture: <2s p95
```

### Step 3: Add the `ai-ready` Label

Click "Labels" on the right side and select `ai-ready`.

> 💡 **Tip:** You can also add `auto-merge` if you want the PR to merge automatically after passing all checks.

### Step 4: Submit the Issue

Click "Submit new issue".

## 🤖 What Happens Next

### Automatic Processing (0-5 minutes)

The AI pipeline starts automatically:

```
✅ Issue created with ai-ready label
    ↓
🤖 AI Pipeline Triggered
    ↓
📐 Architecture Agent analyzes and approves design
    ↓
┌──────────────────────────────────────────────┐
│  Parallel Execution (5-15 minutes)           │
├──────────────┬──────────────┬────────────────┤
│ Implementation│   Testing    │   DevOps       │
│    Agent      │    Agent     │    Agent       │
│               │              │                │
│ • Domain      │ • Unit tests │ • Dockerfile   │
│ • Application │ • Integration│ • CI/CD        │
│ • API         │ • E2E tests  │ • AWS config   │
└──────────────┴──────────────┴────────────────┘
    ↓
🔍 Review Agent validates code
    ↓
🎉 Pull Request Created
```

### You'll See Comments Like This:

**Initial Comment:**
```
🤖 AI Development Pipeline Started

The AI agents are now analyzing and implementing this feature.

Pipeline Steps:
1. ✅ Architecture analysis
2. ⏳ Implementation
3. ⏳ Testing
4. ⏳ Code review
5. ⏳ DevOps setup

A pull request will be created automatically when ready.
```

**Architecture Approval:**
```
✅ Architecture Analysis Complete

Implementation Strategy:
1. Implement domain models
2. Create application handlers
3. Add infrastructure components
4. Create API endpoints

Proceeding with implementation...
```

**Completion:**
```
✅ AI Development Pipeline Complete!

A pull request has been created: #42

Next Steps:
1. Review the generated code
2. Run additional tests if needed
3. Approve and merge when ready

The PR includes:
- Implementation code
- Unit and integration tests
- DevOps configuration
- Documentation updates
```

## 👀 Step 5: Review the Pull Request

Click the PR link in the comment.

### What's Included

The AI-generated PR contains:

```
src/Avro.UserProfile.Domain/
  ├── Aggregates/
  │   └── UserProfile.cs
  ├── Events/
  │   ├── UserProfileCreated.cs
  │   └── UserProfileUpdated.cs
  └── ValueObjects/
      └── ProfilePicture.cs

src/Avro.UserProfile.Application/
  ├── Commands/
  │   ├── CreateUserProfileCommand.cs
  │   └── UpdateUserProfileCommand.cs
  ├── Queries/
  │   └── GetUserProfileQuery.cs
  └── Handlers/
      ├── CreateUserProfileHandler.cs
      ├── UpdateUserProfileHandler.cs
      └── GetUserProfileQueryHandler.cs

src/Avro.UserProfile.Infrastructure/
  ├── Repositories/
  │   └── UserProfileRepository.cs
  └── Persistence/
      ├── UserProfileDbContext.cs
      └── Configurations/
          └── UserProfileConfiguration.cs

src/Avro.UserProfile.WebApi/
  ├── Controllers/
  │   └── UserProfileController.cs
  └── Program.cs

tests/Avro.UserProfile.UnitTests/
  ├── Domain/
  │   └── UserProfileTests.cs
  └── Application/
      └── CreateUserProfileHandlerTests.cs

tests/Avro.UserProfile.IntegrationTests/
  └── UserProfileIntegrationTests.cs

.github/workflows/
  └── deploy-user-profile.yml

Dockerfile
docker-compose.yml
```

### Quality Checks

The PR shows:
- ✅ Build successful
- ✅ All tests passing
- ✅ Code coverage >80%
- ✅ Security scan passed
- ✅ No vulnerabilities found

## ✅ Step 6: Merge

If everything looks good:

1. Click "Approve" if required
2. Click "Merge pull request"
3. Confirm the merge

**That's it!** Your feature is now deployed. 🎉

## 📊 Monitoring Deployment

After merge:

1. **Staging**: Automatically deployed in 2-5 minutes
2. **Production**: Manual approval required (for safety)

Check deployment status:
- GitHub Actions tab shows deployment progress
- AWS Console shows service updates
- CloudWatch shows metrics and logs

## 🔧 Advanced Usage

### Custom Agent Instructions

Add specific instructions for each agent:

```markdown
### Agent Instructions
@architect: Use event sourcing pattern for audit trail
@implementation: Use specification pattern for complex queries
@testing: Include load tests for 1000 concurrent users
@devops: Set up blue-green deployment with canary release
```

### Parallel Development

Create multiple AI-ready issues for different features:

```
Issue #101: User profile service [ai-ready]
Issue #102: Notification service [ai-ready]
Issue #103: Payment processing [ai-ready]
```

All will be processed in parallel!

### Auto-Merge

Add the `auto-merge` label for trusted scenarios:

```
Labels: ai-ready, auto-merge
```

The PR will merge automatically after all checks pass.

## 🐛 Troubleshooting

### Pipeline Fails

**Check the workflow logs:**
1. Go to Actions tab
2. Click on the failed workflow
3. Review error messages

**Common issues:**
- Missing required information in issue
- Conflicts with existing code
- Test failures

**Solution:** The AI will comment with the error. Fix and re-trigger by updating the issue.

### Code Quality Issues

**The Review Agent may request changes:**

```
❌ Code Review: Changes Required

Issues Found:
1. Missing input validation in CreateUserProfileCommand
2. No error handling in profile picture upload
3. Missing integration tests for email verification

Please address these issues.
```

**Solution:** 
- Let the AI fix it by commenting: `@ai-pipeline please fix review comments`
- Or manually fix and push to the same branch

### Need Help?

1. Check [AI Pipeline Documentation](AI_PIPELINE.md)
2. Review [Issue Templates](ISSUE_TEMPLATES.md)
3. See [Copilot Instructions](copilot-instructions.md)
4. Create an issue with `help` label

## 🎓 Learning Resources

- **Agent Documentation**: See `.github/agents/` for each agent's responsibilities
- **Code Examples**: Review previous AI-generated PRs
- **Best Practices**: Read `.github/copilot-instructions.md`
- **Architecture Patterns**: Explore `.github/instructions/`

## 💡 Tips for Success

### ✅ DO

- Be specific about requirements
- Include acceptance criteria
- Specify performance targets
- Provide context and examples
- Add agent-specific instructions

### ❌ DON'T

- Keep descriptions vague
- Mix multiple features in one issue
- Forget the `ai-ready` label
- Skip technical details

## 📈 Success Metrics

**Your AI pipeline is working well when:**

- ✅ 95%+ of PRs require no changes
- ✅ Features deployed in <30 minutes
- ✅ Zero production bugs from AI code
- ✅ >80% code coverage maintained
- ✅ All security checks passing

## 🚀 Next Steps

1. **Create your first AI-ready issue** using the template above
2. **Watch the magic happen** as AI builds your feature
3. **Review and merge** the generated code
4. **Iterate and improve** your requirements for better results

---

**Ready to get started?** [Create your first AI-ready issue →](../../issues/new)

**Questions?** See the [full documentation](AI_PIPELINE.md) or create an issue with the `help` label.
