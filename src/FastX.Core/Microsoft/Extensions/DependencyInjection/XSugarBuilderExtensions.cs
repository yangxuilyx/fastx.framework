using FastX.Data.SqlSugar;
using Microsoft.Extensions.DependencyInjection.Extensions;
using SqlSugar;
using Check = FastX.Check;

namespace Microsoft.Extensions.DependencyInjection;

public static class XSugarBuilderExtensions
{
    public static XSugarBuilder UseDataCache(this XSugarBuilder builder, bool isAutoRemoveDataCache = true)
    {
        Check.NotNull(builder.SqlSugarClient, nameof(builder.SqlSugarClient));
        Check.NotNull(builder.Services, nameof(builder.Services));

        builder.Services.Configure<XSugarOptions>(t => t.EnableDataCache = true);

        builder.Services.TryAddSingleton<ICacheService, XSugarCacheService>();

        var cacheService = builder.Services.BuildServiceProvider().GetRequiredService<ICacheService>();

        builder.SqlSugarClient.CurrentConnectionConfig.ConfigureExternalServices ??= new ConfigureExternalServices();
        builder.SqlSugarClient.CurrentConnectionConfig.ConfigureExternalServices.DataInfoCacheService = cacheService;

        builder.SqlSugarClient.CurrentConnectionConfig.MoreSettings ??= new ConnMoreSettings();
        builder.SqlSugarClient.CurrentConnectionConfig.MoreSettings.IsAutoRemoveDataCache = isAutoRemoveDataCache;

        return builder;
    }
}