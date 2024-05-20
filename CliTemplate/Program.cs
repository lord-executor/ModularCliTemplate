using System.CommandLine;
using System.CommandLine.Builder;
using System.CommandLine.Parsing;

using CliTemplate;
using CliTemplate.Status;

var handlerFactory = new HandlerFactory();
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
            Console.WriteLine("The operation was aborted");
            ctx.ExitCode = ExitCodes.Aborted;
            return;
        }

        Console.Error.WriteLine(ex.ToString());
        ctx.ExitCode = ExitCodes.UnhandledException;
    })
    .CancelOnProcessTermination()
    .Build();

return await parser.InvokeAsync(args);
