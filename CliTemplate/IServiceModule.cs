using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace CliTemplate;

public interface IServiceModule
{
    void ConfigureServices(IServiceCollection services, IConfigurationRoot config);
}
