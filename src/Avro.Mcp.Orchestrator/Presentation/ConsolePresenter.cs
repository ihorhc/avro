namespace Avro.Mcp.Orchestrator.Presentation;

/// <summary>
/// Console presenter for command output
/// </summary>
public class ConsolePresenter
{
    public void PresentServerAddedSuccess(string serverName)
    {
        Console.WriteLine($"✓ Server '{serverName}' added successfully");
    }

    public void PresentError(string message)
    {
        Console.Error.WriteLine($"✗ Error: {message}");
    }

    public void PresentNoServersConfigured()
    {
        Console.WriteLine("No servers configured");
    }

    public void PresentServersList(IEnumerable<(string Name, string Command, bool Running, bool AutoStart)> servers)
    {
        Console.WriteLine("\nConfigured Servers:");
        foreach (var (name, command, running, autoStart) in servers)
        {
            var status = running ? "✓ Running" : "✗ Stopped";
            var auto = autoStart ? " [AutoStart]" : "";
            Console.WriteLine($"  {name}: {command} - {status}{auto}");
        }
    }

    public void PresentServerStarting(string serverName)
    {
        Console.WriteLine($"→ Starting server '{serverName}'...");
    }

    public void PresentServerStartedSuccess(string serverName, int processId)
    {
        Console.WriteLine($"✓ Server '{serverName}' started (PID: {processId})");
    }

    public void PresentServerStopping(string serverName)
    {
        Console.WriteLine($"→ Stopping server '{serverName}'...");
    }

    public void PresentServerStoppedSuccess(string serverName)
    {
        Console.WriteLine($"✓ Server '{serverName}' stopped");
    }

    public void PresentServerStatus(IEnumerable<(string Name, bool Running, int? ProcessId, TimeSpan? Uptime)> statuses)
    {
        Console.WriteLine("\nServer Status:");
        foreach (var (name, running, pid, uptime) in statuses)
        {
            var status = running ? "✓ Running" : "✗ Stopped";
            var info = running ? $" (PID: {pid}, Uptime: {uptime?.ToString(@"hh\:mm\:ss")})" : "";
            Console.WriteLine($"  {name}: {status}{info}");
        }
    }

    public void PresentConfiguration(string configPath, int serverCount, int runningCount, IEnumerable<(string Name, ServerConfig Config)> configs)
    {
        Console.WriteLine($"\nConfiguration: {configPath}");
        Console.WriteLine($"Total Servers: {serverCount} (Running: {runningCount})");
        Console.WriteLine("\nDetails:");
        foreach (var (name, config) in configs)
        {
            Console.WriteLine($"  {name}:");
            Console.WriteLine($"    Command: {config.Command}");
            if (!string.IsNullOrEmpty(config.Arguments))
                Console.WriteLine($"    Arguments: {config.Arguments}");
            Console.WriteLine($"    Timeout: {config.TimeoutSeconds}s");
            Console.WriteLine($"    AutoStart: {config.AutoStart}");
        }
    }
}
