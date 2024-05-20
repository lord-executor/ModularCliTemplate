using System.CommandLine;

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace CliTemplate.Status;

public class StatusCommand : Command, IServiceModule
{
    public StatusCommand(HandlerFactory handlerFactory)
        : base("status", "Gets the status")
    {
        StatusArgs.Declare(this);
        Handler = handlerFactory.SimpleHandler<StatusHandler, StatusArgs>(() => [this]);
    }

    void IServiceModule.ConfigureServices(IServiceCollection services, IConfigurationRoot config)
    {
        services.AddSingleton(new DelayConfig(config));
    }
}
