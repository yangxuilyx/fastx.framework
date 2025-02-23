using Microsoft.Extensions.DependencyInjection;

namespace FastX;
public interface IXApplication : IDisposable
{
    Type StartupModuleType { get; }

    IServiceCollection Services { get; }

    IServiceProvider ServiceProvider { get; }

    Task ConfigureServicesAsync();
}