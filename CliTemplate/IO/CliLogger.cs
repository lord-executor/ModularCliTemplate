using System.CommandLine;
using System.CommandLine.IO;
using System.CommandLine.Rendering;

using Microsoft.Extensions.Logging;

namespace CliTemplate.IO;

public class CliLogger : ICliLogger
{
    private static TextSpan SimpleFgColor(AnsiControlCode fgColor, string msg)
    {
        return new ContainerSpan(
            new ForegroundColorSpan("fg", fgColor),
            new ContentSpan(msg),
            ForegroundColorSpan.Reset()
        );
    }

    private static readonly IDictionary<LogLevel, Func<string, TextSpan>> _spanMapping =
        new Dictionary<LogLevel, Func<string, TextSpan>>
        {
            [LogLevel.Trace] = msg => SimpleFgColor(Ansi.Color.Foreground.LightGray, msg),
            [LogLevel.Debug] = msg => SimpleFgColor(Ansi.Color.Foreground.LightCyan, msg),
            [LogLevel.Information] = msg => SimpleFgColor(Ansi.Color.Foreground.LightBlue, msg),
            [LogLevel.Warning] = msg => SimpleFgColor(Ansi.Color.Foreground.Yellow, msg),
            [LogLevel.Error] = msg => SimpleFgColor(Ansi.Color.Foreground.Red, msg),
            [LogLevel.Critical] = msg => SimpleFgColor(Ansi.Color.Foreground.Red, msg),
        };

    private readonly LogLevel _threshold;

    public IConsole Console { get; }

    public CliLogger(IConsole console, LogLevel threshold)
    {
        Console = console;
        _threshold = threshold;
    }

    public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception? exception, Func<TState, Exception?, string> formatter)
    {
        if (IsEnabled(logLevel))
        {
            var span = _spanMapping[logLevel](formatter(state, exception));

            if (logLevel >= LogLevel.Error)
            {
                WriteError(span);
            }
            else
            {
                WriteOutput(span);
            }
        }
    }

    public bool IsEnabled(LogLevel logLevel)
    {
        return logLevel >= _threshold;
    }

    public IDisposable? BeginScope<TState>(TState state) where TState : notnull
    {
        throw new NotImplementedException();
    }

    public void WriteOutput(TextSpan span)
    {
        Console.Out.WriteLine(span.ToString(OutputMode.Ansi));
    }

    public void WriteError(TextSpan span)
    {
        Console.Error.WriteLine(span.ToString(OutputMode.Ansi));
    }

    public string? Prompt(string promptLine)
    {
        Console.Write(promptLine);
        Console.WriteLine(" ");
        return System.Console.ReadLine();
    }
}
