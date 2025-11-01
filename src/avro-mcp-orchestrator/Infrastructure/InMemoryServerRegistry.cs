namespace Avro.Mcp.Orchestrator.Infrastructure;

using Avro.Mcp.Orchestrator.Services;

/// <summary>
/// In-memory server registry
/// </summary>
public class InMemoryServerRegistry : IServerRegistry
{
    private readonly Dictionary<string, IServerProcessManager> _instances = new();

    public IServerProcessManager? Get(string serverName)
    {
        _instances.TryGetValue(serverName, out var manager);
        return manager;
    }

    public void Register(string serverName, IServerProcessManager manager)
    {
        _instances[serverName] = manager;
    }

    public void Unregister(string serverName)
    {
        _instances.Remove(serverName);
    }

    public IEnumerable<(string Name, IServerProcessManager Manager)> GetAll()
    {
        return _instances.Select(x => (x.Key, x.Value));
    }

    public bool IsRunning(string serverName)
    {
        return _instances.TryGetValue(serverName, out var manager) && manager.IsRunning;
    }
}
