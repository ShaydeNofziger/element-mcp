using ElementMcpServer.Data;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace ElementMcpServer.Services;

/// <summary>
/// Background service that enriches component data during application startup.
/// </summary>
public class DataEnrichmentService : IHostedService
{
    private readonly ElementDataService _dataService;
    private readonly ILogger<DataEnrichmentService> _logger;

    public DataEnrichmentService(ElementDataService dataService, ILogger<DataEnrichmentService> logger)
    {
        _dataService = dataService;
        _logger = logger;
    }

    public async Task StartAsync(CancellationToken cancellationToken)
    {
        _logger.LogInformation("Starting data enrichment service");
        
        try
        {
            await _dataService.EnrichComponentsAsync();
            _logger.LogInformation("Data enrichment completed successfully");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to enrich component data");
            // Don't throw - let the service start even if enrichment fails
        }
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        _logger.LogInformation("Data enrichment service stopped");
        return Task.CompletedTask;
    }
}
