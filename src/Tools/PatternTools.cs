using System.ComponentModel;
using System.Text.Json;
using ElementMcpServer.Data;
using ModelContextProtocol.Server;

namespace ElementMcpServer.Tools;

/// <summary>
/// MCP tools for querying Availity Element Design System patterns.
/// </summary>
public class PatternTools
{
    private readonly ElementDataService _dataService;

    public PatternTools(ElementDataService dataService)
    {
        _dataService = dataService;
    }

    [McpServerTool]
    [Description("Lists all available design patterns in the Availity Element Design System. Patterns are reusable solutions to common design problems.")]
    public string ListPatterns()
    {
        var patterns = _dataService.GetAllPatterns()
            .Select(p => new { p.Id, p.Name, p.Category, p.Description })
            .ToList();

        return JsonSerializer.Serialize(patterns, new JsonSerializerOptions { WriteIndented = true });
    }

    [McpServerTool]
    [Description("Gets detailed information about a specific pattern including the problem it solves, solution, best practices, and examples.")]
    public string GetPattern(
        [Description("The ID of the pattern (e.g., 'form-validation', 'data-loading', 'modal-workflow')")] string patternId)
    {
        var pattern = _dataService.GetPattern(patternId);
        if (pattern == null)
        {
            return JsonSerializer.Serialize(new { error = $"Pattern '{patternId}' not found" });
        }

        return JsonSerializer.Serialize(pattern, new JsonSerializerOptions { WriteIndented = true });
    }

    [McpServerTool]
    [Description("Gets all patterns in a specific category (e.g., 'Forms', 'Navigation', 'Feedback').")]
    public string GetPatternsByCategory(
        [Description("The category name")] string category)
    {
        var patterns = _dataService.GetPatternsByCategory(category).ToList();
        
        if (!patterns.Any())
        {
            return JsonSerializer.Serialize(new { error = $"No patterns found in category '{category}'" });
        }

        return JsonSerializer.Serialize(patterns, new JsonSerializerOptions { WriteIndented = true });
    }

    [McpServerTool]
    [Description("Lists all available pattern categories in the Element Design System.")]
    public string ListPatternCategories()
    {
        var categories = _dataService.GetAllPatterns()
            .Select(p => p.Category)
            .Distinct()
            .OrderBy(c => c)
            .ToList();

        return JsonSerializer.Serialize(categories, new JsonSerializerOptions { WriteIndented = true });
    }
}
