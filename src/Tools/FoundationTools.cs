using System.ComponentModel;
using System.Text.Json;
using ElementMcpServer.Data;
using ModelContextProtocol.Server;

namespace ElementMcpServer.Tools;

/// <summary>
/// MCP tools for querying Availity Element Design System foundations.
/// </summary>
public class FoundationTools
{
    private readonly ElementDataService _dataService;

    public FoundationTools(ElementDataService dataService)
    {
        _dataService = dataService;
    }

    [McpServerTool]
    [Description("Lists all available foundations in the Availity Element Design System. Foundations include colors, typography, spacing, elevation, and theme.")]
    public string ListFoundations()
    {
        var foundations = _dataService.GetAllFoundations()
            .Select(f => new { f.Id, f.Name, f.Type, f.Description })
            .ToList();

        return JsonSerializer.Serialize(foundations, new JsonSerializerOptions { WriteIndented = true });
    }

    [McpServerTool]
    [Description("Gets detailed information about a specific foundation including design tokens, usage guidelines, and examples.")]
    public string GetFoundation(
        [Description("The ID of the foundation (e.g., 'colors', 'typography', 'spacing', 'elevation', 'theme')")] string foundationId)
    {
        var foundation = _dataService.GetFoundation(foundationId);
        if (foundation == null)
        {
            return JsonSerializer.Serialize(new { error = $"Foundation '{foundationId}' not found" });
        }

        return JsonSerializer.Serialize(foundation, new JsonSerializerOptions { WriteIndented = true });
    }

    [McpServerTool]
    [Description("Gets all foundations of a specific type (e.g., 'Color', 'Typography', 'Spacing').")]
    public string GetFoundationsByType(
        [Description("The type of foundations to retrieve")] string type)
    {
        var foundations = _dataService.GetFoundationsByType(type).ToList();
        
        if (!foundations.Any())
        {
            return JsonSerializer.Serialize(new { error = $"No foundations found of type '{type}'" });
        }

        return JsonSerializer.Serialize(foundations, new JsonSerializerOptions { WriteIndented = true });
    }

    [McpServerTool]
    [Description("Lists all available foundation types in the Element Design System.")]
    public string ListFoundationTypes()
    {
        var types = _dataService.GetAllFoundations()
            .Select(f => f.Type)
            .Distinct()
            .OrderBy(t => t)
            .ToList();

        return JsonSerializer.Serialize(types, new JsonSerializerOptions { WriteIndented = true });
    }
}
