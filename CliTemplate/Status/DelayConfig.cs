using Microsoft.Extensions.Configuration;

namespace CliTemplate.Status;

public class DelayConfig
{
    public int Delay { get; }

    public DelayConfig(IConfiguration config)
    {
        Delay = config.GetValue<int>("Delay");
    }
}
