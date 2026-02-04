# Element MCP Server - Quick Start Demo

This document demonstrates the Element MCP Server capabilities.

## What is This?

An MCP (Model Context Protocol) server that provides AI assistants with access to Availity's Element Design System documentation.

## Quick Demo

### Available Documentation

The server provides comprehensive information about:

**47 Components** across 6 categories:
- **Inputs (11):** Button, IconButton, ButtonGroup, TextField, Select, Autocomplete, Checkbox, Radio, Switch, Slider, DatePicker
- **Surfaces (5):** Card, Paper, Accordion, AppBar, Toolbar
- **Feedback (8):** Alert, Snackbar, Dialog, Progress, CircularProgress, LinearProgress, Backdrop, Skeleton
- **Data Display (8):** Table, List, Chip, Avatar, Badge, Tooltip, Typography, Divider
- **Navigation (8):** Tabs, Drawer, Menu, Breadcrumbs, Link, Stepper, Pagination, BottomNavigation
- **Layout (5):** Box, Container, Grid, Stack, ImageList

**8 Foundations:**
- Colors - Design tokens and palette
- Typography - Font scale and styles
- Spacing - 8px grid system
- Elevation - Shadow depths
- Breakpoints - Responsive design system
- Theme - ThemeProvider configuration
- Icons - Material Design icon system
- Shape - Border radius and shape system

**8 Patterns:**
- Form Validation - Consistent form handling
- Data Loading States - Loading/error/empty states
- Modal Workflow - Multi-step dialogs
- Search and Filter - Data exploration
- Responsive Layout - Cross-device layouts
- Navigation Patterns - App navigation structures
- Data Tables - Sortable, filterable tables
- Form Layouts - Structured form design

**6 Templates:**
- Dashboard Layout - Metrics and widgets
- Form Page - Structured data entry
- List View - Searchable tables
- Detail View - Entity information with tabs
- Wizard Flow - Multi-step processes
- Settings Page - Configuration interface

### Example Usage

Ask the AI assistant questions like:

```
"What components are available in the Element Design System?"
```

**Response:** Lists all 6 components with categories

```
"Show me how to use the Button component"
```

**Response:** Full component details including:
- Description and usage guidelines
- All props with types and defaults
- Complete code example
- Accessibility information
- Related components
- Link to Storybook

```
"What are the primary color tokens?"
```

**Response:** Color foundation with all tokens:
- primary.main: #0078B6
- primary.light: #33A3D5
- primary.dark: #00578F
- And more semantic colors

```
"Show me the form validation pattern"
```

**Response:** Pattern details including:
- Problem it solves
- Solution approach
- Best practices
- Complete code example
- Components used
- Related patterns

```
"Search for dialog"
```

**Response:** All items matching "dialog":
- Dialog component
- Modal Workflow pattern
- With full details for each

## Installation

### From NuGet (once published)

```bash
dotnet tool install -g ElementMcpServer
```

### From Source

```bash
git clone https://github.com/ShaydeNofziger/element-mcp.git
cd element-mcp/src
dotnet build
dotnet run
```

## Configuration

### Claude Desktop

Edit `~/Library/Application Support/Claude/claude_desktop_config.json`:

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

### VS Code with Copilot

Create `.vscode/mcp.json`:

```json
{
  "servers": {
    "element": {
      "type": "stdio",
      "command": "dotnet",
      "args": ["run", "--project", "/path/to/element-mcp/src"]
    }
  }
}
```

## Key Features

✅ **20+ MCP Tools** - Complete coverage of documentation
✅ **.NET 10** - Latest platform features
✅ **Type-Safe** - Strongly-typed data models
✅ **Fast** - In-memory data for instant responses
✅ **Comprehensive** - Full examples and guidelines
✅ **Searchable** - Find anything quickly
✅ **Cross-Platform** - Works on Windows, macOS, Linux

## Architecture Highlights

```
MCP Client (Claude/Copilot)
         ↓ JSON-RPC over stdio
Element MCP Server (.NET 10)
         ↓
    Tool Layer (5 classes, 20+ tools)
         ↓
  ElementDataService (Query/Filter/Search)
         ↓
  Data Models (Component/Foundation/Pattern/Template)
```

## Sample Data Included

**Components** (6):
- Button, Card, TextField, Alert, Table, Dialog
- With props, examples, accessibility info

**Foundations** (5):
- Colors, Typography, Spacing, Elevation, Theme
- With design tokens and usage guidelines

**Patterns** (4):
- Form Validation, Data Loading, Modal Workflow, Search/Filter
- With problem/solution and best practices

**Templates** (4):
- Dashboard, Form Page, List View, Detail View
- With use cases and customization guides

## Real-World Usage

This server enables AI assistants to:

1. **Answer Questions**: "How do I create a form with validation?"
2. **Provide Examples**: Complete, copy-paste ready code
3. **Suggest Best Practices**: Accessibility, UX patterns
4. **Guide Development**: Step-by-step component usage
5. **Discover Related Items**: Find complementary components/patterns

## Benefits

**For Developers:**
- Instant access to documentation without leaving IDE
- AI-powered code generation using Element
- Contextual examples and best practices
- Reduced context switching

**For Teams:**
- Consistent component usage
- Faster onboarding
- Reduced documentation lookups
- Better adherence to design system

**For AI Assistants:**
- Structured, queryable knowledge
- Complete component specifications
- Real code examples
- Design system context

## Documentation

- **README.md** - Comprehensive usage guide
- **AGENTS.md** - Architecture and specifications
- **src/README.md** - Developer guide
- **test/TESTING.md** - Testing instructions

## Links

- GitHub: https://github.com/ShaydeNofziger/element-mcp
- Element Design System: https://availity.github.io/element/
- Element GitHub: https://github.com/Availity/element
- MCP Specification: https://modelcontextprotocol.io/

## Try It Now!

1. Clone the repository
2. Run `cd element-mcp/src && dotnet run`
3. Configure your MCP client
4. Ask questions about Element!

---

Built with ❤️ using .NET 10 and the Model Context Protocol
