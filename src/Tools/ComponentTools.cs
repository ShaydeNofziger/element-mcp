using System.ComponentModel;
using System.Text.Json;
using ElementMcpServer.Data;
using ElementMcpServer.Models;
using ModelContextProtocol.Server;

namespace ElementMcpServer.Tools;

/// <summary>
/// MCP tools for querying Availity Element Design System components.
/// </summary>
public class ComponentTools
{
    private readonly ElementDataService _dataService;

    public ComponentTools(ElementDataService dataService)
    {
        _dataService = dataService;
    }

    [McpServerTool]
    [Description("Lists all available components in the Availity Element Design System. Returns component IDs, names, and categories.")]
    public string ListComponents()
    {
        var components = _dataService.GetAllComponents()
            .Select(c => new { c.Id, c.Name, c.Category, c.Description })
            .ToList();

        return JsonSerializer.Serialize(components, new JsonSerializerOptions { WriteIndented = true });
    }

    [McpServerTool]
    [Description("Gets detailed information about a specific component including props, usage examples, and accessibility guidelines.")]
    public string GetComponent(
        [Description("The ID of the component (e.g., 'button', 'card', 'textfield')")] string componentId)
    {
        var component = _dataService.GetComponent(componentId);
        if (component == null)
        {
            return JsonSerializer.Serialize(new { error = $"Component '{componentId}' not found" });
        }

        return JsonSerializer.Serialize(component, new JsonSerializerOptions { WriteIndented = true });
    }

    [McpServerTool]
    [Description("Gets all components in a specific category (e.g., 'Inputs', 'Surfaces', 'Feedback', 'Data Display').")]
    public string GetComponentsByCategory(
        [Description("The category name (e.g., 'Inputs', 'Surfaces', 'Feedback')")] string category)
    {
        var components = _dataService.GetComponentsByCategory(category).ToList();
        
        if (!components.Any())
        {
            return JsonSerializer.Serialize(new { error = $"No components found in category '{category}'" });
        }

        return JsonSerializer.Serialize(components, new JsonSerializerOptions { WriteIndented = true });
    }

    [McpServerTool]
    [Description("Lists all available component categories in the Element Design System.")]
    public string ListComponentCategories()
    {
        var categories = _dataService.GetAllComponents()
            .Select(c => c.Category)
            .Distinct()
            .OrderBy(c => c)
            .ToList();

        return JsonSerializer.Serialize(categories, new JsonSerializerOptions { WriteIndented = true });
    }
}
