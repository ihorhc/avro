namespace Avro.Mcp.Application.Queries;

/// <summary>
/// Handler for GetServerStatusQuery
/// </summary>
public class GetServerStatusQueryHandler : IRequestHandler<GetServerStatusQuery, (string Name, bool Running, int? ProcessId, TimeSpan? Uptime)?>
{
    private readonly IServerConfigurationRepository _repository;
    private readonly IServerRegistry _registry;

    public GetServerStatusQueryHandler(
        IServerConfigurationRepository repository,
        IServerRegistry registry)
    {
        _repository = repository;
        _registry = registry;
    }

    public async Task<(string Name, bool Running, int? ProcessId, TimeSpan? Uptime)?> Handle(
        GetServerStatusQuery request,
        CancellationToken cancellationToken)
    {
        var config = await _repository.GetAsync(request.ServerName, cancellationToken);
        if (config is null)
            return null;

        var manager = _registry.Get(request.ServerName);
        if (manager?.IsRunning == true)
        {
            return (request.ServerName, true, manager.ProcessId, manager.GetUptime());
        }

        return (request.ServerName, false, null, null);
    }
}
