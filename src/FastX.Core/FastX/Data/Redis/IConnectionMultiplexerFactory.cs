using StackExchange.Redis;

namespace FastX.Data.Redis;

public interface IConnectionMultiplexerFactory
{
    Task<IConnectionMultiplexer> GetConnectionMultiplexer();
}