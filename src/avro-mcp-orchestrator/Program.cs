using System.CommandLine;
using Avro.Mcp.Orchestrator;
using Avro.Mcp.Orchestrator.Infrastructure;
using Avro.Mcp.Orchestrator.Presentation;
using Avro.Mcp.Orchestrator.Services;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Serilog;

var (services, logger) = ConfigureServices();

var configPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile), ".avro", "mcp-config.json");
var repository = new JsonServerConfigurationRepository(configPath, services.GetRequiredService<ILogger<JsonServerConfigurationRepository>>());
var validator = new ServerConfigurationValidator();
var registry = new InMemoryServerRegistry();
var orchestrationService = new ServerOrchestrationService(repository, validator, registry, services.GetRequiredService<ILogger<ServerOrchestrationService>>());
var presenter = new ConsolePresenter();

// Command arguments and options
var serverAddNameArg = new Argument<string>("name", "Server name");
var serverAddCommandArg = new Argument<string>("command", "Command to execute");
var argsOption = new Option<string?>(new[] { "-a", "--args" }, "Command arguments");
var workDirOption = new Option<string?>(new[] { "-w", "--working-dir" }, "Working directory");
var timeoutOption = new Option<int>(new[] { "-t", "--timeout" }, () => 30, "Timeout in seconds");

// Server add command
var serverAdd = new Command("add", "Add server") { serverAddNameArg, serverAddCommandArg, argsOption, workDirOption, timeoutOption };
serverAdd.SetHandler(async (name, command, args, workDir, timeout) =>
{
    var config = new ServerConfig 
    { 
        Name = name, 
        Command = command, 
        Arguments = args, 
        WorkingDirectory = workDir, 
        TimeoutSeconds = timeout 
    };
    
    try
    {
        await orchestrationService.AddServerAsync(config);
        presenter.PresentServerAddedSuccess(name);
    }
    catch (Exception ex)
    {
        presenter.PresentError(ex.Message);
    }
}, serverAddNameArg, serverAddCommandArg, argsOption, workDirOption, timeoutOption);

var server = new Command("server", "Manage servers") { serverAdd };

// List command
var list = new Command("list", "List servers");
list.SetHandler(async () =>
{
    var configs = await orchestrationService.GetAllConfigurationsAsync();
    if (!configs.Any())
    {
        presenter.PresentNoServersConfigured();
        return;
    }
    
    var servers = new List<(string, string, bool, bool)>();
    foreach (var config in configs.OrderBy(x => x.Name))
    {
        var status = await orchestrationService.GetServerStatusAsync(config.Name);
        servers.Add((config.Name, config.Command, status?.Running ?? false, config.AutoStart));
    }
    
    presenter.PresentServersList(servers);
});

// Start command
var startNameArg = new Argument<string?>("name", () => null, "Server name");
var start = new Command("start", "Start") { startNameArg };
start.SetHandler(async (name) =>
{
    if (string.IsNullOrEmpty(name)) 
    {
        await orchestrationService.StartAllServersAsync();
    }
    else 
    {
        presenter.PresentServerStarting(name);
        await orchestrationService.StartServerAsync(name);
        var status = await orchestrationService.GetServerStatusAsync(name);
        if (status?.Running == true)
        {
            presenter.PresentServerStartedSuccess(name, status.Value.ProcessId ?? 0);
        }
    }
}, startNameArg);

// Stop command
var stopNameArg = new Argument<string?>("name", () => null, "Server name");
var stop = new Command("stop", "Stop") { stopNameArg };
stop.SetHandler(async (name) =>
{
    if (string.IsNullOrEmpty(name)) 
    {
        await orchestrationService.StopAllServersAsync();
    }
    else 
    {
        presenter.PresentServerStopping(name);
        await orchestrationService.StopServerAsync(name);
        presenter.PresentServerStoppedSuccess(name);
    }
}, stopNameArg);

// Status command
var status = new Command("status", "Show server status");
status.SetHandler(async () =>
{
    var statuses = await orchestrationService.GetAllStatusesAsync();
    if (!statuses.Any())
    {
        presenter.PresentNoServersConfigured();
        return;
    }
    presenter.PresentServerStatus(statuses);
});

// Config show command
var configShow = new Command("show", "Show config");
configShow.SetHandler(async () =>
{
    var configs = await orchestrationService.GetAllConfigurationsAsync();
    var statuses = await orchestrationService.GetAllStatusesAsync();
    var runningCount = statuses.Count(s => s.Running);
    
    presenter.PresentConfiguration(configPath, configs.Count(), runningCount, configs.Select(c => (c.Name, c)));
});

var config = new Command("config", "Configuration") { configShow };

// Root command
var root = new RootCommand("Avro MCP Orchestrator") { server, list, start, stop, status, config };
return await root.InvokeAsync(args);

(ServiceProvider, ILogger<Program>) ConfigureServices()
{
    var services = new ServiceCollection();
    
    services.AddLogging(loggingBuilder =>
    {
        Log.Logger = new LoggerConfiguration()
            .MinimumLevel.Information()
            .WriteTo.Console()
            .WriteTo.File("logs/mcp-orchestrator-.txt", rollingInterval: Serilog.RollingInterval.Day)
            .CreateLogger();

        loggingBuilder.AddSerilog(Log.Logger);
    });

    var provider = services.BuildServiceProvider();
    var logger = provider.GetRequiredService<ILogger<Program>>();
    
    return (provider, logger);
}
