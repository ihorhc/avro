# CodeQL Security Scanning Workflow

## Overview

The CodeQL workflow (`codeql.yml`) provides automated security analysis for the
Avro platform codebase. It uses GitHub's CodeQL engine to detect security
vulnerabilities, coding errors, and potential issues across multiple languages.

## Workflow Architecture

### Trigger Events

The workflow runs automatically on:

1. **Push to main branch** - Scans code changes merged to the main branch
2. **Pull requests to main** - Validates security before merging
3. **Scheduled scans** - Weekly scans every Friday at 3:32 AM UTC (`32 3 * * 5`)

### Analysis Matrix

The workflow analyzes code in multiple languages simultaneously:

| Language | Build Mode | Purpose |
|----------|-----------|---------|
| C# | autobuild | Analyzes .NET backend code for security issues |
| JavaScript/TypeScript | none | Scans frontend and Node.js code without building |

## Runtime Requirements

### .NET SDK Version

**Current Standard**: .NET 10.0.x

The workflow uses .NET 10 to align with the repository's technology standard as
documented in the project README. The setup step automatically installs the
latest .NET 10 SDK patch version.

```yaml
- name: set-up-dotnet
  uses: actions/setup-dotnet@v4
  with:
    dotnet-version: '10.0.x'
```

**Compatibility Notes**:
- The repository's `global.json` specifies SDK version 9.0.0 with
  `"rollForward": "latestFeature"` policy, which allows the SDK to
  automatically roll forward to .NET 10
- All C# projects currently target `net9.0` framework and will build
  successfully with .NET 10 SDK
- Future migrations to `net10.0` target framework can be done incrementally

### Required Permissions

The CodeQL job requires the following GitHub Actions permissions:

```yaml
permissions:
  security-events: write  # Upload CodeQL results to GitHub Security
  packages: read          # Access package registries for dependencies
  actions: read           # Read workflow run data
  contents: read          # Checkout repository code
```

**Note**: These permissions are explicitly scoped to the minimum required for
security scanning operations.

## Build Process

### C# Autobuild

For C# analysis, CodeQL uses the `autobuild` mode which:

1. Detects the build system (MSBuild for .NET projects)
2. Runs `dotnet restore` to restore NuGet packages
3. Executes `dotnet build` to compile the solution
4. Captures compilation data for analysis

The autobuild process works automatically with the Avro solution structure:
- Restores all projects in dependency order
- Builds the complete solution (`Avro.sln`)
- Handles multi-project dependencies correctly

**Manual Build Override**: If autobuild fails, you can replace it with explicit
commands:

```yaml
- name: build-csharp
  run: |
    dotnet restore
    dotnet build --no-restore --configuration Release
```

### JavaScript/TypeScript Analysis

JavaScript and TypeScript use `build-mode: none` because:
- Static analysis doesn't require compilation
- CodeQL analyzes source code directly
- No build artifacts are needed for security scanning

## Workflow Steps

### 1. Repository Checkout

```yaml
- name: checkout-repository
  uses: actions/checkout@v4
```

Checks out the repository code for analysis.

### 2. .NET Setup

```yaml
- name: set-up-dotnet
  uses: actions/setup-dotnet@v4
  with:
    dotnet-version: '10.0.x'
```

Installs the .NET 10 SDK for building C# projects.

### 3. CodeQL Initialization

```yaml
- name: initialize-codeql
  uses: github/codeql-action/init@v4
  with:
    languages: ${{ matrix.language }}
    build-mode: ${{ matrix.build-mode }}
```

Initializes CodeQL for the target language and configures the build mode.

### 4. CodeQL Analysis

```yaml
- name: perform-codeql-analysis
  uses: github/codeql-action/analyze@v4
  with:
    category: "/language:${{ matrix.language }}"
  continue-on-error: true
```

Performs the security analysis and uploads results to GitHub Security tab.

**Note**: `continue-on-error: true` allows the workflow to complete even if
vulnerabilities are found, ensuring results are always uploaded.

## Security Results

### Viewing Results

After each workflow run:

1. Navigate to the **Security** tab in the GitHub repository
2. Click **Code scanning alerts** in the left sidebar
3. Review any detected vulnerabilities or issues
4. Filter by severity, status, or language

### Alert Categories

CodeQL detects various security issues including:

- **SQL Injection** - Unsafe database queries
- **Cross-Site Scripting (XSS)** - Unsafe HTML output
- **Path Traversal** - File system access vulnerabilities
- **Authentication Issues** - Weak or missing authentication
- **Cryptography Problems** - Weak encryption or hashing
- **Resource Management** - Memory leaks, file handle leaks
- **Code Quality** - Potential bugs and anti-patterns

