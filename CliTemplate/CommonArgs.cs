using Larcanum.ShellToolkit.Terminal.Integration;

using Microsoft.Extensions.Logging;

namespace CliTemplate;

public class CommonArgs : IArguments<CommonArgs>
{
    // See https://learn.microsoft.com/en-us/dotnet/standard/commandline/syntax#the---verbosity-option
    public string Verbosity { get; set; } = string.Empty;


    public static IEnumerable<ISymbolBinding<CommonArgs>> Register(BindingBuilder<CommonArgs> builder)
    {
        yield return builder.BindOption(x => x.Verbosity, "--verbosity", "Output verbosity Q(uiet), M(inimal), N(ormal), D(etailed) or Diag(nostic)")
            .WithAlias("-v");
    }

    public LogLevel ToLogLevel()
    {
        return Verbosity switch
        {
            "Q" or "Quiet" => LogLevel.Critical,
            "M" or "Minimal" => LogLevel.Error,
            "D" or "Detailed" => LogLevel.Debug,
            "Diag" or "Diagnostic" => LogLevel.Trace,
            _ => LogLevel.Warning
        };
    }
}
