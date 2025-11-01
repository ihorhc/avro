namespace Avro.Mcp.Orchestrator.Services;

/// <summary>
/// Repository for managing server configurations
/// </summary>
public interface IServerConfigurationRepository
{
    Task<ServerConfig?> GetAsync(string serverName, CancellationToken ct = default);
    Task<IEnumerable<ServerConfig>> GetAllAsync(CancellationToken ct = default);
    Task AddOrUpdateAsync(ServerConfig config, CancellationToken ct = default);
    Task RemoveAsync(string serverName, CancellationToken ct = default);
    Task SaveAsync(CancellationToken ct = default);
}
