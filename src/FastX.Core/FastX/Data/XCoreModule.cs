using FastX.Data.DataFilters;
using FastX.Data.Redis;
using FastX.Data.Repository;
using FastX.Modularity;
using Microsoft.Extensions.DependencyInjection;

namespace FastX.Data;

[DependsOn(typeof(FastXModule))]
public class XDataModule : XModule
{
    public override void ConfigurationService(IServiceCollection services)
    {
        services.AddTransient(typeof(IRepository<>), typeof(XSugarRepository<>));
        services.AddSingleton(typeof(IDataFilter<>), typeof(DataFilter<>));

        services.AddSingleton<IConnectionMultiplexerFactory, ConnectionMultiplexerFactory>();
    }
}