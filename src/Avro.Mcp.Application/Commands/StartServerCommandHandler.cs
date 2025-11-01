namespace Avro.Mcp.Application.Commands;

/// <summary>
/// Handler for StartServerCommand
/// </summary>
public class StartServerCommandHandler : IRequestHandler<StartServerCommand, Unit>
{
    private readonly IServerConfigurationRepository _repository;
    private readonly IServerRegistry _registry;
    private readonly ILogger<StartServerCommandHandler> _logger;

    public StartServerCommandHandler(
        IServerConfigurationRepository repository,
        IServerRegistry registry,
        ILogger<StartServerCommandHandler> logger)
    {
        _repository = repository;
        _registry = registry;
        _logger = logger;
    }

    public async Task<Unit> Handle(StartServerCommand request, CancellationToken cancellationToken)
    {
        if (_registry.IsRunning(request.ServerName))
        {
            _logger.LogInformation("Server {ServerName} is already running", request.ServerName);
            return Unit.Value;
        }

        var config = await _repository.GetAsync(request.ServerName, cancellationToken);
        if (config is null)
        {
            _logger.LogWarning("Server {ServerName} not found", request.ServerName);
            return Unit.Value;
        }

        var manager = new ProcessServerManager(request.ServerName, _logger as ILogger<ProcessServerManager>);
        await manager.StartAsync(config, cancellationToken);
        _registry.Register(request.ServerName, manager);

        _logger.LogInformation("Server {ServerName} started with PID {ProcessId}", request.ServerName, manager.ProcessId);

        return Unit.Value;
    }
}
