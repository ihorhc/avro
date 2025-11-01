namespace Avro.Mcp.Orchestrator.Services;

/// <summary>
/// High-level server orchestration service
/// </summary>
public interface IServerOrchestrationService
{
    Task AddServerAsync(ServerConfig config, CancellationToken ct = default);
    Task StartServerAsync(string serverName, CancellationToken ct = default);
    Task StartAllServersAsync(CancellationToken ct = default);
    Task StopServerAsync(string serverName, CancellationToken ct = default);
    Task StopAllServersAsync(CancellationToken ct = default);
    Task<IEnumerable<ServerConfig>> GetAllConfigurationsAsync(CancellationToken ct = default);
    Task<(string Name, bool Running, int? ProcessId, TimeSpan? Uptime)?> GetServerStatusAsync(string serverName, CancellationToken ct = default);
    Task<IEnumerable<(string Name, bool Running, int? ProcessId, TimeSpan? Uptime)>> GetAllStatusesAsync(CancellationToken ct = default);
}
