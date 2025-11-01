namespace Avro.Mcp.Application.Queries;

/// <summary>
/// Handler for GetAllServersQuery
/// </summary>
public class GetAllServersQueryHandler : IRequestHandler<GetAllServersQuery, IEnumerable<ServerConfig>>
{
    private readonly IServerConfigurationRepository _repository;

    public GetAllServersQueryHandler(IServerConfigurationRepository repository)
    {
        _repository = repository;
    }

    public async Task<IEnumerable<ServerConfig>> Handle(GetAllServersQuery request, CancellationToken cancellationToken)
    {
        return await _repository.GetAllAsync(cancellationToken);
    }
}
