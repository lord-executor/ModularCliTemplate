using System.CommandLine;
using System.CommandLine.Invocation;

using AppRootNs.IO;

using Microsoft.Extensions.Logging;

namespace CmdNs;

public class CmdTplHandler : ISimpleHandler<CmdTplArgs>
{
    private readonly ICliLogger _logger;

    public CmdTplHandler(ICliLogger logger)
    {
        _logger = logger;
        _logger.LogInformation("CmdTplHandler created with CLI logger");
    }

    public async Task<int> RunAsync(InvocationContext context, CmdTplArgs args, CancellationToken cancellationToken = default)
    {
        _logger.LogContent($"Hello {args.Name}!");
        return ExitCodes.Ok;
    }
}
