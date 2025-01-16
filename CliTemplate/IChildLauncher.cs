using System.CommandLine.Invocation;

namespace CliTemplate;

public interface IChildLauncher
{
    public Task<int> RunAsync<TArg>(InvocationContext ctx, TArg arg, CancellationToken ct);
}
