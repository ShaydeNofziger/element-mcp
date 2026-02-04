using System.Text.Json;
using Microsoft.Extensions.Logging;

namespace ElementMcpServer.Services;

/// <summary>
/// Service for fetching and parsing Availity Element Storybook index data.
/// </summary>
public class StorybookService
{
    private const string StorybookIndexUrl = "https://availity.github.io/element/index.json";
    private const string GitHubBaseUrl = "https://github.com/Availity/element/tree/main";
    
    private readonly HttpClient _httpClient;
    private readonly ILogger<StorybookService> _logger;
    private Dictionary<string, StorybookEntry>? _cachedEntries;

    public StorybookService(HttpClient httpClient, ILogger<StorybookService> logger)
    {
        _httpClient = httpClient;
        _logger = logger;
    }

    /// <summary>
    /// Fetches the Storybook index and returns component entries.
    /// </summary>
    public async Task<Dictionary<string, StorybookEntry>> GetStorybookEntriesAsync()
    {
        // Return cached entries if available
        if (_cachedEntries != null)
        {
            return _cachedEntries;
        }

        try
        {
            _logger.LogInformation("Fetching Storybook index from {Url}", StorybookIndexUrl);
            
            var response = await _httpClient.GetStringAsync(StorybookIndexUrl);
            var storybookIndex = JsonSerializer.Deserialize<StorybookIndex>(response, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });

            if (storybookIndex?.Entries == null)
            {
                _logger.LogWarning("No entries found in Storybook index");
                return new Dictionary<string, StorybookEntry>();
            }

            _cachedEntries = storybookIndex.Entries;
            _logger.LogInformation("Successfully loaded {Count} Storybook entries", _cachedEntries.Count);
            
            return _cachedEntries;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to fetch Storybook index from {Url}", StorybookIndexUrl);
            return new Dictionary<string, StorybookEntry>();
        }
    }

    /// <summary>
    /// Finds a Storybook entry matching the component name.
    /// </summary>
    public async Task<StorybookEntry?> FindComponentEntryAsync(string componentName, string? storybookUrl = null)
    {
        var entries = await GetStorybookEntriesAsync();
        
        // If we have a Storybook URL, try to extract the story ID from it
        if (!string.IsNullOrEmpty(storybookUrl))
        {
            var storyId = ExtractStoryIdFromUrl(storybookUrl);
            if (!string.IsNullOrEmpty(storyId))
            {
                // Look for introduction docs entry
                var introId = storyId.Replace("--docs", "-introduction--docs");
                if (entries.TryGetValue(introId, out var entry))
                {
                    return entry;
                }
                
                // Try without the replacement
                if (entries.TryGetValue(storyId, out var directEntry))
                {
                    return directEntry;
                }
            }
        }

        // Fallback: Search by component name
        var normalizedName = componentName.Replace(" ", "").ToLowerInvariant();
        
        // Try to find an introduction entry for this component
        var matchingEntry = entries.FirstOrDefault(e => 
            e.Key.Contains(normalizedName, StringComparison.OrdinalIgnoreCase) &&
            e.Key.EndsWith("introduction--docs", StringComparison.OrdinalIgnoreCase));
        
        return matchingEntry.Value;
    }

    /// <summary>
    /// Converts a Storybook import path to a GitHub URL.
    /// </summary>
    public string ConvertImportPathToGitHubUrl(string importPath)
    {
        // Remove leading "./" from path
        var cleanPath = importPath.TrimStart('.', '/');
        return $"{GitHubBaseUrl}/{cleanPath}";
    }

    /// <summary>
    /// Extracts the package path from an import path and converts it to a GitHub URL.
    /// </summary>
    public string? GetGitHubPackageUrl(string importPath)
    {
        // Remove leading "./" from path
        var cleanPath = importPath.TrimStart('.', '/');
        
        // Extract the package directory (e.g., "packages/button")
        var parts = cleanPath.Split('/');
        if (parts.Length >= 2 && parts[0] == "packages")
        {
            var packagePath = $"{parts[0]}/{parts[1]}";
            return $"{GitHubBaseUrl}/{packagePath}";
        }
        
        return null;
    }

    private string? ExtractStoryIdFromUrl(string storybookUrl)
    {
        // Example: "https://availity.github.io/element/?path=/docs/components-button--docs"
        // Extract: "components-button--docs"
        var pathPrefix = "path=/docs/";
        var pathIndex = storybookUrl.IndexOf(pathPrefix, StringComparison.OrdinalIgnoreCase);
        
        if (pathIndex < 0)
        {
            return null;
        }
        
        var storyId = storybookUrl[(pathIndex + pathPrefix.Length)..];
        
        // Remove any query parameters
        var queryIndex = storyId.IndexOf('&');
        if (queryIndex >= 0)
        {
            storyId = storyId[..queryIndex];
        }
        
        return storyId;
    }
}

/// <summary>
/// Represents the Storybook index.json structure.
/// </summary>
public class StorybookIndex
{
    public int V { get; set; }
    public Dictionary<string, StorybookEntry>? Entries { get; set; }
}

/// <summary>
/// Represents a single entry in the Storybook index.
/// </summary>
public class StorybookEntry
{
    public string? Id { get; set; }
    public string? Title { get; set; }
    public string? Name { get; set; }
    public string? ImportPath { get; set; }
    public string? Type { get; set; }
    public List<string>? Tags { get; set; }
}
