using System.Text.Json;
using ElementMcpServer.Data;
using ElementMcpServer.Services;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

Console.WriteLine("=== Element MCP Server - Enrichment Test ===\n");

// Set up services
var services = new ServiceCollection();
services.AddLogging(builder =>
{
    builder.AddConsole();
    builder.SetMinimumLevel(LogLevel.Information);
});
services.AddHttpClient<StorybookService>();
services.AddSingleton<StorybookService>();
services.AddSingleton<ElementDataService>();

var serviceProvider = services.BuildServiceProvider();
var dataService = serviceProvider.GetRequiredService<ElementDataService>();

Console.WriteLine("Waiting for enrichment to complete...");
await dataService.EnrichComponentsAsync();
Console.WriteLine("Enrichment completed.\n");

// Test Button component
Console.WriteLine("=== Button Component ===");
var button = dataService.GetComponent("button");
if (button != null)
{
    Console.WriteLine($"ID: {button.Id}");
    Console.WriteLine($"Name: {button.Name}");
    Console.WriteLine($"Category: {button.Category}");
    Console.WriteLine($"Storybook URL: {button.StorybookUrl}");
    Console.WriteLine($"Import Path: {button.ImportPath ?? "NOT ENRICHED"}");
    Console.WriteLine($"GitHub URL: {button.GitHubUrl ?? "NOT ENRICHED"}");
    Console.WriteLine($"GitHub Package URL: {button.GitHubPackageUrl ?? "NOT ENRICHED"}");
    
    if (!string.IsNullOrEmpty(button.GitHubUrl))
    {
        Console.WriteLine("\n✓ SUCCESS: Button component enriched!");
    }
    else
    {
        Console.WriteLine("\n✗ WARNING: Button component not enriched");
    }
}
else
{
    Console.WriteLine("✗ ERROR: Button component not found");
}

Console.WriteLine("\n=== TextField Component ===");
var textfield = dataService.GetComponent("textfield");
if (textfield != null)
{
    Console.WriteLine($"ID: {textfield.Id}");
    Console.WriteLine($"Name: {textfield.Name}");
    Console.WriteLine($"Storybook URL: {textfield.StorybookUrl}");
    Console.WriteLine($"Import Path: {textfield.ImportPath ?? "NOT ENRICHED"}");
    Console.WriteLine($"GitHub URL: {textfield.GitHubUrl ?? "NOT ENRICHED"}");
}

Console.WriteLine("\n=== Autocomplete Component ===");
var autocomplete = dataService.GetComponent("autocomplete");
if (autocomplete != null)
{
    Console.WriteLine($"ID: {autocomplete.Id}");
    Console.WriteLine($"Name: {autocomplete.Name}");
    Console.WriteLine($"Storybook URL: {autocomplete.StorybookUrl}");
    Console.WriteLine($"Import Path: {autocomplete.ImportPath ?? "NOT ENRICHED"}");
    Console.WriteLine($"GitHub URL: {autocomplete.GitHubUrl ?? "NOT ENRICHED"}");
}

// Count enriched components
var allComponents = dataService.GetAllComponents().ToList();
var enrichedCount = allComponents.Count(c => !string.IsNullOrEmpty(c.GitHubUrl));
Console.WriteLine($"\n=== Summary ===");
Console.WriteLine($"Total Components: {allComponents.Count}");
Console.WriteLine($"Enriched Components: {enrichedCount}");
Console.WriteLine($"Enrichment Rate: {(double)enrichedCount / allComponents.Count * 100:F1}%");
