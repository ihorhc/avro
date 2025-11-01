namespace Avro.Mcp.Application.Commands;

/// <summary>
/// Command to start a server
/// </summary>
public record StartServerCommand(string ServerName) : IRequest<Unit>;
