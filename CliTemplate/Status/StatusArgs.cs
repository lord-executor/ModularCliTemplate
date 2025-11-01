using Larcanum.ShellToolkit.Terminal.Integration;

namespace CliTemplate.Status;

public class StatusArgs : IArguments<StatusArgs>
{
    public bool Ignored { get; set; }

    public static IEnumerable<ISymbolBinding<StatusArgs>> Register(BindingBuilder<StatusArgs> builder)
    {
        yield return builder.BindOption(x => x.Ignored, "--ignored", "This option is ignored");
    }
}
