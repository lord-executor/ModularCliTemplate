using System.CommandLine;

namespace CliTemplate.GitInfo;

public class GitInfoArgs
{
    public string? Path { get; set; }

    public static void Declare(Command command)
    {
        command.AddOption(new Option<string>("--path", "Path to git repository"));
    }
}
