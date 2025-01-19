using System.CommandLine.Invocation;
using System.Text.Json;

using CliTemplate.IO;

namespace CliTemplate.GitInfo;

public class GitInfoHandler : ISimpleHandler<GitInfoArgs>
{
    private readonly ICliLogger _logger;
    private readonly GitShellCommands _gitShellCommands;

    public GitInfoHandler(ICliLogger logger, GitShellCommands gitShellCommands)
    {
        _logger = logger;
        _gitShellCommands = gitShellCommands;
    }

    public async Task<int> RunAsync(InvocationContext context, GitInfoArgs args, CancellationToken ct = default)
    {
        var gitInfo = new Dictionary<string, string>
        {
            ["branch"] = await _gitShellCommands.CurrentBranch(args.Path, ct),
            ["commit"] = await _gitShellCommands.CurrentCommit(args.Path, ct),
            ["version"] = await _gitShellCommands.Describe(args.Path, ct)
        };

        _logger.LogContent(JsonSerializer.Serialize(gitInfo, new JsonSerializerOptions { WriteIndented = true }));

        return ExitCodes.Ok;
    }
}
