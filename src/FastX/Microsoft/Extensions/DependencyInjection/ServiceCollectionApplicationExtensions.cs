using FastX;
using FastX.Modularity;

namespace Microsoft.Extensions.DependencyInjection;

public static class ServiceCollectionApplicationExtensions
{
    public static async Task<IXApplication> AddApplicationAsync<TStartupModule>(
        this IServiceCollection services
        )
        where TStartupModule : IXModule
    {
        return await XApplicationFactory.CreateAsync<TStartupModule>(services);
    }
}