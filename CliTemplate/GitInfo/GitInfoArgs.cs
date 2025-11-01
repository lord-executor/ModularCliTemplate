using Larcanum.ShellToolkit.Terminal.Integration;

namespace CliTemplate.GitInfo;

public class GitInfoArgs : IArguments<GitInfoArgs>
{
    public string? Path { get; set; }

    public static IEnumerable<ISymbolBinding<GitInfoArgs>> Register(BindingBuilder<GitInfoArgs> builder)
    {
        yield return builder.BindOption(x => x.Path, "--path", "Path to git repository");
    }
}
