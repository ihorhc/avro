namespace Avro.Mcp.Orchestrator.Presentation;

using Spectre.Console;

/// <summary>
/// Formats and presents information to console
/// </summary>
public class ConsolePresenter
{
    public void PresentServerAddedSuccess(string serverName)
    {
        AnsiConsole.MarkupLine($"[green]✓ Server '{serverName}' added successfully[/]");
    }

    public void PresentServerAlreadyExists(string serverName)
    {
        AnsiConsole.MarkupLine($"[yellow]⚠ Warning: Server '{serverName}' already exists. Updating configuration.[/]");
    }

    public void PresentError(string message)
    {
        AnsiConsole.MarkupLine($"[red]✗ Error: {message}[/]");
    }

    public void PresentServerNotFound(string serverName)
    {
        AnsiConsole.MarkupLine($"[red]✗ Server '{serverName}' not found[/]");
    }

    public void PresentServerAlreadyRunning(string serverName)
    {
        AnsiConsole.MarkupLine($"[yellow]⚠ Server '{serverName}' is already running[/]");
    }

    public void PresentServerStarting(string serverName)
    {
        AnsiConsole.MarkupLine($"[cyan]▶ Starting server '{serverName}'...[/]");
    }

    public void PresentServerStartedSuccess(string serverName, int processId)
    {
        AnsiConsole.MarkupLine($"[green]✓ Server '{serverName}' started successfully (PID: {processId})[/]");
    }

    public void PresentServerStopping(string serverName)
    {
        AnsiConsole.MarkupLine($"[cyan]▶ Stopping server '{serverName}'...[/]");
    }

    public void PresentServerStoppedSuccess(string serverName)
    {
        AnsiConsole.MarkupLine($"[green]✓ Server '{serverName}' stopped successfully[/]");
    }

    public void PresentServerNotRunning(string serverName)
    {
        AnsiConsole.MarkupLine($"[yellow]⚠ Server '{serverName}' is not running[/]");
    }

    public void PresentNoServersConfigured()
    {
        AnsiConsole.MarkupLine("[yellow]ℹ No servers configured[/]");
    }

    public void PresentNoServersRunning()
    {
        AnsiConsole.MarkupLine("[yellow]ℹ No servers running[/]");
    }

    public void PresentStartingServers(int count)
    {
        AnsiConsole.MarkupLine($"[cyan]Starting {count} server(s)...[/]");
    }

    public void PresentStoppingServers(int count)
    {
        AnsiConsole.MarkupLine($"[cyan]Stopping {count} server(s)...[/]");
    }

    public void PresentNoAutoStartServers()
    {
        AnsiConsole.MarkupLine("[yellow]ℹ No servers configured for auto-start[/]");
    }

    public void PresentServersList(IEnumerable<(string Name, string Command, bool Running, bool AutoStart)> servers)
    {
        var table = new Table();
        table.AddColumn("Name");
        table.AddColumn("Command");
        table.AddColumn("Status");
        table.AddColumn("Auto-Start");

        foreach (var (name, command, running, autoStart) in servers)
        {
            var status = running ? "[green]Running[/]" : "[yellow]Stopped[/]";
            var autoStartText = autoStart ? "[green]Yes[/]" : "[red]No[/]";
            table.AddRow(name, command, status, autoStartText);
        }

        AnsiConsole.Write(table);
    }

    public void PresentServerStatus(IEnumerable<(string Name, bool Running, int? ProcessId, TimeSpan? Uptime)> statuses)
    {
        var table = new Table();
        table.AddColumn("Server");
        table.AddColumn("Status");
        table.AddColumn("PID");
        table.AddColumn("Uptime");

        foreach (var (name, running, pid, uptime) in statuses)
        {
            var statusText = running ? "[green]✓ Running[/]" : "[red]✗ Stopped[/]";
            var pidText = running ? pid.ToString() ?? "N/A" : "N/A";
            var uptimeText = running ? FormatUptime(uptime) : "N/A";

            table.AddRow(name, statusText, pidText, uptimeText);
        }

        AnsiConsole.Write(table);
    }

    public void PresentConfiguration(string configPath, int totalServers, int runningServers, IEnumerable<(string Name, ServerConfig Config)> configurations)
    {
        AnsiConsole.MarkupLine($"[cyan]Configuration File: {configPath}[/]");
        AnsiConsole.MarkupLine($"[cyan]Total Servers: {totalServers}[/]");
        AnsiConsole.MarkupLine($"[cyan]Running Servers: {runningServers}[/]");
        
        AnsiConsole.WriteLine();
        
        if (configurations.Any())
        {
            AnsiConsole.MarkupLine("[bold]Configured Servers:[/]");
            foreach (var (name, config) in configurations)
            {
                AnsiConsole.MarkupLine($"  [green]{name}[/]");
                AnsiConsole.MarkupLine($"    Command: {config.Command}");
                if (!string.IsNullOrEmpty(config.Arguments))
                    AnsiConsole.MarkupLine($"    Args: {config.Arguments}");
                AnsiConsole.MarkupLine($"    Timeout: {config.TimeoutSeconds}s");
                AnsiConsole.MarkupLine($"    Auto-Start: {(config.AutoStart ? "Yes" : "No")}");
            }
        }
    }

    private static string FormatUptime(TimeSpan? uptime)
    {
        if (uptime is null) return "N/A";
        
        return uptime.Value.TotalHours >= 1
            ? $"{uptime.Value.Hours:D2}:{uptime.Value.Minutes:D2}:{uptime.Value.Seconds:D2}"
            : $"{uptime.Value.Minutes:D2}:{uptime.Value.Seconds:D2}";
    }
}
