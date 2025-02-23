using FastX.Modularity;
using FastX.MultiTenancy;
using Microsoft.Extensions.DependencyInjection;

namespace FastX;

public class FastXModule : XModule
{
    /// <summary>
    /// ConfigurationService
    /// </summary>
    /// <param name="services"></param>
    public override void ConfigurationService(IServiceCollection services)
    {
        services.AddSingleton<ICurrentTenantAccessor>(AsyncLocalCurrentTenantAccessor.Instance);
    }
}
