namespace Avro.Mcp.Application.Commands;

/// <summary>
/// Handler for StopAllServersCommand
/// </summary>
public class StopAllServersCommandHandler : IRequestHandler<StopAllServersCommand, Unit>
{
    private readonly IServerRegistry _registry;
    private readonly IMediator _mediator;
    private readonly ILogger<StopAllServersCommandHandler> _logger;

    public StopAllServersCommandHandler(
        IServerRegistry registry,
        IMediator mediator,
        ILogger<StopAllServersCommandHandler> logger)
    {
        _registry = registry;
        _mediator = mediator;
        _logger = logger;
    }

    public async Task<Unit> Handle(StopAllServersCommand request, CancellationToken cancellationToken)
    {
        var runningServers = _registry.GetAll().Where(x => x.Manager.IsRunning).ToList();

        if (!runningServers.Any())
        {
            _logger.LogInformation("No servers running");
            return Unit.Value;
        }

        _logger.LogInformation("Stopping {Count} server(s)", runningServers.Count);

        foreach (var (serverName, _) in runningServers)
        {
            await _mediator.Send(new StopServerCommand(serverName), cancellationToken);
        }

        return Unit.Value;
    }
}
