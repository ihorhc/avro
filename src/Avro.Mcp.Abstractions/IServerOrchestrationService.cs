namespace Avro.Mcp.Abstractions;

/// <summary>
/// High-level interface for server orchestration operations
/// </summary>
public interface IServerOrchestrationService
{
    /// <summary>
    /// Adds a new server configuration
    /// </summary>
    Task AddServerAsync(ServerConfig config, CancellationToken cancellationToken = default);

    /// <summary>
    /// Starts a specific server by name
    /// </summary>
    Task StartServerAsync(string serverName, CancellationToken cancellationToken = default);

    /// <summary>
    /// Starts all servers configured for auto-start
    /// </summary>
    Task StartAllServersAsync(CancellationToken cancellationToken = default);

    /// <summary>
    /// Stops a specific server by name
    /// </summary>
    Task StopServerAsync(string serverName, CancellationToken cancellationToken = default);

    /// <summary>
    /// Stops all running servers
    /// </summary>
    Task StopAllServersAsync(CancellationToken cancellationToken = default);

    /// <summary>
    /// Gets all server configurations
    /// </summary>
    Task<IEnumerable<ServerConfig>> GetAllConfigurationsAsync(CancellationToken cancellationToken = default);

    /// <summary>
    /// Gets the status of a specific server
    /// </summary>
    Task<(string Name, bool Running, int? ProcessId, TimeSpan? Uptime)?> GetServerStatusAsync(string serverName, CancellationToken cancellationToken = default);

    /// <summary>
    /// Gets the status of all servers
    /// </summary>
    Task<IEnumerable<(string Name, bool Running, int? ProcessId, TimeSpan? Uptime)>> GetAllStatusesAsync(CancellationToken cancellationToken = default);
}
