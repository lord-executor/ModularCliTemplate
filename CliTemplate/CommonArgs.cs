using System.CommandLine;
using System.CommandLine.Binding;

using Microsoft.Extensions.Logging;

namespace CliTemplate;

public class CommonArgs
{
    private static readonly Option<string?> VerbosityOption = new Option<string?>(["--verbosity", "-v"], () => null);
    public string Verbosity { get; set; } = string.Empty;

    public static void Declare(Command command)
    {
        command.AddGlobalOption(VerbosityOption);
    }

    public static CommonArgs Bind(BindingContext bindingContext, CliSettings settings)
    {
        return new CommonArgs
        {
            Verbosity = bindingContext.ParseResult.GetValueForOption(VerbosityOption) ?? settings.DefaultVerbosity
        };
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