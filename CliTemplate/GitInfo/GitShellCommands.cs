using Larcanum.ShellToolkit;

namespace CliTemplate.GitInfo;

public static class GitShellCommands
{
    private const string GitExecutable = "git";

    public static ICommand CurrentBranch(string? targetDirectory)
    {
        return Command.Create(GitExecutable, PrependTarget("branch --show-current", targetDirectory));
    }

    public static ICommand CurrentCommit(string? targetDirectory)
    {
        return Command.Create(GitExecutable, PrependTarget("rev-parse HEAD", targetDirectory));
    }

    public static ICommand Describe(string? targetDirectory)
    {
        return Command.Create(GitExecutable, PrependTarget("describe --tags --dirty --always", targetDirectory));
    }

    private static IEnumerable<string> PrependTarget(string args, string? targetDirectory)
    {
        var result = args.Split(" ");
        return targetDirectory is null
            ? result
            : result.Prepend(targetDirectory).Prepend("-C");
    }
}
