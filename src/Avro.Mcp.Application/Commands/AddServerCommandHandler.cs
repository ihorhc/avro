namespace Avro.Mcp.Application.Commands;

/// <summary>
/// Handler for AddServerCommand
/// </summary>
public class AddServerCommandHandler : IRequestHandler<AddServerCommand, Unit>
{
    private readonly IServerConfigurationRepository _repository;
    private readonly IServerConfigurationValidator _validator;
    private readonly ILogger<AddServerCommandHandler> _logger;

    public AddServerCommandHandler(
        IServerConfigurationRepository repository,
        IServerConfigurationValidator validator,
        ILogger<AddServerCommandHandler> logger)
    {
        _repository = repository;
        _validator = validator;
        _logger = logger;
    }

    public async Task<Unit> Handle(AddServerCommand request, CancellationToken cancellationToken)
    {
        var config = new ServerConfig
        {
            Name = request.Name,
            Command = request.Command,
            Arguments = request.Arguments,
            WorkingDirectory = request.WorkingDirectory,
            TimeoutSeconds = request.TimeoutSeconds,
            AutoStart = request.AutoStart
        };

        _validator.Validate(config);

        await _repository.AddOrUpdateAsync(config, cancellationToken);
        await _repository.SaveAsync(cancellationToken);

        _logger.LogInformation("Server {ServerName} added with command: {Command}", config.Name, config.Command);

        return Unit.Value;
    }
}
