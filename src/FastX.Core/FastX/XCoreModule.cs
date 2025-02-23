using FastX.Data.DataFilters;
using FastX.Data.Repository;
using FastX.DistributedCache;
using FastX.Modularity;
using FastX.MultiTenancy;
using Microsoft.Extensions.DependencyInjection;

namespace FastX;

public class XDataModule : XModule
{
    public override void ConfigurationService(IServiceCollection services)
    {
        services.AddTransient(typeof(IRepository<>), typeof(XSugarRepository<>));
        services.AddSingleton(typeof(IDataFilter<>), typeof(DataFilter<>));

        services.AddSingleton<IConnectionMultiplexerFactory, ConnectionMultiplexerFactory>();

        services.AddSingleton<ICurrentTenantAccessor>(AsyncLocalCurrentTenantAccessor.Instance);

    }
}