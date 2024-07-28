using System.CommandLine;
using System.CommandLine.Invocation;
using System.CommandLine.NamingConventionBinder;

using CliTemplate.IO;

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

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
            var commonArgs = CommonArgs.Bind(ctx.BindingContext, _config.GetRequiredSection(CliSettings.SectionName).Get<CliSettings>()!);
            // This allows us to access the CommonArgs from within the exception handler in the Launcher
            ctx.BindingContext.AddService(_ => commonArgs);

            _services.AddSingleton(_config);
            _services.AddSingleton(commonArgs);
            _services.AddSingleton(arg);
            _services.AddSingleton<ICliLogger>(_ => new CliLogger(ctx.Console, commonArgs.ToLogLevel()));
            // This allows injecting the ICliLogger as a normal ILogger
            _services.AddSingleton<ILogger>(provider => provider.GetRequiredService<ICliLogger>());

            foreach (var mod in modules())
            {
                mod.ConfigureServices(_services, _config);
            }

            var handler = _services.BuildServiceProvider().GetRequiredService<ISimpleHandler<TArg>>();
            return await handler.RunAsync(ctx, arg, ctx.GetCancellationToken());
        });
    }
}
