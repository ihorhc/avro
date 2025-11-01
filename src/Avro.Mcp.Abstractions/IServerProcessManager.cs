namespace Avro.Mcp.Abstractions;

/// <summary>
/// Interface for managing server process lifecycle
/// </summary>
public interface IServerProcessManager
{
    /// <summary>
    /// Gets the process ID
    /// </summary>
    int? ProcessId { get; }

    /// <summary>
    /// Gets whether the server is running
    /// </summary>
    bool IsRunning { get; }

    /// <summary>
    /// Starts the server process
    /// </summary>
    Task StartAsync(ServerConfig config, CancellationToken cancellationToken = default);

    /// <summary>
    /// Stops the server process
    /// </summary>
    Task StopAsync(int timeoutMilliseconds = 5000, CancellationToken cancellationToken = default);

    /// <summary>
    /// Gets the uptime of the server
    /// </summary>
    TimeSpan? GetUptime();
}
