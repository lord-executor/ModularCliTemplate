using System.CommandLine;
using System.CommandLine.Invocation;
using System.CommandLine.IO;
using System.Diagnostics;

namespace CliTemplate.Status;

public class StatusHandler : ISimpleHandler<StatusArgs>
{
    private readonly IConsole _console;

    public StatusHandler(IConsole console)
    {
        _console = console;
        _console.WriteLine("StatusHandler created with console");
    }
    public async Task<int> RunAsync(InvocationContext context, StatusArgs args, CancellationToken cancellationToken = default)
    {
        _console.WriteLine("Hello StatusHandler!");
        _console.WriteLine($"Running in {Environment.CurrentDirectory}");

        var processStartInfo = new ProcessStartInfo
        {
            FileName = "git",
            WorkingDirectory = Environment.CurrentDirectory,
            ArgumentList = { "log", "--color" },
            UseShellExecute = false,
            RedirectStandardInput = true,
            RedirectStandardOutput = true,
            RedirectStandardError = true,
            CreateNoWindow = true,
        };

        var p = new Process() { StartInfo = processStartInfo };
        p.OutputDataReceived += (_, eventArgs) =>
        {
            if (eventArgs.Data != null)
            {
                _console.Out.WriteLine(eventArgs.Data!);
            }
        };
        p.ErrorDataReceived += (_, eventArgs) =>
        {
            if (eventArgs.Data != null)
            {
                _console.Error.WriteLine(eventArgs.Data!);
            }
        };
        p.Start();

        p.BeginErrorReadLine();
        p.BeginOutputReadLine();

        await p.WaitForExitAsync(cancellationToken);

        return ExitCodes.Ok;
    }
}
