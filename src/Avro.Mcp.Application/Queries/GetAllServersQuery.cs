namespace Avro.Mcp.Application.Queries;

/// <summary>
/// Query to get all server configurations
/// </summary>
public record GetAllServersQuery : IRequest<IEnumerable<ServerConfig>>;
