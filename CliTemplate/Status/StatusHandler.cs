using System.CommandLine;
using System.CommandLine.Invocation;

using CliTemplate.GitInfo;
using CliTemplate.IO;

using Microsoft.Extensions.Logging;

namespace CliTemplate.Status;

public class StatusHandler : ISimpleHandler<StatusArgs>
{
    private readonly ICliLogger _logger;
    private readonly IChildLauncher _childLauncher;
    private readonly DelayConfig _config;

    public StatusHandler(ICliLogger logger, IChildLauncher childLauncher, DelayConfig config)
    {
        _logger = logger;
        _childLauncher = childLauncher;
        _config = config;
        _logger.LogInformation("StatusHandler created with CLI logger");
    }

    public async Task<int> RunAsync(InvocationContext context, StatusArgs args, CancellationToken cancellationToken = default)
    {
        _logger.LogContent("Hello StatusHandler!");
        await Task.Delay(_config.Delay, cancellationToken);
        await _childLauncher.RunAsync(context, new GitInfoArgs(), cancellationToken);
        _logger.LogContent("Done");

        return ExitCodes.Ok;
    }
}
