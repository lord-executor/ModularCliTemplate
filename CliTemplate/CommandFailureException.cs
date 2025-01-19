namespace CliTemplate;

public class CommandFailureException : Exception
{
    public int ExitCode { get; }

    public CommandFailureException(int exitCode)
    {
        ExitCode = exitCode;
    }

    public CommandFailureException(int exitCode, string message) : base(message)
    {
        ExitCode = exitCode;
    }

    public CommandFailureException(int exitCode, string message, Exception inner) : base(message, inner)
    {
        ExitCode = exitCode;
    }
}
