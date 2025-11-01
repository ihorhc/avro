namespace Avro.Mcp.Application.Queries;

/// <summary>
/// Handler for GetAllServerStatusesQuery
/// </summary>
public class GetAllServerStatusesQueryHandler : IRequestHandler<GetAllServerStatusesQuery, IEnumerable<(string Name, bool Running, int? ProcessId, TimeSpan? Uptime)>>
{
    private readonly IServerConfigurationRepository _repository;
    private readonly IServerRegistry _registry;

    public GetAllServerStatusesQueryHandler(
        IServerConfigurationRepository repository,
        IServerRegistry registry)
    {
        _repository = repository;
        _registry = registry;
    }

    public async Task<IEnumerable<(string Name, bool Running, int? ProcessId, TimeSpan? Uptime)>> Handle(
        GetAllServerStatusesQuery request,
        CancellationToken cancellationToken)
    {
        var configs = await _repository.GetAllAsync(cancellationToken);
        var statuses = new List<(string, bool, int?, TimeSpan?)>();

        foreach (var config in configs.OrderBy(x => x.Name))
        {
            var manager = _registry.Get(config.Name);
            if (manager?.IsRunning == true)
            {
                statuses.Add((config.Name, true, manager.ProcessId, manager.GetUptime()));
            }
            else
            {
                statuses.Add((config.Name, false, null, null));
            }
        }

        return statuses;
    }
}
