using System.CommandLine;

using CliTemplate.GitInfo;

using Larcanum.ShellToolkit.Terminal.Integration;
using Larcanum.ShellToolkit.Terminal.Rendering;

using Microsoft.Extensions.Logging;

namespace CliTemplate.Status;

public class StatusCommand : ICommand<StatusArgs>
{
    public static CommandDefinition<StatusCommand, StatusArgs> Def = new Command("status", "Gets the status");

    private readonly ICliLogger _logger;
    private readonly IChildLauncher _childLauncher;
    private readonly DelayConfig _config;

    public StatusCommand(ICliLogger logger, IChildLauncher childLauncher, DelayConfig config)
    {
        _logger = logger;
        _childLauncher = childLauncher;
        _config = config;
        _logger.LogInformation("StatusHandler created with CLI logger");
    }

    public async Task<int> RunAsync(StatusArgs args, CancellationToken ct = default)
    {
        _logger.LogContent("Hello StatusHandler!");
        await Task.Delay(_config.Delay, ct);
        await _childLauncher.RunAsync(GitInfoCommand.Def, new GitInfoArgs(), ct);
        _logger.LogContent("Done");

        return ExitCodes.Ok;
    }
}
