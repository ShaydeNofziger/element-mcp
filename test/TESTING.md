# Element MCP Server - Testing Guide

## Quick Test

Run the cross-platform test script (requires PowerShell Core):

```powershell
cd element-mcp
pwsh test/test-server.ps1
```

Or on Windows:
```powershell
cd element-mcp
.\test\test-server.ps1
```

This will:
1. Build the server
2. Verify it starts correctly
3. Show configuration instructions

## Manual Testing

### 1. Start the Server

```bash
cd src
dotnet run
```

You should see:
```
info: ModelContextProtocol.Server.StdioServerTransport[857250842]
      Server (stream) (ElementMcpServer) transport reading messages.
info: Microsoft.Hosting.Lifetime[0]
      Application started. Press Ctrl+C to shut down.
```

### 2. Test with MCP Inspector (if available)

If you have the MCP Inspector tool:

```bash
npx @modelcontextprotocol/inspector dotnet run --project /path/to/element-mcp/src
```

### 3. Configure with Claude Desktop

Edit `~/Library/Application Support/Claude/claude_desktop_config.json` (macOS):

```json
{
  "mcpServers": {
    "element": {
      "command": "dotnet",
      "args": ["run", "--project", "/full/path/to/element-mcp/src"]
    }
  }
}
```

Restart Claude Desktop and try:
- "List all Element components"
- "Show me the Button component"
- "What color tokens are available?"

### 4. Configure with VS Code Copilot

Create `.vscode/mcp.json` in your workspace:

```json
{
  "servers": {
    "element": {
      "type": "stdio",
      "command": "dotnet",
      "args": ["run", "--project", "/full/path/to/element-mcp/src"]
    }
  }
}
```

Open Copilot Chat and ask questions about Element.

## Expected Tool List

The server should expose these 20+ tools:

### Component Tools (4)
- ListComponents
- GetComponent
- GetComponentsByCategory
- ListComponentCategories

### Foundation Tools (4)
- ListFoundations
- GetFoundation
- GetFoundationsByType
- ListFoundationTypes

### Pattern Tools (4)
- ListPatterns
- GetPattern
- GetPatternsByCategory
- ListPatternCategories

### Template Tools (4)
- ListTemplates
- GetTemplate
- GetTemplatesByType
- ListTemplateTypes

### Search Tools (1)
- Search

## Sample Queries to Test

Once connected to an MCP client, try these queries:

1. **List Components**
   - "What components are available in Element?"
   - "List all UI components"

2. **Get Component Details**
   - "Show me the Button component"
   - "How do I use the TextField component?"
   - "What props does the Dialog component have?"

3. **Explore Foundations**
   - "What are the color tokens?"
   - "Show me the typography scale"
   - "What spacing values are available?"

4. **Learn Patterns**
   - "Show me form validation patterns"
   - "How do I handle data loading states?"
   - "What's the modal workflow pattern?"

5. **Find Templates**
   - "What templates are available?"
   - "Show me the dashboard template"
   - "How do I create a form page?"

6. **Search**
   - "Search for dialog"
   - "Find components related to forms"
   - "Search for button"

## Expected Responses

### Successful Response Example

```json
{
  "Id": "button",
  "Name": "Button",
  "Category": "Inputs",
  "Description": "Buttons allow users to trigger actions...",
  "Usage": "Import Button from @availity/element...",
  "Example": "import { Button } from '@availity/element';\n\n<Button variant=\"contained\">Click Me</Button>",
  "Props": [
    {
      "Name": "variant",
      "Type": "string",
      "Description": "Button variant: text, contained, outlined",
      "DefaultValue": "text"
    }
  ],
  "Accessibility": "Buttons should have descriptive text...",
  "StorybookUrl": "https://availity.github.io/element/?path=/docs/components-button--docs",
  "RelatedComponents": ["IconButton", "ButtonGroup"]
}
```

### Error Response Example

```json
{
  "error": "Component 'xyz' not found"
}
```

### Search Response Example

```json
{
  "query": "dialog",
  "count": 2,
  "results": [
    { /* Dialog component */ },
    { /* Modal Workflow pattern */ }
  ]
}
```

## Troubleshooting

### Server won't start

```bash
# Check .NET version
dotnet --version
# Should be 10.0.x or later

# Clean and rebuild
cd src
dotnet clean
dotnet build

# Check for errors
dotnet run
```

### Tools not appearing

- Verify server is configured correctly in client
- Check client logs for connection errors
- Ensure absolute path is used in configuration
- Restart the client application

### Empty responses

- Check the tool name is correct (case-sensitive)
- Verify parameters match tool signatures
- Enable verbose logging in server

### Performance issues

- In-memory data should be very fast (<100ms)
- Check CPU/memory usage
- Verify only one server instance is running

## Success Criteria

✅ Server builds without errors
✅ Server starts and shows "Application started" message
✅ Client can connect to server
✅ All 20+ tools are available
✅ Queries return valid JSON responses
✅ Sample data includes 6 components, 5 foundations, 4 patterns, 4 templates
✅ Search functionality works across all categories

## Next Steps

Once testing is complete:
1. Publish to NuGet (see README.md)
2. Share configuration with team
3. Add to team documentation
4. Collect feedback for improvements
