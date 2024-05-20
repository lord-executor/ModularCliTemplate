using System.CommandLine;
using System.CommandLine.Invocation;
using System.CommandLine.NamingConventionBinder;

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace CliTemplate;

public class HandlerFactory
{
    private readonly IConfigurationRoot _config;
    private readonly ServiceCollection _services;

    public HandlerFactory(IConfigurationRoot config)
    {
        _config = config;
        _services = new ServiceCollection();
        _services.AddSingleton<IConfiguration>(config);
    }

    public ICommandHandler SimpleHandler<THandler, TArg>()
        where THandler : class, ISimpleHandler<TArg>
        where TArg : class
    {
        return SimpleHandler<THandler, TArg>(Enumerable.Empty<IServiceModule>);
    }

    public ICommandHandler SimpleHandler<THandler, TArg>(Func<IEnumerable<IServiceModule>> modules)
        where THandler : class, ISimpleHandler<TArg>
        where TArg : class
    {
        _services.AddTransient<ISimpleHandler<TArg>, THandler>();

        return CommandHandler.Create(async (InvocationContext ctx, TArg arg) =>
        {
            _services.AddSingleton(arg);
            _services.AddTransient<IConsole>(_ => ctx.BindingContext.GetService<IConsole>()!);

            foreach (var mod in modules())
            {
                mod.ConfigureServices(_services, _config);
            }

            var handler = _services.BuildServiceProvider().GetRequiredService<ISimpleHandler<TArg>>();
            return await handler.RunAsync(ctx, arg, ctx.GetCancellationToken());
        });
    }
}
