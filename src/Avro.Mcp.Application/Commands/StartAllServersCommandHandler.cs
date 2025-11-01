namespace Avro.Mcp.Application.Commands;

/// <summary>
/// Handler for StartAllServersCommand
/// </summary>
public class StartAllServersCommandHandler : IRequestHandler<StartAllServersCommand, Unit>
{
    private readonly IServerConfigurationRepository _repository;
    private readonly IMediator _mediator;
    private readonly ILogger<StartAllServersCommandHandler> _logger;

    public StartAllServersCommandHandler(
        IServerConfigurationRepository repository,
        IMediator mediator,
        ILogger<StartAllServersCommandHandler> logger)
    {
        _repository = repository;
        _mediator = mediator;
        _logger = logger;
    }

    public async Task<Unit> Handle(StartAllServersCommand request, CancellationToken cancellationToken)
    {
        var autoStartServers = (await _repository.GetAllAsync(cancellationToken))
            .Where(c => c.AutoStart)
            .ToList();

        if (!autoStartServers.Any())
        {
            _logger.LogInformation("No servers configured for auto-start");
            return Unit.Value;
        }

        _logger.LogInformation("Starting {Count} auto-start server(s)", autoStartServers.Count);

        foreach (var config in autoStartServers)
        {
            await _mediator.Send(new StartServerCommand(config.Name), cancellationToken);
        }

        return Unit.Value;
    }
}
