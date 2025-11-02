namespace Avro.Mcp.Orchestrator;

/// <summary>
/// Service configuration
/// </summary>
public static class ServiceConfiguration
{
    /// <summary>
    /// Configures core services
    /// </summary>
    public static (ServiceProvider, object) ConfigureServices()
    {
        var services = new ServiceCollection();

        services.AddLogging(loggingBuilder =>
        {
            Log.Logger = new LoggerConfiguration()
                .MinimumLevel.Information()
                .WriteTo.Console()
                .WriteTo.File("logs/mcp-orchestrator-.txt", rollingInterval: RollingInterval.Day)
                .CreateLogger();

            loggingBuilder.AddSerilog(Log.Logger);
        });

        // Register infrastructure services
        var configPath = Path.Combine(
            Environment.GetFolderPath(Environment.SpecialFolder.UserProfile),
            ".avro", "mcp-config.json");

        services.AddSingleton<IServerConfigurationRepository>(
            new JsonServerConfigurationRepository(configPath));
        services.AddSingleton<IServerConfigurationValidator, ServerConfigurationValidator>();
        services.AddSingleton<IServerRegistry, InMemoryServerRegistry>();

        // Register application services
        services.AddApplicationServices();

        // Register presentation
        services.AddSingleton<ConsolePresenter>();

        var provider = services.BuildServiceProvider();
        var loggerFactory = provider.GetRequiredService<ILoggerFactory>();
        var logger = loggerFactory.CreateLogger("Orchestrator");

        return (provider, logger);
    }
}
