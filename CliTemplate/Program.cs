using CliTemplate;
using CliTemplate.GitInfo;
using CliTemplate.Status;

using Larcanum.ShellToolkit.Terminal;


var bootModule = new LauncherModule();
var launcher = new Launcher(bootModule);

bootModule.RootCommand.Add(launcher.Register(StatusCommand.Def));
bootModule.RootCommand.Add(launcher.Register(GitInfoCommand.Def));

return await launcher.RunAsync(bootModule.RootCommand, args);
