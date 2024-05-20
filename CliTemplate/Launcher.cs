using System.CommandLine;
using System.CommandLine.Builder;
using System.CommandLine.Invocation;
using System.CommandLine.IO;
using System.CommandLine.Parsing;
using System.Reflection;

using Microsoft.Extensions.Configuration;

namespace CliTemplate;

public class Launcher
{
    public HandlerFactory HandlerFactory { get; }

    public Launcher()
    {
        IConfigurationRoot config = new ConfigurationBuilder()
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
        if (ex is OperationCanceledException)
        {
            ctx.Console.WriteLine("The operation was aborted");
            ctx.ExitCode = ExitCodes.Aborted;
            return;
        }

        ctx.Console.Error.WriteLine(ex.ToString());
        ctx.ExitCode = ExitCodes.UnhandledException;
    }
}
