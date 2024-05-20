using System.CommandLine;

namespace CliTemplate.Status;

public class StatusArgs
{
    public bool Ignored { get; set; }

    public static void Declare(Command command)
    {
        command.AddOption(new Option<bool>("--ignored"));
    }
}
