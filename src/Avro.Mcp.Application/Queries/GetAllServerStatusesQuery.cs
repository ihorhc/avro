namespace Avro.Mcp.Application.Queries;

/// <summary>
/// Query to get all server statuses
/// </summary>
public record GetAllServerStatusesQuery : IRequest<IEnumerable<(string Name, bool Running, int? ProcessId, TimeSpan? Uptime)>>;
