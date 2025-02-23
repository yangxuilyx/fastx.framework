using StackExchange.Redis;

namespace FastX.DistributedCache;

public interface IConnectionMultiplexerFactory
{
    Task<IConnectionMultiplexer> GetConnectionMultiplexer();
}