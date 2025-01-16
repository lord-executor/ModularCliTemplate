using System.CommandLine;
using System.CommandLine.Invocation;
using System.CommandLine.NamingConventionBinder;

using CliTemplate.IO;

using Larcanum.ShellToolkit;

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace CliTemplate;

public class HandlerFactory
{
    private readonly IConfiguration _config;
    private readonly ServiceCollection _services;
    private IServiceProvider? _serviceProvider;

    public HandlerFactory(IConfiguration config)
    {
        _config = config;
        _services = new ServiceCollection();
        _services.AddSingleton(_config);
        _services.AddSingleton(new Settings());
        _services.AddSingleton<ICommandRunner, CommandRunner>();
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
        _services.AddScoped<ISimpleHandler<TArg>, THandler>();
        foreach (var mod in modules())
        {
            mod.ConfigureServices(_services, _config);
        }

        return CommandHandler.Create(async (InvocationContext ctx, TArg arg) =>
        {
            var provider = EnsureServiceProvider(ctx);

            using var scope = provider.CreateScope();
            var handler = scope.ServiceProvider.GetRequiredService<ISimpleHandler<TArg>>();
            return await handler.RunAsync(ctx, arg, ctx.GetCancellationToken());
        });
    }

    private IServiceProvider EnsureServiceProvider(InvocationContext ctx)
    {
        // Note that the final part of the service collection configuration takes place only once for the initial
        // command that is being executed and is NOT repeated for child commands. These services are added late
        // because they depend on the invocation context.
        if (_serviceProvider == null)
        {
            var commonArgs = CommonArgs.Bind(ctx.BindingContext,
                _config.GetRequiredSection(CliSettings.SectionName).Get<CliSettings>()!);
            // This allows us to access the CommonArgs from within the exception handler in the Launcher
            ctx.BindingContext.AddService(_ => commonArgs);
            // This is for injecting the CommonArgs in other places which may end up being useful
            _services.AddSingleton(commonArgs);

            _services.AddSingleton<ICliLogger>(_ => new CliLogger(ctx.Console, commonArgs.ToLogLevel()));
            // This allows injecting the ICliLogger as a normal ILogger
            _services.AddSingleton<ILogger>(provider => provider.GetRequiredService<ICliLogger>());

            _serviceProvider = _services.BuildServiceProvider();
        }

        return _serviceProvider;
    }
}
