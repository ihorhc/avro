namespace Avro.Mcp.Application.Commands;

/// <summary>
/// Handler for StopServerCommand
/// </summary>
public class StopServerCommandHandler : IRequestHandler<StopServerCommand, Unit>
{
    private readonly IServerRegistry _registry;
    private readonly ILogger<StopServerCommandHandler> _logger;

    public StopServerCommandHandler(
        IServerRegistry registry,
        ILogger<StopServerCommandHandler> logger)
    {
        _registry = registry;
        _logger = logger;
    }

    public async Task<Unit> Handle(StopServerCommand request, CancellationToken cancellationToken)
    {
        var manager = _registry.Get(request.ServerName);
        if (manager is null)
        {
            _logger.LogWarning("Server {ServerName} is not running", request.ServerName);
            return Unit.Value;
        }

        await manager.StopAsync(5000, cancellationToken);
        _registry.Unregister(request.ServerName);

        _logger.LogInformation("Server {ServerName} stopped", request.ServerName);

        return Unit.Value;
    }
}
