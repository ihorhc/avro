namespace Avro.Mcp.Application.Queries;

/// <summary>
/// Handler for GetAllServersQuery
/// </summary>
public class GetAllServersQueryHandler(IServerConfigurationRepository repository) : IRequestHandler<GetAllServersQuery, IEnumerable<ServerConfig>>
{
    public async Task<IEnumerable<ServerConfig>> Handle(GetAllServersQuery request, CancellationToken cancellationToken)
    {
        return await repository.GetAllAsync(cancellationToken);
    }
}
