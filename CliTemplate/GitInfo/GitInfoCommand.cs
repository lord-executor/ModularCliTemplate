using Larcanum.ShellToolkit;

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

using Command = System.CommandLine.Command;

namespace CliTemplate.GitInfo;

public class GitInfoCommand : Command, IServiceModule
{
    public GitInfoCommand(HandlerFactory handlerFactory)
        : base("gitinfo", "Gathers information from git")
    {
        GitInfoArgs.Declare(this);
        Handler = handlerFactory.SimpleHandler<GitInfoHandler, GitInfoArgs>(() => [this]);
    }

    void IServiceModule.ConfigureServices(IServiceCollection services, IConfigurationRoot config)
    {
        services.AddSingleton(new Settings());
        services.AddSingleton<ICommandRunner, CommandRunner>();
    }
}
