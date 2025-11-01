namespace Avro.Mcp.Abstractions;

/// <summary>
/// Interface for managing server process instances
/// </summary>
public interface IServerRegistry
{
    /// <summary>
    /// Gets a server process manager by name
    /// </summary>
    IServerProcessManager? Get(string serverName);

    /// <summary>
    /// Registers a server process manager
    /// </summary>
    void Register(string serverName, IServerProcessManager manager);

    /// <summary>
    /// Unregisters a server process manager
    /// </summary>
    void Unregister(string serverName);

    /// <summary>
    /// Gets all registered server managers
    /// </summary>
    IEnumerable<(string Name, IServerProcessManager Manager)> GetAll();

    /// <summary>
    /// Checks if a server is running
    /// </summary>
    bool IsRunning(string serverName);
}
