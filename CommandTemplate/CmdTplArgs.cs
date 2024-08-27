using System.CommandLine;

namespace CmdNs;

public class CmdTplArgs
{
    public string Name { get; set; } = "World";

    public static void Declare(Command command)
    {
        command.AddOption(new Option<string>("--name", () => "World"));
    }
}
