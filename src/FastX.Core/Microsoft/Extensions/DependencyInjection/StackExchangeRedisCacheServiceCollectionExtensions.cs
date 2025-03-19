using System.Configuration;
using FastX.DistributedCache;

namespace Microsoft.Extensions.DependencyInjection;

public static class StackExchangeRedisCacheServiceCollectionExtensions
{
    public static IServiceCollection AddXStackExchangeRedisCache(
        this IServiceCollection services,
        string configuration
        )
    {
        services.AddStackExchangeRedisCache(options =>
        {
            options.Configuration = configuration;
            options.ConnectionMultiplexerFactory = () =>
                services.BuildServiceProvider().GetRequiredService<IConnectionMultiplexerFactory>().GetConnectionMultiplexer();
        });

        return services;
    }
}