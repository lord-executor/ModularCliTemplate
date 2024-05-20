using System.CommandLine;
using System.CommandLine.Invocation;
using System.CommandLine.NamingConventionBinder;

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace CliTemplate;

public class HandlerFactory
{
    private readonly ServiceCollection _services;

    public HandlerFactory(IConfigurationRoot config)
    {
        _services = new ServiceCollection();
        _services.AddSingleton<IConfiguration>(config);
    }

    public HandlerFactory Configure(Action<ServiceCollection> configureAction)
    {
        configureAction(_services);
        return this;
    }

    public ICommandHandler SimpleHandler<THandler, TArg>()
        where THandler : class, ISimpleHandler<TArg>
        where TArg : class
    {
        _services.AddTransient<ISimpleHandler<TArg>, THandler>();

        return CommandHandler.Create(async (InvocationContext ctx, TArg arg) =>
        {
            _services.AddSingleton(arg);
            _services.AddTransient<IConsole>(_ => ctx.BindingContext.GetService<IConsole>()!);

            var handler = _services.BuildServiceProvider().GetRequiredService<ISimpleHandler<TArg>>();
            return await handler.RunAsync(ctx, arg, ctx.GetCancellationToken());
        });
    }
}
