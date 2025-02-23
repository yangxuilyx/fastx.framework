using Microsoft.Extensions.DependencyInjection;

namespace FastX.Modularity;

public interface IXModule
{
    /// <summary>
    /// ConfigurationServiceAsync
    /// </summary>
    /// <param name="services"></param>
    /// <returns></returns>
    Task ConfigurationServiceAsync(IServiceCollection services);

    /// <summary>
    /// ConfigurationService
    /// </summary>
    /// <param name="services"></param>
    void ConfigurationService(IServiceCollection services);

    /// <summary>
    /// PostConfigureServices
    /// </summary>
    /// <param name="services"></param>
    void PostConfigureServices(IServiceCollection services);
}