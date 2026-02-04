# Element MCP Server

[![Build and Test](https://github.com/ShaydeNofziger/element-mcp/actions/workflows/build-and-test.yml/badge.svg)](https://github.com/ShaydeNofziger/element-mcp/actions/workflows/build-and-test.yml)

An MCP (Model Context Protocol) Server for Availity's Element Design System, built on .NET 8.

## Overview

The Element MCP Server provides comprehensive access to Availity's Element Design System documentation through the Model Context Protocol. This enables AI assistants and other MCP clients to query and retrieve detailed information about components, foundations, patterns, and templates used in healthcare applications.

## Features

- **Component Documentation**: Access detailed information about UI components including props, usage examples, and accessibility guidelines
- **Dynamic GitHub Integration**: Automatically fetches component data from Availity's Storybook and enriches results with direct GitHub repository links
- **Foundation Documentation**: Query design system foundations like colors, typography, spacing, elevation, and theming
- **Pattern Documentation**: Explore reusable design patterns for common healthcare application scenarios
- **Template Documentation**: Access pre-built layout templates for rapid application development
- **Search Functionality**: Search across all documentation categories with a single query
- **.NET 8**: Built on the latest .NET platform for optimal performance and cross-platform support

## About Availity Element Design System

Availity Element is a modern React-based design system built on Material UI with healthcare-specific customizations. It provides:

- **React Components**: Pre-built, accessible components for healthcare applications
- **Design Tokens**: Consistent styling through `@availity/design-tokens`
- **Material UI Foundation**: Built on top of Material UI with Availity theming
- **Healthcare Focus**: Optimized for healthcare and enterprise workflows

**Official Resources**:
- Documentation: https://availity.github.io/element/
- GitHub Repository: https://github.com/Availity/element
- Storybook: https://availity.github.io/element/?path=/docs/element--docs

## Installation

### Prerequisites

- .NET 8.0 SDK or later
- An MCP-compatible client (e.g., Claude Desktop, GitHub Copilot)

### Install from NuGet

```bash
dotnet tool install --global ElementMcpServer
```

### Build from Source

```bash
git clone https://github.com/ShaydeNofziger/element-mcp.git
cd element-mcp/src
dotnet build
dotnet run
```

## Usage

### With Claude Desktop

Add the server to your Claude Desktop configuration (`~/Library/Application Support/Claude/claude_desktop_config.json` on macOS):

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

### With GitHub Copilot

Configure in your VS Code settings or use the MCP extension to connect to the server.

### Standalone Testing

Run the server directly:

```bash
cd src
dotnet run
```

The server uses stdio transport and communicates via JSON-RPC over stdin/stdout.

## Available Tools

The Element MCP Server provides the following tools for querying documentation:

### Component Tools

- **`ListComponents`**: List all available components
- **`GetComponent`**: Get detailed information about a specific component (e.g., 'button', 'card', 'textfield')
- **`GetComponentsByCategory`**: Get components in a category (e.g., 'Inputs', 'Surfaces', 'Feedback')
- **`ListComponentCategories`**: List all component categories

### Foundation Tools

- **`ListFoundations`**: List all design foundations
- **`GetFoundation`**: Get details about a foundation (e.g., 'colors', 'typography', 'spacing')
- **`GetFoundationsByType`**: Get foundations by type
- **`ListFoundationTypes`**: List all foundation types

### Pattern Tools

- **`ListPatterns`**: List all design patterns
- **`GetPattern`**: Get details about a pattern (e.g., 'form-validation', 'data-loading')
- **`GetPatternsByCategory`**: Get patterns in a category
- **`ListPatternCategories`**: List all pattern categories

### Template Tools

- **`ListTemplates`**: List all templates
- **`GetTemplate`**: Get details about a template (e.g., 'dashboard', 'form-page')
- **`GetTemplatesByType`**: Get templates by type
- **`ListTemplateTypes`**: List all template types

### Search Tools

- **`Search`**: Search across all documentation for a query

## Example Queries

Once connected to an MCP client, you can ask questions like:

- "What components are available in the Element Design System?"
- "Show me how to use the Button component"
- "What are the color tokens in the Element design system?"
- "Give me an example of the form validation pattern"
- "What templates are available for dashboards?"
- "Search for dialog components"

### Example Response

When you query for a component, you'll receive enriched data including GitHub links:

```json
{
  "Id": "button",
  "Name": "Button",
  "Category": "Inputs",
  "Description": "Buttons allow users to trigger actions and navigate throughout the application.",
  "Usage": "Use buttons to trigger actions like submitting forms, opening dialogs, or navigating.",
  "StorybookUrl": "https://availity.github.io/element/?path=/docs/components-button-introduction--docs",
  "PackageImport": "import { Button } from '@availity/element';",
  "GitHubChangelogUrl": "https://github.com/Availity/element/tree/main/packages/button/CHANGELOG.md",
  "GitHubPackageUrl": "https://github.com/Availity/element/tree/main/packages/button"
}
```

The enriched properties provide:
- **PackageImport**: Example of how to import the component from @availity/element
- **GitHubChangelogUrl**: Direct link to view the component's changelog on GitHub
- **GitHubPackageUrl**: Link to browse the entire component package directory on GitHub

## Architecture

The Element MCP Server is built with the following architecture:

```
ElementMcpServer/
├── Models/              # Data models for components, foundations, patterns, templates
│   ├── Component.cs
│   ├── Foundation.cs
│   ├── Pattern.cs
│   └── Template.cs
├── Data/                # Data storage and service
│   ├── element-data.json          # Component documentation data
│   └── ElementDataService.cs      # Data access layer
├── Services/            # Support services
│   ├── StorybookService.cs        # Fetches data from Availity Storybook
│   └── DataEnrichmentService.cs   # Enriches components with GitHub links
├── Tools/               # MCP tool implementations
│   ├── ComponentTools.cs
│   ├── FoundationTools.cs
│   ├── PatternTools.cs
│   ├── TemplateTools.cs
│   └── SearchTools.cs
├── Program.cs           # Application entry point and MCP server configuration
└── .mcp/
    └── server.json      # MCP server metadata
```

### Key Components

- **Models**: Strongly-typed C# records representing the Element Design System entities
- **element-data.json**: JSON file containing all component, foundation, pattern, and template documentation
- **ElementDataService**: Provides in-memory data access with filtering and search capabilities
- **StorybookService**: Fetches and parses the Storybook index from https://availity.github.io/element/index.json
- **DataEnrichmentService**: Background service that enriches component data during startup with GitHub URLs and import examples
- **MCP Tools**: Decorated with `[McpServerTool]` attributes to expose functionality to MCP clients
- **Stdio Transport**: Uses standard input/output for communication with MCP clients

### Dynamic Data Enrichment

On startup, the server:
1. Loads component documentation from the local `element-data.json` file
2. Fetches the latest Storybook index from Availity's GitHub Pages
3. Matches components with their Storybook entries
4. Enriches each component with:
   - **PackageImport**: Example import statement (e.g., `import { Button } from '@availity/element';`)
   - **GitHubChangelogUrl**: Direct link to the component's CHANGELOG.md on GitHub
   - **GitHubPackageUrl**: Link to browse the package directory on GitHub
   - **StorybookUrl**: Updated URL pointing to the component's introduction page

This ensures users always get direct links to the source code and documentation for each component.

## Development

### Building

```bash
cd src
dotnet build
```

### Testing

Run the server and test with an MCP client or use the included test scripts:

```bash
cd src
dotnet run
```

The server will start and listen for MCP protocol messages on stdin.

### Adding New Documentation

To add or update Element documentation:

1. Update the data initialization methods in `Data/ElementDataService.cs`
2. Rebuild the project
3. Test with an MCP client

## CI/CD

The project includes automated GitHub Actions workflows for continuous integration:

- **Build and Test**: Automatically runs on pull requests and pushes to main/master
  - Builds the project on Ubuntu, Windows, and macOS
  - Verifies server startup
  - Validates project structure and all required files
  - Creates build summary with results

The workflow ensures code quality and cross-platform compatibility before merging changes.

## Contributing

Contributions are welcome! Please:

1. Fork the repository
2. Create a feature branch
3. Make your changes
4. Ensure CI/CD checks pass
5. Submit a pull request

All pull requests are automatically validated by the CI/CD pipeline.

## License

This project is licensed under the MIT License - see the LICENSE file for details.

## Acknowledgments

- [Availity](https://www.availity.com/) for creating the Element Design System
- [Microsoft](https://github.com/microsoft/mcp-dotnet-samples) for the MCP .NET SDK
- The Model Context Protocol community

## Support

For issues and questions:
- GitHub Issues: https://github.com/ShaydeNofziger/element-mcp/issues
- Element Documentation: https://availity.github.io/element/
- MCP Specification: https://modelcontextprotocol.io/

## Version History

### 1.1.0 (2026-02-04)
- **NEW**: Dynamic Storybook integration - fetches component data from https://availity.github.io/element/index.json
- **NEW**: Component responses now include enriched properties:
  - `PackageImport`: Example import statement from @availity/element
  - `GitHubChangelogUrl`: Direct link to component's CHANGELOG.md
  - `GitHubPackageUrl`: Link to browse component package directory
  - `StorybookUrl`: Updated to point to introduction page
- **NEW**: Automatic data enrichment on server startup via DataEnrichmentService
- **NEW**: StorybookService for fetching and parsing Storybook data
- **NEW**: JSON-based data storage (element-data.json)

### 1.0.0 (2026-02-04)
- Initial release
- Support for components, foundations, patterns, and templates
- Full search functionality
- .NET 8 support
- Stdio transport
