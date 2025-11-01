namespace Avro.Mcp.Orchestrator.Services;

/// <summary>
/// Registry for tracking running server instances
/// </summary>
public interface IServerRegistry
{
    IServerProcessManager? Get(string serverName);
    void Register(string serverName, IServerProcessManager manager);
    void Unregister(string serverName);
    IEnumerable<(string Name, IServerProcessManager Manager)> GetAll();
    bool IsRunning(string serverName);
}
