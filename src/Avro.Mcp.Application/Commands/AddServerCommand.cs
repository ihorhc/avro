namespace Avro.Mcp.Application.Commands;

/// <summary>
/// Command to add a new server configuration
/// </summary>
public record AddServerCommand : IRequest<Unit>
{
    /// <summary>
    /// Server name
    /// </summary>
    public required string Name { get; init; }

    /// <summary>
    /// Command to execute
    /// </summary>
    public required string Command { get; init; }

    /// <summary>
    /// Optional command arguments
    /// </summary>
    public string? Arguments { get; init; }

    /// <summary>
    /// Optional working directory
    /// </summary>
    public string? WorkingDirectory { get; init; }

    /// <summary>
    /// Timeout in seconds
    /// </summary>
    public int TimeoutSeconds { get; init; } = 30;

    /// <summary>
    /// Whether to auto-start the server
    /// </summary>
    public bool AutoStart { get; init; }
}
