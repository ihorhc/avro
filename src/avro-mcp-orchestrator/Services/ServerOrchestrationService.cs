#nullable enable

namespace Avro.Mcp.Orchestrator.Services;

using Microsoft.Extensions.Logging;

/// <summary>
/// High-level server orchestration service
/// </summary>
public class ServerOrchestrationService : IServerOrchestrationService
{
    private readonly IServerConfigurationRepository _repository;
    private readonly IServerConfigurationValidator _validator;
    private readonly IServerRegistry _registry;
    private readonly ILogger<ServerOrchestrationService>? _logger;

    public ServerOrchestrationService(
        IServerConfigurationRepository repository,
        IServerConfigurationValidator validator,
        IServerRegistry registry,
        ILogger<ServerOrchestrationService>? logger = null)
    {
        _repository = repository;
        _validator = validator;
        _registry = registry;
        _logger = logger;
    }

    public async Task AddServerAsync(ServerConfig config, CancellationToken ct = default)
    {
        try
        {
            _validator.Validate(config);
            
            await _repository.AddOrUpdateAsync(config, ct);
            await _repository.SaveAsync(ct);
            
            _logger?.LogInformation("Server {ServerName} added with command: {Command}", config.Name, config.Command);
        }
        catch (Exception ex)
        {
            _logger?.LogError(ex, "Error adding server {ServerName}", config.Name);
            throw;
        }
    }

    public async Task StartServerAsync(string serverName, CancellationToken ct = default)
    {
        try
        {
            if (!_registry.IsRunning(serverName))
            {
                var config = await _repository.GetAsync(serverName, ct);
                if (config is null)
                {
                    _logger?.LogWarning("Server {ServerName} not found", serverName);
                    return;
                }

                var manager = new Infrastructure.ProcessServerManager(serverName, _logger as ILogger<Infrastructure.ProcessServerManager>);
                await manager.StartAsync(config, ct);
                _registry.Register(serverName, manager);

                _logger?.LogInformation("Server {ServerName} started with PID {ProcessId}", serverName, manager.ProcessId);
            }
            else
            {
                _logger?.LogInformation("Server {ServerName} is already running", serverName);
            }
        }
        catch (Exception ex)
        {
            _logger?.LogError(ex, "Error starting server {ServerName}", serverName);
            throw;
        }
    }

    public async Task StartAllServersAsync(CancellationToken ct = default)
    {
        var autoStartServers = (await _repository.GetAllAsync(ct))
            .Where(c => c.AutoStart)
            .ToList();
        
        if (!autoStartServers.Any())
        {
            _logger?.LogInformation("No servers configured for auto-start");
            return;
        }

        _logger?.LogInformation("Starting {Count} auto-start server(s)", autoStartServers.Count);
        
        foreach (var config in autoStartServers)
        {
            await StartServerAsync(config.Name, ct);
        }
    }

    public async Task StopServerAsync(string serverName, CancellationToken ct = default)
    {
        try
        {
            var manager = _registry.Get(serverName);
            if (manager is null)
            {
                _logger?.LogWarning("Server {ServerName} is not running", serverName);
                return;
            }

            await manager.StopAsync(5000, ct);
            _registry.Unregister(serverName);

            _logger?.LogInformation("Server {ServerName} stopped", serverName);
        }
        catch (Exception ex)
        {
            _logger?.LogError(ex, "Error stopping server {ServerName}", serverName);
            throw;
        }
    }

    public async Task StopAllServersAsync(CancellationToken ct = default)
    {
        var runningServers = _registry.GetAll().Where(x => x.Manager.IsRunning).ToList();
        
        if (!runningServers.Any())
        {
            _logger?.LogInformation("No servers running");
            return;
        }

        _logger?.LogInformation("Stopping {Count} server(s)", runningServers.Count);
        
        foreach (var (serverName, _) in runningServers)
        {
            await StopServerAsync(serverName, ct);
        }
    }

    public async Task<IEnumerable<ServerConfig>> GetAllConfigurationsAsync(CancellationToken ct = default)
    {
        return await _repository.GetAllAsync(ct);
    }

    public async Task<(string Name, bool Running, int? ProcessId, TimeSpan? Uptime)?> GetServerStatusAsync(string serverName, CancellationToken ct = default)
    {
        var config = await _repository.GetAsync(serverName, ct);
        if (config is null)
            return null;

        var manager = _registry.Get(serverName);
        if (manager?.IsRunning == true)
        {
            return (serverName, true, manager.ProcessId, manager.GetUptime());
        }

        return (serverName, false, null, null);
    }

    public async Task<IEnumerable<(string Name, bool Running, int? ProcessId, TimeSpan? Uptime)>> GetAllStatusesAsync(CancellationToken ct = default)
    {
        var configs = await _repository.GetAllAsync(ct);
        var statuses = new List<(string, bool, int?, TimeSpan?)>();

        foreach (var config in configs.OrderBy(x => x.Name))
        {
            var manager = _registry.Get(config.Name);
            if (manager?.IsRunning == true)
            {
                statuses.Add((config.Name, true, manager.ProcessId, manager.GetUptime()));
            }
            else
            {
                statuses.Add((config.Name, false, null, null));
            }
        }

        return statuses;
    }
}
