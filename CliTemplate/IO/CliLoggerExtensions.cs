using System.CommandLine.Rendering;

namespace CliTemplate.IO;

public static class CliLoggerExtensions
{
    public static void LogContent(this ICliLogger logger, string message)
    {
        logger.WriteOutput(new ContentSpan(message));
    }
}
