namespace Avro.Mcp.Infrastructure;

/// <summary>
/// Manages server process instances with lifecycle support
/// </summary>
public class ProcessServerManager : IServerProcessManager
{
    private readonly string _serverName;
    private readonly ILogger<ProcessServerManager>? _logger;
    private Process? _process;
    private DateTime _startTime;

    public int? ProcessId => _process?.Id;
    public bool IsRunning => _process?.HasExited == false;

    /// <summary>
    /// Initializes a new instance of ProcessServerManager
    /// </summary>
    public ProcessServerManager(string serverName, ILogger<ProcessServerManager>? logger = null)
    {
        _serverName = serverName;
        _logger = logger;
    }

    /// <summary>
    /// Starts the server process
    /// </summary>
    public async Task StartAsync(ServerConfig config, CancellationToken cancellationToken = default)
    {
        if (IsRunning)
        {
            _logger?.LogWarning("Server {ServerName} is already running", _serverName);
            return;
        }

        try
        {
            var processInfo = new ProcessStartInfo
            {
                FileName = config.Command,
                Arguments = config.Arguments ?? string.Empty,
                WorkingDirectory = config.WorkingDirectory ?? Environment.CurrentDirectory,
                UseShellExecute = false,
                RedirectStandardOutput = true,
                RedirectStandardError = true,
                CreateNoWindow = true
            };

            if (config.Environment.Count > 0)
            {
                foreach (var env in config.Environment)
                {
                    processInfo.EnvironmentVariables[env.Key] = env.Value;
                }
            }

            _process = Process.Start(processInfo);
            _startTime = DateTime.UtcNow;

            if (_process is null)
                throw new InvalidOperationException($"Failed to start process for server {_serverName}");

            _logger?.LogInformation("Server {ServerName} started with PID {ProcessId}", _serverName, _process.Id);

            await Task.CompletedTask;
        }
        catch (Exception ex)
        {
            _logger?.LogError(ex, "Failed to start server {ServerName}", _serverName);
            throw;
        }
    }

    /// <summary>
    /// Stops the server process
    /// </summary>
    public async Task StopAsync(int timeoutMilliseconds = 5000, CancellationToken cancellationToken = default)
    {
        if (_process is null || _process.HasExited)
        {
            _logger?.LogWarning("Server {ServerName} is not running", _serverName);
            return;
        }

        try
        {
            _process.Kill();
            var completed = _process.WaitForExit(timeoutMilliseconds);

            if (completed)
            {
                _logger?.LogInformation("Server {ServerName} stopped gracefully", _serverName);
            }
            else
            {
                _logger?.LogWarning("Server {ServerName} did not stop within timeout, forcing termination", _serverName);
                _process.Kill(true);
                _process.WaitForExit(2000);
            }

            await Task.CompletedTask;
        }
        catch (Exception ex)
        {
            _logger?.LogError(ex, "Error stopping server {ServerName}", _serverName);
            throw;
        }
        finally
        {
            _process?.Dispose();
            _process = null;
        }
    }

    /// <summary>
    /// Gets the uptime of the server
    /// </summary>
    public TimeSpan? GetUptime()
    {
        return IsRunning ? DateTime.UtcNow - _startTime : null;
    }
}
