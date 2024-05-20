using System.CommandLine.Invocation;

namespace CliTemplate;

public interface ISimpleHandler<TArg>
{
    Task<int> RunAsync(InvocationContext context, TArg args, CancellationToken cancellationToken = default);
}
