using System.CommandLine;
using System.CommandLine.Builder;
using System.CommandLine.Invocation;
using System.CommandLine.Parsing;
using System.Reflection;

using CliTemplate.IO;

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace CliTemplate;

public class Launcher
{
    public HandlerFactory HandlerFactory { get; }

    public Launcher()
    {
        var config = new ConfigurationBuilder()
            .AddJsonFile("appSettings.json")
            .AddJsonFile("secrets.user.json", optional: true)
            .AddJsonFile("secrets.json", optional: true)
            .AddJsonFile($"{Environment.GetFolderPath(Environment.SpecialFolder.UserProfile)}/{Assembly.GetEntryAssembly()?.GetName().Name ?? "cli"}.json", optional: true)
            .Build();

        HandlerFactory = new HandlerFactory(config);
    }

    public async Task<int> InvokeAsync(RootCommand rootCommand, string[] args)
    {
        var parser = new CommandLineBuilder(rootCommand)
            // Basically the same as UseDefaults() but with a custom exception handler
            .UseVersionOption()
            .UseHelp()
            .UseEnvironmentVariableDirective()
            .UseParseDirective()
            .UseSuggestDirective()
            .RegisterWithDotnetSuggest()
            .UseTypoCorrections()
            .UseParseErrorReporting()
            // Custom exception handler
            .UseExceptionHandler(OnException)
            .CancelOnProcessTermination()
            .Build();

        return await parser.InvokeAsync(args);
    }

    private static void OnException(Exception ex, InvocationContext ctx)
    {
        var commonArgs = ctx.BindingContext.GetService<CommonArgs>();
        var logLevel = commonArgs?.ToLogLevel() ?? LogLevel.Information;
        var logger = new CliLogger(ctx.Console, logLevel);

        if (ex is OperationCanceledException canceledException)
        {
            logger.LogWarning($"The operation was aborted - {canceledException.Message}");
            ctx.ExitCode = ExitCodes.Aborted;
            return;
        }

        LogException(logger, logLevel, ex);
        ctx.ExitCode = ExitCodes.UnhandledException;
    }

    private static void LogException(ICliLogger logger, LogLevel logLevel, Exception e)
    {
        if (logLevel <= LogLevel.Debug)
        {
            logger.LogError(e, e.ToString());
        }
        else if (logLevel <= LogLevel.Error)
        {
            logger.LogError(e.Message);
        }
    }
}