### Severity Levels

| Severity | Description | Action Required |
|----------|-------------|----------------|
| Critical | Exploitable security vulnerability | Immediate fix required |
| High | Serious security issue | Fix before next release |
| Medium | Potential security concern | Review and address |
| Low | Code quality or minor issue | Consider addressing |

## Troubleshooting

### Common Issues

#### 1. Autobuild Failure

**Symptoms**: C# analysis fails during build step

**Solutions**:
- Verify all NuGet packages are available and restore successfully
- Check that .NET 10 SDK is compatible with project target frameworks
- Review build logs for specific compilation errors
- Consider adding explicit build commands instead of autobuild

**Debug Steps**:
```bash
# Test locally with .NET 10
dotnet restore
dotnet build --configuration Release
```

#### 2. Missing Dependencies

**Symptoms**: Restore fails with package not found errors

**Solutions**:
- Check `NuGet.config` for correct package sources
- Verify package versions are available on NuGet.org
- Ensure no private packages require authentication

#### 3. Permission Errors

**Symptoms**: Unable to upload results or access repository

**Solutions**:
- Verify workflow has required permissions (see Required Permissions section)
- Check repository security settings allow CodeQL uploads
- Ensure `GITHUB_TOKEN` has necessary scopes

#### 4. .NET Version Mismatch

**Symptoms**: SDK version errors during setup or build

**Solutions**:
- Verify `global.json` allows rollForward to .NET 10
- Check project target frameworks are compatible
- Review setup-dotnet logs for version installation issues

### Validation

Before deploying changes to the CodeQL workflow:

```bash
# Validate YAML syntax
yamllint .github/workflows/codeql.yml

# Validate GitHub Actions workflow
actionlint .github/workflows/codeql.yml
```

## Maintenance

### Updating .NET Version

When upgrading to a new .NET version:

1. Update `dotnet-version` in the workflow
2. Verify `global.json` rollForward policy allows the new version
3. Test build locally with the new SDK
4. Update this documentation with the new version

### Adding Languages

To add analysis for additional languages:

1. Add a new matrix entry:
   ```yaml
   - language: "python"
     build-mode: "none"
   ```
2. Configure any required setup steps for that language
3. Test the workflow with a sample file in that language

### Query Customization

CodeQL supports custom query packs for organization-specific rules:

1. Create a CodeQL query pack in `.github/codeql/`
2. Reference it in the init step:
   ```yaml
   queries: ./.github/codeql/custom-queries.qls
   ```

## Performance Optimization

### Reducing Scan Time

- **Incremental Analysis**: CodeQL automatically analyzes only changed code in
  pull requests
- **Parallel Execution**: Language analysis runs in parallel via matrix strategy
- **Scheduled Scans**: Weekly full scans reduce load on push/PR events

### Resource Usage

Typical workflow execution times:
- **C# Analysis**: 3-5 minutes (depends on solution size)
- **JavaScript/TypeScript**: 1-2 minutes
- **Total Runtime**: ~5-7 minutes for parallel execution

## Best Practices

### For Developers

1. **Review alerts promptly** - Address security issues before merging PRs
2. **Don't disable CodeQL** - Security scanning should run on all changes
3. **Fix root causes** - Don't just suppress alerts without understanding them
4. **Test locally** - Run builds with the same .NET version used in CI

### For Maintainers

1. **Monitor weekly scans** - Review scheduled scan results regularly
2. **Keep queries updated** - CodeQL action auto-updates query packs
3. **Document exceptions** - If suppressing alerts, document why
4. **Track metrics** - Monitor trend of security issues over time

## Integration with Development Workflow

### Pull Request Workflow

1. Developer creates PR
2. CodeQL workflow runs automatically
3. Security results appear as PR checks
4. Review and fix any detected issues
5. Re-run CodeQL after fixes
6. Merge when scan passes

### Security Monitoring

- **Weekly Reports**: Scheduled scans provide regular security health checks
- **Trend Analysis**: Compare scan results over time in Security tab
- **Alert Management**: Triage and assign security alerts to team members

## References

- [CodeQL Documentation](https://codeql.github.com/docs/)
- [CodeQL for C#](https://codeql.github.com/docs/codeql-language-guides/codeql-for-csharp/)
- [CodeQL for JavaScript](https://codeql.github.com/docs/codeql-language-guides/codeql-for-javascript/)
- [GitHub Code Scanning](https://docs.github.com/en/code-security/code-scanning)
- [setup-dotnet Action](https://github.com/actions/setup-dotnet)
