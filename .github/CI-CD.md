# CI/CD Pipeline

This document describes the continuous integration and deployment pipeline for the Element MCP Server.

## GitHub Actions Workflow

The project uses GitHub Actions for automated build and test verification on pull requests.

### Workflow: Build and Test

**File**: `.github/workflows/build-and-test.yml`

**Triggers**:
- Pull requests to `main` or `master` branches
- Pushes to `main` or `master` branches  
- Manual workflow dispatch
- Only runs when files in `src/` or `.github/workflows/` change

### Jobs

#### 1. Build and Test (Matrix)

Runs on multiple operating systems to ensure cross-platform compatibility:
- **Ubuntu Latest** (Linux)
- **Windows Latest**
- **macOS Latest**

**Steps**:
1. Checkout code
2. Setup .NET 10 SDK
3. Restore NuGet dependencies
4. Build the project in Release configuration
5. Verify server startup (5-second test)
6. Upload server logs as artifacts

**Server Verification**:
- **Linux/macOS**: Starts server as background process and polls for "Application started" message (50 checks × 0.1s = 5 seconds)
- **Windows**: Uses PowerShell background jobs with similar polling logic

#### 2. Validate Project

Runs on Ubuntu to validate project structure and configuration:

**Validation Checks**:
- ✅ Project file exists (`src/ElementMcpServer.csproj`)
- ✅ MCP server metadata exists (`src/.mcp/server.json`)
- ✅ All data models present (Component, Foundation, Pattern, Template)
- ✅ All MCP tools present (ComponentTools, FoundationTools, PatternTools, TemplateTools, SearchTools)
- ✅ Data service exists (`src/Data/ElementDataService.cs`)

#### 3. Build Summary

Generates a summary of all job results:
- Displays status of each job
- Shows ✅ success or ❌ failure message
- Fails if any job fails

### Artifacts

The workflow uploads server logs as artifacts for debugging:
- **Artifact Name**: `server-logs-<os>`
- **Contents**: Server startup output
- **Retention**: Default GitHub Actions artifact retention

### Status Badge

The README includes a status badge showing the current build status:

```markdown
[![Build and Test](https://github.com/ShaydeNofziger/element-mcp/actions/workflows/build-and-test.yml/badge.svg)](https://github.com/ShaydeNofziger/element-mcp/actions/workflows/build-and-test.yml)
```

## Running Locally

To run the same checks locally:

### Build and Test

```bash
# Restore and build
dotnet restore src/ElementMcpServer.csproj
dotnet build src/ElementMcpServer.csproj --configuration Release --no-restore

# Test server startup (Linux/macOS)
cd src
# Start server in background
dotnet run --no-build --configuration Release > server-output.log 2>&1 &
SERVER_PID=$!

# Wait up to 5 seconds for server to start
for i in {1..50}; do
  if grep -q "Application started" server-output.log 2>/dev/null; then
    echo "✅ Server started successfully"
    kill $SERVER_PID 2>/dev/null || true
    exit 0
  fi
  sleep 0.1
done

# Server didn't start in time
echo "❌ Server failed to start"
cat server-output.log
kill $SERVER_PID 2>/dev/null || true
exit 1
```

### Validate Project Structure

```bash
# Check all required files exist
test -f src/ElementMcpServer.csproj && echo "✅ Project file exists"
test -f src/.mcp/server.json && echo "✅ MCP metadata exists"
test -f src/Models/Component.cs && echo "✅ Component model exists"
test -f src/Models/Foundation.cs && echo "✅ Foundation model exists"
test -f src/Models/Pattern.cs && echo "✅ Pattern model exists"
test -f src/Models/Template.cs && echo "✅ Template model exists"
test -f src/Tools/ComponentTools.cs && echo "✅ ComponentTools exists"
test -f src/Tools/FoundationTools.cs && echo "✅ FoundationTools exists"
test -f src/Tools/PatternTools.cs && echo "✅ PatternTools exists"
test -f src/Tools/TemplateTools.cs && echo "✅ TemplateTools exists"
test -f src/Tools/SearchTools.cs && echo "✅ SearchTools exists"
test -f src/Data/ElementDataService.cs && echo "✅ Data service exists"
```

## Pull Request Process

1. **Create PR**: Open a pull request to `main` or `master`
2. **Automatic Checks**: GitHub Actions automatically runs the workflow
3. **Review Results**: Check the workflow status and any failures
4. **Fix Issues**: If checks fail, review logs and fix issues
5. **Merge**: Once all checks pass, the PR can be merged

## Troubleshooting

### Build Failures

- Check the build logs in the GitHub Actions tab
- Ensure all dependencies are properly specified in `.csproj`
- Verify .NET 10 SDK is compatible with your code

### Server Startup Failures

- Review the uploaded `server-logs` artifact
- Check for missing dependencies or configuration issues
- Ensure `Program.cs` properly initializes the MCP server

### Validation Failures

- Verify all required files are committed
- Check file paths match expected locations
- Ensure naming conventions are followed

## Future Enhancements

Potential CI/CD improvements:

- [ ] Add unit tests and test coverage reporting
- [ ] Add code quality checks (linting, formatting)
- [ ] Add security scanning (dependency vulnerabilities)
- [ ] Add automated versioning and releases
- [ ] Add NuGet package publishing on tagged releases
- [ ] Add performance benchmarks
- [ ] Add integration tests with sample MCP clients

## References

- [GitHub Actions Documentation](https://docs.github.com/en/actions)
- [.NET GitHub Actions](https://github.com/actions/setup-dotnet)
- [Workflow Syntax](https://docs.github.com/en/actions/using-workflows/workflow-syntax-for-github-actions)
