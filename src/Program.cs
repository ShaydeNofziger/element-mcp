using ElementMcpServer.Data;
using ElementMcpServer.Tools;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

var builder = Host.CreateApplicationBuilder(args);

// Configure all logs to go to stderr (stdout is used for the MCP protocol messages).
builder.Logging.AddConsole(o => o.LogToStandardErrorThreshold = LogLevel.Trace);

// Register the Element Data Service as a singleton
builder.Services.AddSingleton<ElementDataService>();

// Add the MCP services: the transport to use (stdio) and the tools to register.
builder.Services
    .AddMcpServer()
    .WithStdioServerTransport()
    .WithTools<ComponentTools>()
    .WithTools<FoundationTools>()
    .WithTools<PatternTools>()
    .WithTools<TemplateTools>()
    .WithTools<SearchTools>();

await builder.Build().RunAsync();
