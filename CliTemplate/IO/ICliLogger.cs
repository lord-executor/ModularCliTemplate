using System.CommandLine;
using System.CommandLine.Rendering;

using Microsoft.Extensions.Logging;

namespace CliTemplate.IO;

public interface ICliLogger : ILogger
{
    IConsole Console { get; }
    void WriteOutput(TextSpan span);
    void WriteError(TextSpan span);
    string? Prompt(string promptLine);

    bool PromptYesNo(string promptLine)
    {
        return Prompt($"{promptLine} [y(es)/n(o)]:")?.StartsWith('y') ?? false;
    }
}
