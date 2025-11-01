namespace Avro.Mcp.Orchestrator.Services;

/// <summary>
/// Manages server process lifecycle
/// </summary>
public interface IServerProcessManager : IAsyncDisposable
{
    int? ProcessId { get; }
    bool IsRunning { get; }
    
    Task StartAsync(ServerConfig config, CancellationToken ct = default);
    Task StopAsync(int timeoutMs = 5000, CancellationToken ct = default);
    TimeSpan GetUptime();
}
