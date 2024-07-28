using System.CommandLine;

using CliTemplate;
using CliTemplate.Status;

var launcher = new Launcher();

var rootCommand = new RootCommand("TODO: Custom CLI tool for ???")
{
    new StatusCommand(launcher.HandlerFactory),
};
CommonArgs.Declare(rootCommand);
rootCommand.Name = "CliTemplate";
// As of version 2.0.0-beta4.22272.1, the only way to allow additional arguments to actually work is to set the
// property on the ROOT command.
// See https://github.com/dotnet/command-line-api/blob/2.0.0-beta4.22272.1/src/System.CommandLine/Parsing/ParseResult.cs#L68
// This might change in a future release.
rootCommand.TreatUnmatchedTokensAsErrors = false;

return await launcher.InvokeAsync(rootCommand, args);
