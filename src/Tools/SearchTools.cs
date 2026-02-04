using System.ComponentModel;
using System.Text.Json;
using ElementMcpServer.Data;
using ModelContextProtocol.Server;

namespace ElementMcpServer.Tools;

/// <summary>
/// MCP tools for searching across all Element Design System documentation.
/// </summary>
public class SearchTools
{
    private readonly ElementDataService _dataService;

    public SearchTools(ElementDataService dataService)
    {
        _dataService = dataService;
    }

    [McpServerTool]
    [Description("Searches across all Element Design System documentation (components, foundations, patterns, and templates) for a given query.")]
    public string Search(
        [Description("The search query to find in documentation")] string query)
    {
        if (string.IsNullOrWhiteSpace(query))
        {
            return JsonSerializer.Serialize(new { error = "Search query cannot be empty" });
        }

        var results = _dataService.Search(query).ToList();
        
        if (!results.Any())
        {
            return JsonSerializer.Serialize(new { message = $"No results found for '{query}'" });
        }

        return JsonSerializer.Serialize(new 
        { 
            query = query,
            count = results.Count,
            results = results 
        }, new JsonSerializerOptions { WriteIndented = true });
    }
}
