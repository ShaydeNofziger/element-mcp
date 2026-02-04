# Element MCP Server - Architecture and Specifications

## Table of Contents

1. [Overview](#overview)
2. [Architecture](#architecture)
3. [Technical Specifications](#technical-specifications)
4. [Data Models](#data-models)
5. [MCP Tools](#mcp-tools)
6. [Integration Guide](#integration-guide)
7. [Development Guide](#development-guide)
8. [Best Practices](#best-practices)

## Overview

The Element MCP Server is a Model Context Protocol (MCP) server implementation that provides AI assistants and other MCP clients with structured access to Availity's Element Design System documentation. Built on .NET 10, it exposes a comprehensive set of tools for querying components, foundations, patterns, and templates.

### Purpose

Enable AI-powered development workflows by providing:
- Instant access to Element Design System documentation
- Contextual code examples and usage patterns
- Design system best practices and guidelines
- Searchable knowledge base for healthcare UI development

### Key Features

- **20+ MCP Tools** for comprehensive documentation access
- **Type-safe data models** representing design system entities
- **Efficient in-memory data service** for fast queries
- **JSON-serialized responses** for easy consumption
- **Cross-platform support** via .NET 10
- **Stdio transport** for universal MCP client compatibility

## Architecture

### High-Level Architecture

```
┌─────────────────────────────────────────────────────────────┐
│                     MCP Client                              │
│            (Claude Desktop, Copilot, etc.)                  │
└───────────────────────┬─────────────────────────────────────┘
                        │ JSON-RPC over stdio
                        │
┌───────────────────────▼─────────────────────────────────────┐
│                  Element MCP Server                         │
│  ┌─────────────────────────────────────────────────────┐   │
│  │              Program.cs (Entry Point)               │   │
│  │  - Host configuration                               │   │
│  │  - Logging setup                                    │   │
│  │  - Service registration                             │   │
│  │  - MCP server initialization                        │   │
│  │  - Background service startup                       │   │
│  └─────────────────────┬───────────────────────────────┘   │
│                        │                                     │
│  ┌─────────────────────▼───────────────────────────────┐   │
│  │           MCP Tool Layer                            │   │
│  │  ┌──────────────┐  ┌──────────────┐               │   │
│  │  │ Component    │  │ Foundation   │               │   │
│  │  │ Tools        │  │ Tools        │               │   │
│  │  └──────────────┘  └──────────────┘               │   │
│  │  ┌──────────────┐  ┌──────────────┐  ┌────────┐  │   │
│  │  │ Pattern      │  │ Template     │  │ Search │  │   │
│  │  │ Tools        │  │ Tools        │  │ Tools  │  │   │
│  │  └──────────────┘  └──────────────┘  └────────┘  │   │
│  └─────────────────────┬───────────────────────────────┘   │
│                        │                                     │
│  ┌─────────────────────▼───────────────────────────────┐   │
│  │          ElementDataService                         │   │
│  │  - Data initialization from JSON                    │   │
│  │  - Query methods                                    │   │
│  │  - Filtering logic                                  │   │
│  │  - Search implementation                            │   │
│  │  - Component enrichment                             │   │
│  └──────┬──────────────────────────────────────────────┘   │
│         │                                                    │
│  ┌──────▼─────────────┐  ┌──────────────────────────┐     │
│  │  Services Layer    │  │  Data Storage            │     │
│  │  ┌───────────────┐ │  │  ┌──────────────────┐   │     │
│  │  │ Storybook     │ │  │  │ element-data.json│   │     │
│  │  │ Service       │ │  │  │ (Components,     │   │     │
│  │  └───────────────┘ │  │  │  Foundations,    │   │     │
│  │  ┌───────────────┐ │  │  │  Patterns,       │   │     │
│  │  │ Data          │ │  │  │  Templates)      │   │     │
│  │  │ Enrichment    │ │  │  └──────────────────┘   │     │
│  │  │ Service       │ │  │                          │     │
│  │  └───────────────┘ │  └──────────────────────────┘     │
│  └────────────────────┘                                    │
│         │                                                    │
│  ┌──────▼─────────────────────────────────────────────┐   │
│  │              Data Models                            │   │
│  │  - Component (with ComponentProp)                   │   │
│  │  - Foundation                                       │   │
│  │  - Pattern                                          │   │
│  │  - Template                                         │   │
│  └─────────────────────────────────────────────────────┘   │
└─────────────────────────────────────────────────────────────┘
```

### Component Breakdown

#### 1. Program.cs (Entry Point)

**Responsibilities**:
- Configure the .NET Host with dependency injection
- Set up console logging to stderr (avoiding stdout interference with MCP protocol)
- Register `ElementDataService` as a singleton
- Configure MCP server with stdio transport
- Register all tool classes

**Key Code**:
```csharp
// Register HttpClient for StorybookService
builder.Services.AddHttpClient<StorybookService>();

// Register the Storybook Service
builder.Services.AddSingleton<StorybookService>();

// Register the Element Data Service as a singleton
builder.Services.AddSingleton<ElementDataService>();

// Register the data enrichment background service
builder.Services.AddHostedService<DataEnrichmentService>();

// Add the MCP services
builder.Services
    .AddMcpServer()
    .WithStdioServerTransport()
    .WithTools<ComponentTools>()
    .WithTools<FoundationTools>()
    .WithTools<PatternTools>()
    .WithTools<TemplateTools>()
    .WithTools<SearchTools>();
```

#### 2. Data Models

**Location**: `src/Models/`

**Purpose**: Provide strongly-typed representations of Element Design System entities.

**Models**:

- **Component**: Represents UI components (Button, Card, TextField, etc.)
  - Properties: Id, Name, Category, Description, Usage, Example, Props, RelatedComponents, Accessibility, StorybookUrl, PackageImport, GitHubChangelogUrl, GitHubPackageUrl
  - Nested: ComponentProp (Name, Type, Required, DefaultValue, Description)

- **Foundation**: Represents design foundations (Colors, Typography, Spacing, etc.)
  - Properties: Id, Name, Type, Description, Usage, Tokens, Example, RelatedFoundations

- **Pattern**: Represents design patterns (Form Validation, Data Loading, etc.)
  - Properties: Id, Name, Category, Description, Problem, Solution, BestPractices, Example, ComponentsUsed, RelatedPatterns

- **Template**: Represents page templates (Dashboard, Form Page, List View, etc.)
  - Properties: Id, Name, Type, Description, UseCase, Customization, Example, ComponentsUsed, PatternsUsed, RelatedTemplates

**Design Decisions**:
- Used C# records for immutability
- Required properties use `required` keyword
- Optional properties use nullable reference types
- Lists and dictionaries for collections

#### 3. ElementDataService

**Location**: `src/Data/ElementDataService.cs`

**Purpose**: Central data access layer providing in-memory documentation data loaded from JSON file.

**Responsibilities**:
- Load documentation data from `element-data.json`
- Provide query methods for all entity types
- Implement filtering by category/type
- Implement cross-entity search functionality
- Enrich components with Storybook data at startup

**Key Methods**:
```csharp
// Data enrichment
Task EnrichComponentsAsync()

// Components
IEnumerable<Component> GetAllComponents()
Component? GetComponent(string id)
IEnumerable<Component> GetComponentsByCategory(string category)

// Foundations
IEnumerable<Foundation> GetAllFoundations()
Foundation? GetFoundation(string id)
IEnumerable<Foundation> GetFoundationsByType(string type)

// Patterns
IEnumerable<Pattern> GetAllPatterns()
Pattern? GetPattern(string id)
IEnumerable<Pattern> GetPatternsByCategory(string category)

// Templates
IEnumerable<Template> GetAllTemplates()
Template? GetTemplate(string id)
IEnumerable<Template> GetTemplatesByType(string type)

// Search
IEnumerable<object> Search(string query)
```

**Data Storage**:
- Data loaded from JSON file (`Data/element-data.json`)
- Stored in-memory collections for fast access
- Enriched at startup by DataEnrichmentService
- Case-insensitive querying

#### 4. StorybookService

**Location**: `src/Services/StorybookService.cs`

**Purpose**: Fetches and parses Storybook index data from Availity's GitHub Pages.

**Responsibilities**:
- Fetch Storybook index from https://availity.github.io/element/index.json
- Parse and cache Storybook entries
- Match components to their Storybook documentation
- Generate GitHub URLs and import statements

**Key Methods**:
```csharp
Task<Dictionary<string, StorybookEntry>> GetStorybookEntriesAsync()
Task<StorybookEntry?> FindComponentEntryAsync(string componentName, string? storybookUrl)
string? ConvertImportPathToChangelogUrl(string importPath)
string? GeneratePackageImport(string componentName, string importPath)
string? GenerateStorybookIntroductionUrl(string? storybookEntryId)
string? GetGitHubPackageUrl(string importPath)
```

#### 5. DataEnrichmentService

**Location**: `src/Services/DataEnrichmentService.cs`

**Purpose**: Background service that enriches component data during application startup.

**Responsibilities**:
- Start enrichment process when application starts
- Call ElementDataService.EnrichComponentsAsync()
- Handle errors gracefully (app starts even if enrichment fails)

**Implementation**:
- Implements `IHostedService`
- Runs during application startup (StartAsync)
- Logs progress and errors

#### 6. MCP Tools

**Location**: `src/Tools/`

**Purpose**: Expose functionality to MCP clients through decorated methods.

**Tool Classes**:

1. **ComponentTools**: 4 tools for component queries
2. **FoundationTools**: 4 tools for foundation queries
3. **PatternTools**: 4 tools for pattern queries
4. **TemplateTools**: 4 tools for template queries
5. **SearchTools**: 1 tool for cross-entity search

**Tool Structure**:
```csharp
[McpServerTool]
[Description("Tool description for MCP clients")]
public string ToolName(
    [Description("Parameter description")] string param)
{
    // Implementation
    return JsonSerializer.Serialize(result);
}
```

**Tool Patterns**:
- All tools return JSON-serialized strings
- Error responses include `{ error: "message" }` structure
- Success responses match data model structure
- Descriptive parameter documentation

## Technical Specifications

### Platform Requirements

- **.NET Version**: 10.0 or later
- **Runtime**: Cross-platform (Windows, macOS, Linux)
- **Architecture**: x64, ARM64
- **Target Framework**: net10.0

### Dependencies

```xml
<PackageReference Include="Microsoft.Extensions.Hosting" Version="8.0.1" />
<PackageReference Include="Microsoft.Extensions.Http" Version="8.0.1" />
<PackageReference Include="ModelContextProtocol" Version="0.5.0-preview.1" />
```

### Transport Protocol

- **Type**: Standard Input/Output (stdio)
- **Protocol**: JSON-RPC 2.0
- **Message Format**: Newline-delimited JSON

### Performance Characteristics

- **Startup Time**: 2-5 seconds (includes network fetch of Storybook index)
- **Memory Usage**: ~50-100 MB
- **Query Response Time**: < 100ms (in-memory data)
- **Concurrent Connections**: Single client (stdio limitation)
- **Network Dependency**: Initial fetch from https://availity.github.io/element/index.json

### Configuration

The server is configured through:

1. **Project File** (`ElementMcpServer.csproj`): Build and package settings
2. **MCP Metadata** (`.mcp/server.json`): Server identification and capabilities
3. **Code Configuration** (`Program.cs`): Service registration and logging

## Data Models

### Component Model

```csharp
public record Component
{
    public required string Id { get; init; }
    public required string Name { get; init; }
    public required string Category { get; init; }
    public required string Description { get; init; }
    public string? Usage { get; init; }
    public string? Example { get; init; }
    public List<ComponentProp>? Props { get; init; }
    public List<string>? RelatedComponents { get; init; }
    public string? Accessibility { get; init; }
    public string? StorybookUrl { get; init; }
    public string? PackageImport { get; init; }
    public string? GitHubChangelogUrl { get; init; }
    public string? GitHubPackageUrl { get; init; }
}

public record ComponentProp
{
    public required string Name { get; init; }
    public required string Type { get; init; }
    public bool Required { get; init; }
    public string? DefaultValue { get; init; }
    public string? Description { get; init; }
}
```

**Example Data**:
- Components loaded from JSON file with 47+ components across categories
- Categories: Inputs (11), Surfaces (5), Feedback (8), Data Display (8), Navigation (8), Layout (5)
- Components enriched at startup with GitHub links, import statements, and changelog URLs
- Complete with props, examples, accessibility info, and Storybook URLs

### Foundation Model

```csharp
public record Foundation
{
    public required string Id { get; init; }
    public required string Name { get; init; }
    public required string Type { get; init; }
    public required string Description { get; init; }
    public string? Usage { get; init; }
    public Dictionary<string, string>? Tokens { get; init; }
    public string? Example { get; init; }
    public List<string>? RelatedFoundations { get; init; }
}
```

**Example Data**:
- 8 foundations: Colors, Typography, Spacing, Elevation, Breakpoints, Theme, Icons, Shape
- Types: Color, Typography, Spacing, Elevation, Responsive, Theme, Iconography, Shape
- Includes design token mappings and usage guidance

### Pattern Model

```csharp
public record Pattern
{
    public required string Id { get; init; }
    public required string Name { get; init; }
    public required string Category { get; init; }
    public required string Description { get; init; }
    public string? Problem { get; init; }
    public string? Solution { get; init; }
    public string? BestPractices { get; init; }
    public string? Example { get; init; }
    public List<string>? ComponentsUsed { get; init; }
    public List<string>? RelatedPatterns { get; init; }
}
```

**Example Data**:
- 8 patterns: Form Validation, Data Loading States, Modal Workflow, Search and Filter, Responsive Layout, Navigation Patterns, Data Tables, Form Layouts
- Categories: Forms, Feedback, Navigation, Data Display, Layout
- Includes problem/solution framework and best practices

### Template Model

```csharp
public record Template
{
    public required string Id { get; init; }
    public required string Name { get; init; }
    public required string Type { get; init; }
    public required string Description { get; init; }
    public string? UseCase { get; init; }
    public string? Customization { get; init; }
    public string? Example { get; init; }
    public List<string>? ComponentsUsed { get; init; }
    public List<string>? PatternsUsed { get; init; }
    public List<string>? RelatedTemplates { get; init; }
}
```

**Example Data**:
- 6 templates: Dashboard Layout, Form Page, List View, Detail View, Wizard Flow, Settings Page
- Types: Page Layout, Form, Data Display
- Includes use cases and customization guidance

## MCP Tools

### Tool Categories

#### Component Tools (ComponentTools.cs)

| Tool | Description | Parameters | Returns |
|------|-------------|------------|---------|
| `ListComponents` | Lists all components | None | Array of component summaries |
| `GetComponent` | Gets component details | `componentId` (string) | Component object or error |
| `GetComponentsByCategory` | Filters by category | `category` (string) | Array of components |
| `ListComponentCategories` | Lists categories | None | Array of category names |

#### Foundation Tools (FoundationTools.cs)

| Tool | Description | Parameters | Returns |
|------|-------------|------------|---------|
| `ListFoundations` | Lists all foundations | None | Array of foundation summaries |
| `GetFoundation` | Gets foundation details | `foundationId` (string) | Foundation object or error |
| `GetFoundationsByType` | Filters by type | `type` (string) | Array of foundations |
| `ListFoundationTypes` | Lists types | None | Array of type names |

#### Pattern Tools (PatternTools.cs)

| Tool | Description | Parameters | Returns |
|------|-------------|------------|---------|
| `ListPatterns` | Lists all patterns | None | Array of pattern summaries |
| `GetPattern` | Gets pattern details | `patternId` (string) | Pattern object or error |
| `GetPatternsByCategory` | Filters by category | `category` (string) | Array of patterns |
| `ListPatternCategories` | Lists categories | None | Array of category names |

#### Template Tools (TemplateTools.cs)

| Tool | Description | Parameters | Returns |
|------|-------------|------------|---------|
| `ListTemplates` | Lists all templates | None | Array of template summaries |
| `GetTemplate` | Gets template details | `templateId` (string) | Template object or error |
| `GetTemplatesByType` | Filters by type | `type` (string) | Array of templates |
| `ListTemplateTypes` | Lists types | None | Array of type names |

#### Search Tools (SearchTools.cs)

| Tool | Description | Parameters | Returns |
|------|-------------|------------|---------|
| `Search` | Searches all documentation | `query` (string) | Search results with count |

### Tool Response Format

**Success Response**:
```json
{
  "Id": "button",
  "Name": "Button",
  "Category": "Inputs",
  "Description": "...",
  "StorybookUrl": "https://availity.github.io/element/?path=/docs/components-button-introduction--docs",
  "PackageImport": "import { Button } from '@availity/element';",
  "GitHubChangelogUrl": "https://github.com/Availity/element/tree/main/packages/button/CHANGELOG.md",
  "GitHubPackageUrl": "https://github.com/Availity/element/tree/main/packages/button",
  ...
}
```

**Error Response**:
```json
{
  "error": "Component 'xyz' not found"
}
```

**Search Response**:
```json
{
  "query": "search term",
  "count": 5,
  "results": [...]
}
```

## Integration Guide

### MCP Client Configuration

#### Claude Desktop

1. Open configuration file:
   - macOS: `~/Library/Application Support/Claude/claude_desktop_config.json`
   - Windows: `%APPDATA%\Claude\claude_desktop_config.json`

2. Add server configuration:
```json
{
  "mcpServers": {
    "element": {
      "command": "dotnet",
      "args": ["tool", "run", "ElementMcpServer"]
    }
  }
}
```

3. Restart Claude Desktop

#### GitHub Copilot

1. Install MCP extension for VS Code
2. Configure server in settings:
```json
{
  "mcp.servers": {
    "element": {
      "command": "dotnet",
      "args": ["run", "--project", "/path/to/element-mcp/src"]
    }
  }
}
```

#### Custom Integration

Use the MCP SDK to connect programmatically:

```csharp
using ModelContextProtocol.Client;

var client = new McpClient();
await client.ConnectAsync("dotnet", new[] { "run", "--project", "path/to/src" });

var result = await client.CallToolAsync("ListComponents", new { });
```

### Testing the Connection

Once configured, test with queries like:
- "List all Element components"
- "Show me the Button component documentation"
- "What color tokens are available?"

## Development Guide

### Project Structure

```
element-mcp/
├── src/
│   ├── Models/
│   │   ├── Component.cs
│   │   ├── Foundation.cs
│   │   ├── Pattern.cs
│   │   └── Template.cs
│   ├── Data/
│   │   ├── element-data.json
│   │   └── ElementDataService.cs
│   ├── Services/
│   │   ├── StorybookService.cs
│   │   └── DataEnrichmentService.cs
│   ├── Tools/
│   │   ├── ComponentTools.cs
│   │   ├── FoundationTools.cs
│   │   ├── PatternTools.cs
│   │   ├── TemplateTools.cs
│   │   └── SearchTools.cs
│   ├── .mcp/
│   │   └── server.json
│   ├── Program.cs
│   ├── ElementMcpServer.csproj
│   └── README.md
├── test/
│   ├── TESTING.md
│   └── test-server.ps1
├── README.md
├── AGENTS.md
├── DEMO.md
├── IMPLEMENTATION_SUMMARY.md
└── .gitignore
```

### Adding New Components

1. **Add data** to `Data/element-data.json`:
```json
{
  "id": "new-component",
  "name": "NewComponent",
  "category": "Category",
  "description": "Description...",
  "usage": "Usage instructions...",
  "storybookUrl": "https://availity.github.io/element/?path=/docs/..."
}
```

2. **Rebuild** the project:
```bash
dotnet build
```

3. **Test** with MCP client - component will be automatically enriched with GitHub links on startup

### Adding New Tools

1. **Create tool method** in appropriate tools class:
```csharp
[McpServerTool]
[Description("Tool description")]
public string NewTool([Description("param")] string param)
{
    // Implementation
    return JsonSerializer.Serialize(result);
}
```

2. **No registration needed** - automatic via reflection

3. **Test** the new tool

### Logging and Debugging

**Enable verbose logging**:
```csharp
builder.Logging.AddConsole(o => 
{
    o.LogToStandardErrorThreshold = LogLevel.Trace;
});
```

**Log in tools**:
```csharp
public class ComponentTools
{
    private readonly ILogger<ComponentTools> _logger;
    
    public ComponentTools(ElementDataService dataService, ILogger<ComponentTools> logger)
    {
        _dataService = dataService;
        _logger = logger;
    }
    
    [McpServerTool]
    public string GetComponent(string componentId)
    {
        _logger.LogInformation("Getting component: {ComponentId}", componentId);
        // ...
    }
}
```

### Testing

**Manual Testing**:
```bash
cd src
dotnet run
# Server starts and waits for JSON-RPC messages on stdin
```

**Send test message** (JSON-RPC format):
```json
{
  "jsonrpc": "2.0",
  "id": 1,
  "method": "tools/call",
  "params": {
    "name": "ListComponents",
    "arguments": {}
  }
}
```

### Publishing

**Build for release**:
```bash
dotnet publish -c Release -r linux-x64 --self-contained
```

**Create NuGet package**:
```bash
dotnet pack -c Release
```

**Publish to NuGet**:
```bash
dotnet nuget push bin/Release/ElementMcpServer.1.0.0.nupkg --source https://api.nuget.org/v3/index.json --api-key YOUR_API_KEY
```

## Best Practices

### For MCP Server Development

1. **Keep tools focused**: Each tool should do one thing well
2. **Use clear descriptions**: Help AI understand tool purpose
3. **Return JSON**: Ensures consistent, parseable responses
4. **Handle errors gracefully**: Always return error objects, never throw
5. **Log to stderr**: Keep stdout clean for MCP protocol
6. **Use dependency injection**: Makes testing and maintenance easier
7. **Document thoroughly**: Good docs help both humans and AI

### For Using the Server

1. **Start with list operations**: Get overview before querying details
2. **Use search for exploration**: Find relevant items quickly
3. **Combine tools**: Chain queries for comprehensive information
4. **Check categories first**: Know what's available before filtering
5. **Read examples**: Code samples provide implementation guidance

### For Extending the Server

1. **Follow existing patterns**: Maintain consistency with current code
2. **Add tests**: Ensure reliability (future enhancement)
3. **Update documentation**: Keep README.md and AGENTS.md current
4. **Use semantic versioning**: Communicate breaking changes clearly
5. **Consider performance**: In-memory data is fast, keep it that way

## Future Enhancements

Potential improvements for future versions:

1. **Resource Providers**: Expose documentation as MCP resources
2. **Prompt Templates**: Pre-built prompts for common queries
3. **Live Documentation**: Fetch from Availity GitHub Pages
4. **Caching Layer**: Optimize repeated queries
5. **HTTP Transport**: Enable remote access
6. **GraphQL Support**: Flexible querying
7. **Unit Tests**: Comprehensive test coverage
8. **Integration Tests**: End-to-end MCP testing
9. **Performance Monitoring**: Usage analytics
10. **Auto-updating**: Fetch latest Element docs on schedule

## Troubleshooting

### Server won't start

- Verify .NET 10 SDK is installed: `dotnet --version`
- Check for build errors: `dotnet build`
- Ensure no other process is using stdio

### Tools not appearing

- Verify tool class is registered in `Program.cs`
- Check `[McpServerTool]` attribute is present
- Ensure method is public
- Rebuild the project

### Responses are empty

- Check data initialization in `ElementDataService`
- Verify `element-data.json` exists and is valid
- Check data enrichment logs for errors
- Verify JSON serialization settings
- Enable logging to debug

### Client can't connect

- Verify client configuration file syntax
- Check dotnet command path
- Ensure server builds successfully
- Review client logs for connection errors

## Conclusion

The Element MCP Server provides a robust, extensible foundation for integrating Availity's Element Design System into AI-powered development workflows. By following this specification and the established patterns, developers can easily extend the server with new capabilities while maintaining consistency and reliability.

For questions or contributions, please visit: https://github.com/ShaydeNofziger/element-mcp
