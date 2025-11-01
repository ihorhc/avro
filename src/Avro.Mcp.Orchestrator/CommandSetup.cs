namespace Avro.Mcp.Orchestrator;

/// <summary>
/// Command setup and orchestration
/// </summary>
public static class CommandSetup
{
    /// <summary>
    /// Creates the root command with all subcommands
    /// </summary>
    public static RootCommand CreateRootCommand(IMediator mediator, ConsolePresenter presenter)
    {
        var serverAddCommand = CreateServerAddCommand(mediator, presenter);
        var serverCommand = new Command("server", "Manage servers") { serverAddCommand };

        var listCommand = CreateListCommand(mediator, presenter);
        var startCommand = CreateStartCommand(mediator, presenter);
        var stopCommand = CreateStopCommand(mediator, presenter);
        var statusCommand = CreateStatusCommand(mediator, presenter);
        var configShowCommand = CreateConfigShowCommand(mediator, presenter);
        var configCommand = new Command("config", "Configuration") { configShowCommand };

        var root = new RootCommand("Avro MCP Orchestrator") 
        { 
            serverCommand, 
            listCommand, 
            startCommand, 
            stopCommand, 
            statusCommand, 
            configCommand 
        };

        return root;
    }

    private static Command CreateServerAddCommand(IMediator mediator, ConsolePresenter presenter)
    {
        var nameArg = new Argument<string>("name", "Server name");
        var commandArg = new Argument<string>("command", "Command to execute");
        var argsOption = new Option<string?>(new[] { "-a", "--args" }, "Command arguments");
        var workDirOption = new Option<string?>(new[] { "-w", "--working-dir" }, "Working directory");
        var timeoutOption = new Option<int>(new[] { "-t", "--timeout" }, () => 30, "Timeout in seconds");

        var command = new Command("add", "Add server") { nameArg, commandArg, argsOption, workDirOption, timeoutOption };
        command.SetHandler(async (name, cmd, args, workDir, timeout) =>
        {
            try
            {
                var addServerCmd = new AddServerCommand
                {
                    Name = name,
                    Command = cmd,
                    Arguments = args,
                    WorkingDirectory = workDir,
                    TimeoutSeconds = timeout
                };

                await mediator.Send(addServerCmd);
                presenter.PresentServerAddedSuccess(name);
            }
            catch (Exception ex)
            {
                presenter.PresentError(ex.Message);
            }
        }, nameArg, commandArg, argsOption, workDirOption, timeoutOption);

        return command;
    }

    private static Command CreateListCommand(IMediator mediator, ConsolePresenter presenter)
    {
        var list = new Command("list", "List servers");
        list.SetHandler(async () =>
        {
            try
            {
                var configs = await mediator.Send(new GetAllServersQuery());
                if (!configs.Any())
                {
                    presenter.PresentNoServersConfigured();
                    return;
                }

                var statuses = await mediator.Send(new GetAllServerStatusesQuery());
                var statusDict = statuses.ToDictionary(s => s.Name);

                var servers = configs.OrderBy(x => x.Name).Select(config =>
                    (config.Name, config.Command, statusDict[config.Name].Running, config.AutoStart)
                );

                presenter.PresentServersList(servers);
            }
            catch (Exception ex)
            {
                presenter.PresentError(ex.Message);
            }
        });

        return list;
    }

    private static Command CreateStartCommand(IMediator mediator, ConsolePresenter presenter)
    {
        var nameArg = new Argument<string?>("name", () => null, "Server name");
        var start = new Command("start", "Start") { nameArg };
        start.SetHandler(async (name) =>
        {
            try
            {
                if (string.IsNullOrEmpty(name))
                {
                    await mediator.Send(new StartAllServersCommand());
                }
                else
                {
                    presenter.PresentServerStarting(name);
                    await mediator.Send(new StartServerCommand(name));
                    var status = await mediator.Send(new GetServerStatusQuery(name));
                    if (status?.Running == true)
                    {
                        presenter.PresentServerStartedSuccess(name, status.Value.ProcessId ?? 0);
                    }
                }
            }
            catch (Exception ex)
            {
                presenter.PresentError(ex.Message);
            }
        }, nameArg);

        return start;
    }

    private static Command CreateStopCommand(IMediator mediator, ConsolePresenter presenter)
    {
        var nameArg = new Argument<string?>("name", () => null, "Server name");
        var stop = new Command("stop", "Stop") { nameArg };
        stop.SetHandler(async (name) =>
        {
            try
            {
                if (string.IsNullOrEmpty(name))
                {
                    await mediator.Send(new StopAllServersCommand());
                }
                else
                {
                    presenter.PresentServerStopping(name);
                    await mediator.Send(new StopServerCommand(name));
                    presenter.PresentServerStoppedSuccess(name);
                }
            }
            catch (Exception ex)
            {
                presenter.PresentError(ex.Message);
            }
        }, nameArg);

        return stop;
    }

    private static Command CreateStatusCommand(IMediator mediator, ConsolePresenter presenter)
    {
        var status = new Command("status", "Show server status");
        status.SetHandler(async () =>
        {
            try
            {
                var statuses = await mediator.Send(new GetAllServerStatusesQuery());
                if (!statuses.Any())
                {
                    presenter.PresentNoServersConfigured();
                    return;
                }

                presenter.PresentServerStatus(statuses);
            }
            catch (Exception ex)
            {
                presenter.PresentError(ex.Message);
            }
        });

        return status;
    }

    private static Command CreateConfigShowCommand(IMediator mediator, ConsolePresenter presenter)
    {
        var configShow = new Command("show", "Show config");
        configShow.SetHandler(async () =>
        {
            try
            {
                var configs = await mediator.Send(new GetAllServersQuery());
                var statuses = await mediator.Send(new GetAllServerStatusesQuery());
                var runningCount = statuses.Count(s => s.Running);

                var configPath = Path.Combine(
                    Environment.GetFolderPath(Environment.SpecialFolder.UserProfile),
                    ".avro", "mcp-config.json");

                var configDetails = configs.Select(c => (c.Name, c));
                presenter.PresentConfiguration(configPath, configs.Count(), runningCount, configDetails);
            }
            catch (Exception ex)
            {
                presenter.PresentError(ex.Message);
            }
        });

        return configShow;
    }
}
