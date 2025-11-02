namespace Avro.Mcp.Abstractions;

/// <summary>
/// Interface for server configuration persistence
/// </summary>
public interface IServerConfigurationRepository
{
    /// <summary>
    /// Gets a server configuration by name
    /// </summary>
    Task<ServerConfig?> GetAsync(string serverName, CancellationToken cancellationToken = default);

    /// <summary>
    /// Gets all server configurations
    /// </summary>
    Task<IEnumerable<ServerConfig>> GetAllAsync(CancellationToken cancellationToken = default);

    /// <summary>
    /// Adds or updates a server configuration
    /// </summary>
    Task AddOrUpdateAsync(ServerConfig config, CancellationToken cancellationToken = default);

    /// <summary>
    /// Removes a server configuration
    /// </summary>
    Task RemoveAsync(string serverName, CancellationToken cancellationToken = default(CancellationToken));

    /// <summary>
    /// Saves all configurations to persistent storage
    /// </summary>
    Task SaveAsync(CancellationToken cancellationToken = default);
}
