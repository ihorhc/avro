#nullable enable

namespace Avro.Mcp.Orchestrator.Infrastructure;

using System.Text.Json;
using Avro.Mcp.Orchestrator.Services;
using Microsoft.Extensions.Logging;

/// <summary>
/// JSON file-based server configuration repository
/// </summary>
public class JsonServerConfigurationRepository : IServerConfigurationRepository
{
    private readonly ILogger<JsonServerConfigurationRepository>? _logger;
    private readonly string _configFilePath;
    private Dictionary<string, ServerConfig> _configurations = new();

    public JsonServerConfigurationRepository(string? configPath = null, ILogger<JsonServerConfigurationRepository>? logger = null)
    {
        _logger = logger;
        _configFilePath = configPath ?? Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile), ".avro", "mcp-config.json");
        
        InitializeConfigDirectory();
        LoadConfiguration();
    }

    public Task<ServerConfig?> GetAsync(string serverName, CancellationToken ct = default)
    {
        _configurations.TryGetValue(serverName, out var config);
        return Task.FromResult(config);
    }

    public Task<IEnumerable<ServerConfig>> GetAllAsync(CancellationToken ct = default)
    {
        return Task.FromResult(_configurations.Values.AsEnumerable());
    }

    public Task AddOrUpdateAsync(ServerConfig config, CancellationToken ct = default)
    {
        _configurations[config.Name] = config;
        return Task.CompletedTask;
    }

    public Task RemoveAsync(string serverName, CancellationToken ct = default)
    {
        _configurations.Remove(serverName);
        return Task.CompletedTask;
    }

    public async Task SaveAsync(CancellationToken ct = default)
    {
        try
        {
            var json = JsonSerializer.Serialize(
                _configurations.Values.ToList(),
                new JsonSerializerOptions { WriteIndented = true });
            
            var directory = Path.GetDirectoryName(_configFilePath);
            if (!string.IsNullOrEmpty(directory) && !Directory.Exists(directory))
            {
                Directory.CreateDirectory(directory);
            }

            await File.WriteAllTextAsync(_configFilePath, json, ct);
            _logger?.LogInformation("Saved configuration to {Path}", _configFilePath);
        }
        catch (Exception ex)
        {
            _logger?.LogError(ex, "Error saving configuration to {Path}", _configFilePath);
            throw;
        }
    }

    private void InitializeConfigDirectory()
    {
        var directory = Path.GetDirectoryName(_configFilePath);
        if (!string.IsNullOrEmpty(directory) && !Directory.Exists(directory))
        {
            Directory.CreateDirectory(directory);
            _logger?.LogInformation("Created configuration directory: {Directory}", directory);
        }
    }

    private void LoadConfiguration()
    {
        try
        {
            if (!File.Exists(_configFilePath))
            {
                _logger?.LogInformation("No existing configuration file found at {Path}", _configFilePath);
                return;
            }

            var json = File.ReadAllText(_configFilePath);
            var configs = JsonSerializer.Deserialize<List<ServerConfig>>(json) ?? new();
            
            foreach (var config in configs)
            {
                _configurations[config.Name] = config;
            }

            _logger?.LogInformation("Loaded {Count} server configurations", configs.Count);
        }
        catch (Exception ex)
        {
            _logger?.LogError(ex, "Error loading configuration from {Path}", _configFilePath);
        }
    }
}
