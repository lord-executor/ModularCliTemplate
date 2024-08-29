using System.CommandLine;
using System.CommandLine.Invocation;
using System.Text.Json;

using CliTemplate.IO;

using Larcanum.ShellToolkit;

using Microsoft.Extensions.Logging;

namespace CliTemplate.GitInfo;

public class GitInfoHandler : ISimpleHandler<GitInfoArgs>
{
    private readonly ICliLogger _logger;
    private readonly ICommandRunner _commandRunner;

    public GitInfoHandler(ICliLogger logger, ICommandRunner commandRunner)
    {
        _logger = logger;
        _commandRunner = commandRunner;
    }

    public async Task<int> RunAsync(InvocationContext context, GitInfoArgs args, CancellationToken cancellationToken = default)
    {
        var gitInfo = new Dictionary<string, string?>();

        gitInfo["branch"] = (await _commandRunner.CaptureAsync(GitShellCommands.CurrentBranch(args.Path), cancellationToken)).Output?.Trim();
        gitInfo["commit"] = (await _commandRunner.CaptureAsync(GitShellCommands.CurrentCommit(args.Path), cancellationToken)).Output?.Trim();
        gitInfo["version"] = (await _commandRunner.CaptureAsync(GitShellCommands.Describe(args.Path), cancellationToken)).Output?.Trim();

        _logger.LogContent(JsonSerializer.Serialize(gitInfo, new JsonSerializerOptions { WriteIndented = true }));

        return ExitCodes.Ok;
    }
}
