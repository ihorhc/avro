#nullable enable

namespace Avro.Mcp.Orchestrator;

/// <summary>
/// Legacy compatibility wrapper - use dependency injection services instead
/// </summary>
[Obsolete("Use IServerOrchestrationService with dependency injection instead")]
public class McpOrchestrator
{
    private readonly Services.IServerOrchestrationService _orchestrationService;
    private readonly Presentation.ConsolePresenter _presenter;

    public McpOrchestrator(ILogger<McpOrchestrator>? logger = null, OrchestratorOptions? options = null)
    {
        var configPath = options?.ConfigPath ?? Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile), ".avro", "mcp-config.json");
        
        var repository = new Infrastructure.JsonServerConfigurationRepository(configPath, null);
        var validator = new Infrastructure.ServerConfigurationValidator();
        var registry = new Infrastructure.InMemoryServerRegistry();
        
        _orchestrationService = new Services.ServerOrchestrationService(repository, validator, registry, null);
        _presenter = new Presentation.ConsolePresenter();
    }

    public async Task AddServerAsync(ServerConfig config)
    {
        try
        {
            await _orchestrationService.AddServerAsync(config);
            _presenter.PresentServerAddedSuccess(config.Name);
        }
        catch (Exception ex)
        {
            _presenter.PresentError(ex.Message);
            throw;
        }
    }

    public async Task StartServerAsync(string serverName)
    {
        try
        {
            _presenter.PresentServerStarting(serverName);
            await _orchestrationService.StartServerAsync(serverName);
            var status = await _orchestrationService.GetServerStatusAsync(serverName);
            if (status?.Running == true)
            {
                _presenter.PresentServerStartedSuccess(serverName, status.Value.ProcessId ?? 0);
            }
        }
        catch (Exception ex)
        {
            _presenter.PresentError(ex.Message);
        }
    }

    public async Task StartAllServersAsync()
    {
        try
        {
            await _orchestrationService.StartAllServersAsync();
        }
        catch (Exception ex)
        {
            _presenter.PresentError(ex.Message);
        }
    }

    public async Task StopServerAsync(string serverName)
    {
        try
        {
            _presenter.PresentServerStopping(serverName);
            await _orchestrationService.StopServerAsync(serverName);
            _presenter.PresentServerStoppedSuccess(serverName);
        }
        catch (Exception ex)
        {
            _presenter.PresentError(ex.Message);
        }
    }

    public async Task StopAllServersAsync()
    {
        try
        {
            await _orchestrationService.StopAllServersAsync();
        }
        catch (Exception ex)
        {
            _presenter.PresentError(ex.Message);
        }
    }

    public async Task ListServersAsync()
    {
        var configs = await _orchestrationService.GetAllConfigurationsAsync();
        if (!configs.Any())
        {
            _presenter.PresentNoServersConfigured();
            return;
        }

        var servers = configs.Select(c => 
            (c.Name, c.Command, _orchestrationService.GetServerStatusAsync(c.Name).Result?.Running ?? false, c.AutoStart)
        );
        _presenter.PresentServersList(servers);
    }

    public async Task ShowStatusAsync()
    {
        var statuses = await _orchestrationService.GetAllStatusesAsync();
        if (!statuses.Any())
        {
            _presenter.PresentNoServersConfigured();
            return;
        }
        _presenter.PresentServerStatus(statuses);
    }

    public async Task ShowConfigAsync()
    {
        var configs = await _orchestrationService.GetAllConfigurationsAsync();
        var configPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile), ".avro", "mcp-config.json");
        var runningCount = (await _orchestrationService.GetAllStatusesAsync()).Count(s => s.Running);
        
        _presenter.PresentConfiguration(configPath, configs.Count(), runningCount, configs.Select(c => (c.Name, c)));
    }
}
