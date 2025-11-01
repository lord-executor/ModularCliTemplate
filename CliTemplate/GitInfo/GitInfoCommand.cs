using System.Text.Json;

using Larcanum.ShellToolkit.Terminal.Integration;
using Larcanum.ShellToolkit.Terminal.Rendering;

using Command = System.CommandLine.Command;

namespace CliTemplate.GitInfo;

public class GitInfoCommand : ICommand<GitInfoArgs>
{
    public static CommandDefinition<GitInfoCommand, GitInfoArgs> Def = new Command("gitinfo", "Gathers information from git");

    private readonly ICliLogger _logger;
    private readonly GitShellCommands _gitShellCommands;

    public GitInfoCommand(ICliLogger logger, GitShellCommands gitShellCommands)
    {
        _logger = logger;
        _gitShellCommands = gitShellCommands;
    }

    public async Task<int> RunAsync(GitInfoArgs args, CancellationToken ct = default)
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
