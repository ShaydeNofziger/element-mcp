using System.ComponentModel;
using System.Text.Json;
using ElementMcpServer.Data;
using ModelContextProtocol.Server;

namespace ElementMcpServer.Tools;

/// <summary>
/// MCP tools for querying Availity Element Design System templates.
/// </summary>
public class TemplateTools
{
    private readonly ElementDataService _dataService;

    public TemplateTools(ElementDataService dataService)
    {
        _dataService = dataService;
    }

    [McpServerTool]
    [Description("Lists all available templates in the Availity Element Design System. Templates are pre-built layouts combining patterns and components.")]
    public string ListTemplates()
    {
        var templates = _dataService.GetAllTemplates()
            .Select(t => new { t.Id, t.Name, t.Type, t.Description })
            .ToList();

        return JsonSerializer.Serialize(templates, new JsonSerializerOptions { WriteIndented = true });
    }

    [McpServerTool]
    [Description("Gets detailed information about a specific template including use cases, customization options, and examples.")]
    public string GetTemplate(
        [Description("The ID of the template (e.g., 'dashboard', 'form-page', 'list-view')")] string templateId)
    {
        var template = _dataService.GetTemplate(templateId);
        if (template == null)
        {
            return JsonSerializer.Serialize(new { error = $"Template '{templateId}' not found" });
        }

        return JsonSerializer.Serialize(template, new JsonSerializerOptions { WriteIndented = true });
    }

    [McpServerTool]
    [Description("Gets all templates of a specific type (e.g., 'Page Layout', 'Form', 'Data Display').")]
    public string GetTemplatesByType(
        [Description("The type of templates to retrieve")] string type)
    {
        var templates = _dataService.GetTemplatesByType(type).ToList();
        
        if (!templates.Any())
        {
            return JsonSerializer.Serialize(new { error = $"No templates found of type '{type}'" });
        }

        return JsonSerializer.Serialize(templates, new JsonSerializerOptions { WriteIndented = true });
    }

    [McpServerTool]
    [Description("Lists all available template types in the Element Design System.")]
    public string ListTemplateTypes()
    {
        var types = _dataService.GetAllTemplates()
            .Select(t => t.Type)
            .Distinct()
            .OrderBy(t => t)
            .ToList();

        return JsonSerializer.Serialize(types, new JsonSerializerOptions { WriteIndented = true });
    }
}
