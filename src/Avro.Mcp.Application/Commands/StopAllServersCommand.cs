namespace Avro.Mcp.Application.Commands;

/// <summary>
/// Command to stop all running servers
/// </summary>
public record StopAllServersCommand : IRequest<Unit>;
