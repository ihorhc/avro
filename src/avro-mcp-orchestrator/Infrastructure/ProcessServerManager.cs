#nullable enable

namespace Avro.Mcp.Orchestrator.Infrastructure;

using System.Diagnostics;
using Avro.Mcp.Orchestrator.Services;
using Microsoft.Extensions.Logging;

/// <summary>
/// Process-based server manager
/// </summary>
public class ProcessServerManager : IServerProcessManager
{
    private readonly ILogger<ProcessServerManager>? _logger;
    private readonly string _serverName;
    private Process? _process;
    private DateTime _startTime;

    public int? ProcessId => _process?.Id;
    public bool IsRunning => _process is not null && !_process.HasExited;

    public ProcessServerManager(string serverName, ILogger<ProcessServerManager>? logger = null)
    {
        _serverName = serverName;
        _logger = logger;
    }

    public async Task StartAsync(ServerConfig config, CancellationToken ct = default)
    {
        if (_process is not null && !_process.HasExited)
            throw new InvalidOperationException($"Server {config.Name} is already running");

        var processInfo = new ProcessStartInfo
        {
            FileName = config.Command,
            Arguments = config.Arguments ?? string.Empty,
            UseShellExecute = false,
            RedirectStandardOutput = true,
            RedirectStandardError = true,
            CreateNoWindow = true
        };

        if (!string.IsNullOrEmpty(config.WorkingDirectory))
        {
            processInfo.WorkingDirectory = config.WorkingDirectory;
        }

        // Add environment variables
        foreach (var (key, value) in config.Environment)
        {
            processInfo.EnvironmentVariables[key] = value;
        }

        _process = new Process { StartInfo = processInfo };
        
        _process.OutputDataReceived += (_, args) =>
        {
            if (!string.IsNullOrEmpty(args.Data))
                _logger?.LogInformation("[{ServerName}] {Output}", _serverName, args.Data);
        };

        _process.ErrorDataReceived += (_, args) =>
        {
            if (!string.IsNullOrEmpty(args.Data))
                _logger?.LogError("[{ServerName}] {Error}", _serverName, args.Data);
        };

        _process.Start();
        _process.BeginOutputReadLine();
        _process.BeginErrorReadLine();

        _startTime = DateTime.UtcNow;
        
        _logger?.LogInformation("Server {ServerName} started with PID {ProcessId}", _serverName, _process.Id);
        await Task.CompletedTask;
    }

    public async Task StopAsync(int timeoutMs = 5000, CancellationToken ct = default)
    {
        if (_process is null || _process.HasExited)
            return;

        try
        {
            _process.Kill();
            using (var cts = new CancellationTokenSource(timeoutMs))
            {
                try
                {
                    await _process.WaitForExitAsync(cts.Token);
                }
                catch (OperationCanceledException)
                {
                    _process.Kill(entireProcessTree: true);
                }
            }
            
            _logger?.LogInformation("Server {ServerName} stopped", _serverName);
        }
        catch (Exception ex)
        {
            _logger?.LogError(ex, "Error stopping server {ServerName}", _serverName);
            throw;
        }
    }

    public TimeSpan GetUptime()
    {
        return IsRunning ? DateTime.UtcNow - _startTime : TimeSpan.Zero;
    }

    public async ValueTask DisposeAsync()
    {
        if (_process is not null && !_process.HasExited)
        {
            await StopAsync();
        }
        _process?.Dispose();
    }
}
