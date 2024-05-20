using System.CommandLine;

namespace CliTemplate.Status;

public class StatusCommand : Command
{
    public StatusCommand(HandlerFactory handlerFactory)
        : base("status", "Gets the status")
    {
        StatusArgs.Declare(this);
        Handler = handlerFactory.SimpleHandler<StatusHandler, StatusArgs>();
    }
}
