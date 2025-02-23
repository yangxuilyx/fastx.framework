using System.Diagnostics.CodeAnalysis;
using FastX.Modularity;
using Microsoft.Extensions.DependencyInjection;

namespace FastX;

public static class XApplicationFactory
{
    public static async Task<IXApplication> CreateAsync<TStartupModule>(
        [NotNull] IServiceCollection services)
        where TStartupModule : IXModule
    {
        var app = Create(typeof(TStartupModule), services);
        await app.ConfigureServicesAsync();
        return app;
    }

    public static IXApplication Create(
        [NotNull] Type startupModuleType,
        [NotNull] IServiceCollection services)
    {
        return new XApplicationBase(startupModuleType, services);
    }
}