namespace Avro.Mcp.Application.Commands;

/// <summary>
/// Command to stop a server
/// </summary>
public record StopServerCommand(string ServerName) : IRequest<Unit>;
