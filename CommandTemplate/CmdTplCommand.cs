using System.CommandLine;

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace CmdNs;

public class CmdTplCommand : Command, IServiceModule
{
    public CmdTplCommand(HandlerFactory handlerFactory)
        : base("cmdname", "cmddesc")
    {
        CmdTplArgs.Declare(this);
        Handler = handlerFactory.SimpleHandler<CmdTplHandler, CmdTplArgs>(() => [this]);
    }

    void IServiceModule.ConfigureServices(IServiceCollection services, IConfigurationRoot config)
    {
    }
}
