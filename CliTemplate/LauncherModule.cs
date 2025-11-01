using System.CommandLine;
using System.Reflection;

using CliTemplate.GitInfo;
using CliTemplate.Status;

using Larcanum.ShellToolkit;
using Larcanum.ShellToolkit.Terminal;
using Larcanum.ShellToolkit.Terminal.Integration;
using Larcanum.ShellToolkit.Terminal.Rendering;

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace CliTemplate;

public class LauncherModule : ILauncherModule
{
    private readonly ArgumentFactory<CommonArgs> _commonArgFactory;
    private CliSettings? _cliSettings;

    public Func<LauncherContext, Exception, int>? ExceptionHandler => OnException;
    public RootCommand RootCommand { get; }

    public LauncherModule()
    {
        RootCommand = new RootCommand("CliTemplate");
        _commonArgFactory = new ArgumentFactory<CommonArgs>(RootCommand);
    }

    public IConfiguration GetConfiguration()
    {
        return new ConfigurationBuilder()
            .AddJsonFile("appSettings.json")
            .AddJsonFile("secrets.user.json", optional: true)
            .AddJsonFile("secrets.json", optional: true)
            .AddJsonFile($"{Environment.GetFolderPath(Environment.SpecialFolder.UserProfile)}/{Assembly.GetEntryAssembly()?.GetName().Name ?? "cli"}.json", optional: true)
            .Build();
    }

    public void ConfigureRootServices(LauncherContext ctx)
    {
        ctx.Services.AddSingleton(new Settings());
        // the CommandRunner requires an ILogger
        ctx.Services.AddSingleton<ILogger>(provider => provider.GetRequiredService<ICliLogger>());
        ctx.Services.AddSingleton<ICommandRunner, CommandRunner>();

        _cliSettings = ctx.Configuration.GetRequiredSection(CliSettings.SectionName).Get<CliSettings>()!;
        ctx.Services.AddSingleton(_cliSettings);

        ctx.Services.AddScoped<GitShellCommands>();
        ctx.Services.AddSingleton(new DelayConfig(ctx.Configuration));
    }

    public void ConfigureInvocationContext(LauncherContext ctx, ParseResult parseResult)
    {
        var commonArgs = _commonArgFactory.Create(parseResult);
        if (string.IsNullOrEmpty(commonArgs.Verbosity))
        {
            commonArgs.Verbosity = _cliSettings!.DefaultVerbosity;
        }

        ctx.Services.AddSingleton(commonArgs);
        ctx.Logger.SetLogLevel(commonArgs.ToLogLevel());
    }

    private int OnException(LauncherContext ctx, Exception ex)
    {
        switch (ex)
        {
            case OperationCanceledException cEx:
                ctx.Logger.LogWarning($"The operation was aborted - {cEx.Message}");
                return ExitCodes.Aborted;
            case CommandFailureException cfEx:
                LogException(ctx.Logger, cfEx);
                return cfEx.ExitCode;
            default:
                LogException(ctx.Logger, ex);
                return ExitCodes.UnhandledException;
        }
    }

    private static void LogException(ICliLogger logger, Exception e)
    {
        if (logger.IsEnabled(LogLevel.Debug))
        {
            logger.LogError(e, e.ToString());
        }
        else if (logger.IsEnabled(LogLevel.Error))
        {
            logger.LogError(e.Message);
        }
    }
}
