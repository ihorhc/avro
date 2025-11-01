using Avro.Mcp.Orchestrator;

var (services, logger) = ServiceConfiguration.ConfigureServices();

try
{
    ((Microsoft.Extensions.Logging.ILogger)logger).LogInformation("Starting Avro MCP Orchestrator");

    var mediator = services.GetRequiredService<MediatR.IMediator>();
    var presenter = services.GetRequiredService<ConsolePresenter>();

    var root = CommandSetup.CreateRootCommand(mediator, presenter);
    return await root.InvokeAsync(args);
}
catch (Exception ex)
{
    ((Microsoft.Extensions.Logging.ILogger)logger).LogError(ex, "Unhandled exception occurred");
    return 1;
}
finally
{
    Serilog.Log.CloseAndFlush();
    services.Dispose();
}
