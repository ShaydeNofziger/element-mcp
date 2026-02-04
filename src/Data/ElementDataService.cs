using System.Text.Json;
using ElementMcpServer.Models;
using ElementMcpServer.Services;
using Microsoft.Extensions.Logging;

namespace ElementMcpServer.Data;

/// <summary>
/// Provides access to Availity Element Design System documentation data.
/// Data is loaded from a JSON file containing comprehensive component information,
/// enriched with dynamic data from the Storybook index.
/// </summary>
public class ElementDataService
{
    private readonly List<Component> _components;
    private readonly List<Foundation> _foundations;
    private readonly List<Pattern> _patterns;
    private readonly List<Template> _templates;
    private readonly StorybookService _storybookService;
    private readonly ILogger<ElementDataService> _logger;
    private bool _enrichmentCompleted;

    public ElementDataService(StorybookService storybookService, ILogger<ElementDataService> logger)
    {
        _storybookService = storybookService;
        _logger = logger;
        
        var jsonData = LoadJsonData();
        _components = jsonData.Components;
        _foundations = jsonData.Foundations;
        _patterns = jsonData.Patterns;
        _templates = jsonData.Templates;
    }

    /// <summary>
    /// Enriches component data with information from Storybook index.
    /// This method should be called once during application startup.
    /// </summary>
    public async Task EnrichComponentsAsync()
    {
        if (_enrichmentCompleted)
        {
            _logger.LogInformation("Component enrichment already completed, skipping");
            return;
        }

        try
        {
            _logger.LogInformation("Starting component enrichment with Storybook data");
            
            var enrichedComponents = new List<Component>();
            
            foreach (var component in _components)
            {
                try
                {
                    var storybookEntry = await _storybookService.FindComponentEntryAsync(component.Name, component.StorybookUrl);
                    
                    if (storybookEntry?.ImportPath != null)
                    {
                        var changelogUrl = _storybookService.ConvertImportPathToChangelogUrl(storybookEntry.ImportPath);
                        var packageUrl = _storybookService.GetGitHubPackageUrl(storybookEntry.ImportPath);
                        var packageImport = _storybookService.GeneratePackageImport(component.Name, storybookEntry.ImportPath);
                        var storybookIntroUrl = _storybookService.GenerateStorybookIntroductionUrl(storybookEntry.Id);
                        
                        var enrichedComponent = component with
                        {
                            PackageImport = packageImport,
                            GitHubChangelogUrl = changelogUrl,
                            GitHubPackageUrl = packageUrl,
                            StorybookUrl = storybookIntroUrl ?? component.StorybookUrl // Use new URL or keep original
                        };
                        
                        enrichedComponents.Add(enrichedComponent);
                        _logger.LogDebug("Enriched component {Name} with GitHub URLs", component.Name);
                    }
                    else
                    {
                        enrichedComponents.Add(component);
                        _logger.LogDebug("No Storybook entry found for component {Name}", component.Name);
                    }
                }
                catch (Exception ex)
                {
                    _logger.LogWarning(ex, "Failed to enrich component {Name}", component.Name);
                    enrichedComponents.Add(component);
                }
            }
            
            // Replace the components list with enriched versions
            _components.Clear();
            _components.AddRange(enrichedComponents);
            
            _enrichmentCompleted = true;
            _logger.LogInformation("Component enrichment completed. {Count} components enriched", 
                enrichedComponents.Count(c => c.GitHubChangelogUrl != null));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to enrich components with Storybook data");
        }
    }

    public IEnumerable<Component> GetAllComponents() => _components;
    public Component? GetComponent(string id) => _components.FirstOrDefault(c => c.Id.Equals(id, StringComparison.OrdinalIgnoreCase));
    public IEnumerable<Component> GetComponentsByCategory(string category) => 
        _components.Where(c => c.Category.Equals(category, StringComparison.OrdinalIgnoreCase));

    public IEnumerable<Foundation> GetAllFoundations() => _foundations;
    public Foundation? GetFoundation(string id) => _foundations.FirstOrDefault(f => f.Id.Equals(id, StringComparison.OrdinalIgnoreCase));
    public IEnumerable<Foundation> GetFoundationsByType(string type) => 
        _foundations.Where(f => f.Type.Equals(type, StringComparison.OrdinalIgnoreCase));

    public IEnumerable<Pattern> GetAllPatterns() => _patterns;
    public Pattern? GetPattern(string id) => _patterns.FirstOrDefault(p => p.Id.Equals(id, StringComparison.OrdinalIgnoreCase));
    public IEnumerable<Pattern> GetPatternsByCategory(string category) => 
        _patterns.Where(p => p.Category.Equals(category, StringComparison.OrdinalIgnoreCase));

    public IEnumerable<Template> GetAllTemplates() => _templates;
    public Template? GetTemplate(string id) => _templates.FirstOrDefault(t => t.Id.Equals(id, StringComparison.OrdinalIgnoreCase));
    public IEnumerable<Template> GetTemplatesByType(string type) => 
        _templates.Where(t => t.Type.Equals(type, StringComparison.OrdinalIgnoreCase));

    public IEnumerable<object> Search(string query)
    {
        var results = new List<object>();

        results.AddRange(_components.Where(c => 
            c.Name.Contains(query, StringComparison.OrdinalIgnoreCase) || 
            c.Description.Contains(query, StringComparison.OrdinalIgnoreCase)));

        results.AddRange(_foundations.Where(f => 
            f.Name.Contains(query, StringComparison.OrdinalIgnoreCase) || 
            f.Description.Contains(query, StringComparison.OrdinalIgnoreCase)));

        results.AddRange(_patterns.Where(p => 
            p.Name.Contains(query, StringComparison.OrdinalIgnoreCase) || 
            p.Description.Contains(query, StringComparison.OrdinalIgnoreCase)));

        results.AddRange(_templates.Where(t => 
            t.Name.Contains(query, StringComparison.OrdinalIgnoreCase) || 
            t.Description.Contains(query, StringComparison.OrdinalIgnoreCase)));

        return results;
    }

    private ElementData LoadJsonData()
    {
        try
        {
            // Get the directory where the application is located
            var appDirectory = AppContext.BaseDirectory;
            
            // Look for the JSON file in the Data directory relative to the app
            var jsonPath = Path.Combine(appDirectory, "Data", "element-data.json");
            
            // If not found, try relative to current directory (for dotnet run scenarios)
            if (!File.Exists(jsonPath))
            {
                jsonPath = Path.Combine(Directory.GetCurrentDirectory(), "Data", "element-data.json");
            }
            
            if (!File.Exists(jsonPath))
            {
                throw new FileNotFoundException($"Could not find element-data.json. Searched: {jsonPath} and {Path.Combine(Directory.GetCurrentDirectory(), "Data", "element-data.json")}");
            }

            var jsonContent = File.ReadAllText(jsonPath);
            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };
            
            var data = JsonSerializer.Deserialize<ElementData>(jsonContent, options);
            return data ?? throw new InvalidOperationException("Failed to deserialize JSON data");
        }
        catch (Exception ex)
        {
            throw new InvalidOperationException($"Failed to load Element data: {ex.Message}", ex);
        }
    }

    private class ElementData
    {
        public List<Component> Components { get; set; } = new();
        public List<Foundation> Foundations { get; set; } = new();
        public List<Pattern> Patterns { get; set; } = new();
        public List<Template> Templates { get; set; } = new();
    }
}
