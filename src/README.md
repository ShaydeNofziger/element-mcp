# Element MCP Server

This is an MCP (Model Context Protocol) Server for Availity's Element Design System.

## What is Element?

Element is Availity's React-based design system built on Material UI with healthcare-specific customizations. It provides components, foundations, patterns, and templates for building healthcare applications.

**Official Resources**:
- Documentation: https://availity.github.io/element/
- GitHub: https://github.com/Availity/element
- Storybook: https://availity.github.io/element/?path=/docs/element--docs

## What This Server Provides

The Element MCP Server exposes 20+ tools that allow AI assistants to query and retrieve:
- **Components**: UI components like Button, Card, TextField, Alert, Table, Dialog
- **Foundations**: Design tokens for colors, typography, spacing, elevation, theming
- **Patterns**: Design patterns like form validation, data loading, modal workflows
- **Templates**: Page templates like dashboards, forms, list views, detail views

## Available Tools

### Component Tools
- `ListComponents` - List all components
- `GetComponent` - Get component details by ID
- `GetComponentsByCategory` - Filter components by category
- `ListComponentCategories` - List all categories

### Foundation Tools
- `ListFoundations` - List all foundations
- `GetFoundation` - Get foundation details by ID
- `GetFoundationsByType` - Filter foundations by type
- `ListFoundationTypes` - List all types

### Pattern Tools
- `ListPatterns` - List all patterns
- `GetPattern` - Get pattern details by ID
- `GetPatternsByCategory` - Filter patterns by category
- `ListPatternCategories` - List all categories

### Template Tools
- `ListTemplates` - List all templates
- `GetTemplate` - Get template details by ID
- `GetTemplatesByType` - Filter templates by type
- `ListTemplateTypes` - List all types

### Search Tools
- `Search` - Search across all documentation

## Testing Locally

To test this MCP server from source code:

```json
{
  "servers": {
    "element": {
      "type": "stdio",
      "command": "dotnet",
      "args": [
        "run",
        "--project",
        "/path/to/element-mcp/src"
      ]
    }
  }
}
```

### Build and Run

```bash
cd /path/to/element-mcp/src
dotnet build
dotnet run
```

### Example Usage

Once configured in your IDE (VS Code or Visual Studio), you can ask:
- "What components are available in Element?"
- "Show me how to use the Button component"
- "What are the color tokens?"
- "Give me an example of form validation pattern"
- "What templates are available for dashboards?"

The MCP server will respond with detailed documentation including code examples, props, usage guidelines, and best practices.

## Publishing to NuGet.org

1. Update package metadata in `ElementMcpServer.csproj` (already configured)
2. Update `.mcp/server.json` with your information (already configured)
3. Build and pack:
   ```bash
   dotnet pack -c Release
   ```
4. Publish:
   ```bash
   dotnet nuget push bin/Release/*.nupkg --api-key <your-api-key> --source https://api.nuget.org/v3/index.json
   ```

## Using from NuGet

Once published, configure in your IDE:

**VS Code**: Create `.vscode/mcp.json`
**Visual Studio**: Create `.mcp.json`

```json
{
  "servers": {
    "element": {
      "type": "stdio",
      "command": "dnx",
      "args": [
        "ElementMcpServer",
        "--version",
        "1.0.0",
        "--yes"
      ]
    }
  }
}
```

## Architecture

The server is built on .NET 10 with the following structure:

- **Models**: Type-safe data models for Component, Foundation, Pattern, Template
- **Data**: `ElementDataService` provides in-memory documentation data
- **Tools**: MCP tool classes expose functionality to clients
- **Program.cs**: Entry point with dependency injection and MCP configuration

All tools return JSON-serialized responses for easy consumption by AI assistants.

## Documentation

For comprehensive documentation, see:
- **README.md** (root): Full usage guide and installation instructions
- **AGENTS.md**: Detailed architecture, specifications, and development guide

## Requirements

- .NET 10.0 SDK or later
- An MCP-compatible client (VS Code with Copilot, Visual Studio, Claude Desktop)

## More Information

- [MCP Official Documentation](https://modelcontextprotocol.io/)
- [MCP C# SDK](https://modelcontextprotocol.github.io/csharp-sdk)
- [Element Design System](https://availity.github.io/element/)
- [GitHub Repository](https://github.com/ShaydeNofziger/element-mcp)
