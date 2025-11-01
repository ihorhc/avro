namespace Avro.Mcp.Orchestrator;

/// <summary>
/// Configuration for an MCP server
/// </summary>
public record ServerConfig
{
    /// <summary>
    /// Name identifier for the server
    /// </summary>
    public required string Name { get; init; }

    /// <summary>
    /// Command to execute the server
    /// </summary>
    public required string Command { get; init; }

    /// <summary>
    /// Optional command arguments
    /// </summary>
    public string? Arguments { get; init; }

    /// <summary>
    /// Working directory for the server
    /// </summary>
    public string? WorkingDirectory { get; init; }

    /// <summary>
    /// Server timeout in seconds
    /// </summary>
    public int TimeoutSeconds { get; init; } = 30;

    /// <summary>
    /// Whether the server auto-starts
    /// </summary>
    public bool AutoStart { get; init; }

    /// <summary>
    /// Environment variables for the server
    /// </summary>
    public Dictionary<string, string> Environment { get; init; } = new();
}

/// <summary>
/// Orchestrator options
/// </summary>
public record OrchestratorOptions
{
    public bool Verbose { get; init; }
    public string? ConfigPath { get; init; }
}
