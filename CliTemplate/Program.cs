using System.CommandLine;

using CliTemplate;
using CliTemplate.Status;

var launcher = new Launcher();

var rootCommand = new RootCommand("TODO: Custom CLI tool for ???")
{
    new StatusCommand(launcher.HandlerFactory),
};

return await launcher.InvokeAsync(rootCommand, args);
