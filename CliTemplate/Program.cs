using System.CommandLine;
using System.CommandLine.Builder;
using System.CommandLine.IO;
using System.CommandLine.Parsing;
using System.Reflection;

using CliTemplate;
using CliTemplate.Status;

using Microsoft.Extensions.Configuration;

var config = new ConfigurationBuilder()
    .AddJsonFile("appSettings.json")
    .AddJsonFile("secrets.user.json", optional: true)
    .AddJsonFile("secrets.json", optional: true)
    .AddJsonFile($"{Environment.GetFolderPath(Environment.SpecialFolder.UserProfile)}/{Assembly.GetEntryAssembly()?.GetName().Name ?? "cli"}.json", optional: true)
    .Build();

var handlerFactory = new HandlerFactory(config);

var rootCommand = new RootCommand("TODO: Custom CLI tool for ???")
{
    new StatusCommand(handlerFactory),
};

var parser = new CommandLineBuilder(rootCommand)
    .UseVersionOption()
    .UseHelp()
    .UseEnvironmentVariableDirective()
    .UseParseDirective()
    .UseSuggestDirective()
    .RegisterWithDotnetSuggest()
    .UseTypoCorrections()
    .UseParseErrorReporting()
    .UseExceptionHandler((ex, ctx) =>
    {
        if (ex is OperationCanceledException)
        {
            ctx.Console.WriteLine("The operation was aborted");
            ctx.ExitCode = ExitCodes.Aborted;
            return;
        }

        ctx.Console.Error.WriteLine(ex.ToString());
        ctx.ExitCode = ExitCodes.UnhandledException;
    })
    .CancelOnProcessTermination()
    .Build();

return await parser.InvokeAsync(args);
