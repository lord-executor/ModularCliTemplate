using Larcanum.ShellToolkit;

namespace CliTemplate.GitInfo;

public class GitShellCommands
{
    private const string GitExecutable = "git";

    private readonly ICommandRunner _shell;

    public GitShellCommands(ICommandRunner shell)
    {
        _shell = shell;
    }

    public Task<string> CurrentBranch(string? targetDirectory, CancellationToken ct = default)
    {
        return ShellExecute("branch --show-current", targetDirectory, ct);
    }

    public Task<string> CurrentCommit(string? targetDirectory, CancellationToken ct = default)
    {
        return ShellExecute("rev-parse HEAD", targetDirectory, ct);
    }

    public Task<string> Describe(string? targetDirectory, CancellationToken ct = default)
    {
        return ShellExecute("describe --tags --dirty --always", targetDirectory, ct);
    }

    private async Task<string> ShellExecute(string args, string? targetDirectory, CancellationToken ct = default)
    {
        return (await _shell.CaptureAsync(Command.Create(GitExecutable, PrependTarget(args, targetDirectory)), ct)).Output?.Trim() ?? string.Empty;
    }

    private static IEnumerable<string> PrependTarget(string args, string? targetDirectory)
    {
        var result = args.Split(" ");
        return targetDirectory is null
            ? result
            : result.Prepend(targetDirectory).Prepend("-C");
    }
}
