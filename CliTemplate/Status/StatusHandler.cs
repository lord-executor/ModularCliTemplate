using System.CommandLine;
using System.CommandLine.Invocation;
using System.CommandLine.IO;

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
        await Task.Delay(5000, cancellationToken);
        _console.WriteLine("Done");

        return ExitCodes.Ok;
    }
}
