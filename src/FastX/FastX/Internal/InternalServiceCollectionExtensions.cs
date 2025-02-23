using FastX.Modularity;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace FastX.Internal;

internal static class InternalServiceCollectionExtensions
{
    internal static void AddCoreServices(this IServiceCollection services)
    {
        services.AddOptions();
        services.AddLogging();
        var moduleLoader = new ModuleLoader();
        services.TryAddSingleton<IModuleLoader>(moduleLoader);
    }
}
