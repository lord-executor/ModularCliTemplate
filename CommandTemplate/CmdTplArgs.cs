using Larcanum.ShellToolkit.Terminal.Integration;

namespace CmdNs;

public class CmdTplArgs : IArguments<CmdTplArgs>
{
    public string Name { get; set; } = "World";

    public static IEnumerable<ISymbolBinding<CmdTplArgs>> Register(BindingBuilder<CmdTplArgs> builder)
    {
        yield return builder.BindOption(x => x.Name, "--name", "Name option description");
    }
}
