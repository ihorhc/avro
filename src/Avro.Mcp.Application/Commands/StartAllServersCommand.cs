namespace Avro.Mcp.Application.Commands;

/// <summary>
/// Command to start all auto-start servers
/// </summary>
public record StartAllServersCommand : IRequest<Unit>;
