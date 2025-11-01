namespace Avro.Mcp.Application.Queries;

/// <summary>
/// Query to get a server status by name
/// </summary>
public record GetServerStatusQuery(string ServerName) : IRequest<(string Name, bool Running, int? ProcessId, TimeSpan? Uptime)?>;
