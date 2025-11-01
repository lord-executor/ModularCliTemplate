using System.CommandLine;

using Larcanum.ShellToolkit.Terminal.Integration;
using Larcanum.ShellToolkit.Terminal.Rendering;

using Microsoft.Extensions.Logging;

namespace CmdNs;

public class CmdTplCommand : ICommand<CmdTplArgs>
{
    public static CommandDefinition<CmdTplCommand, CmdTplArgs> Def = new Command("cmdname", "cmddesc");

    private readonly ICliLogger _logger;

    public CmdTplCommand(ICliLogger logger)
    {
        _logger = logger;
        _logger.LogInformation("CmdTplHandler created with CLI logger");
    }

    public async Task<int> RunAsync(CmdTplArgs args, CancellationToken cancellationToken = default)
    {
        _logger.LogContent($"Hello {args.Name}!");
        return ExitCodes.Ok;
    }
}
