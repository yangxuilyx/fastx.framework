using Microsoft.Extensions.Caching.Distributed;
using SqlSugar;
using System.Reflection.PortableExecutable;
using System.Text;
using System.Text.Json;
using Microsoft.AspNetCore.Shared;
using StackExchange.Redis;
using Newtonsoft.Json.Linq;
using FastX.DistributedCache;

namespace FastX.Data.SqlSugar;

public class XSugarCacheService(IConnectionMultiplexerFactory connectionMultiplexerFactory) : ICacheService, IDisposable
{
    protected IConnectionMultiplexerFactory ConnectionMultiplexerFactory { get; } = connectionMultiplexerFactory;
    private volatile IDatabase? _cache;

    public static readonly string HashKey = "SqlSugarDataCache:HashKey";

    private IDatabase Connect()
    {
        if (_cache != null)
            return _cache;

        var connection =
            ConnectionMultiplexerFactory.GetConnectionMultiplexer().GetAwaiter().GetResult();
        _cache = connection.GetDatabase();

        return _cache;
    }


    private string GetHashKey(string key)
    {
        var keys = key.Split(".");
        if (keys.Length < 2)
            throw new ArgumentException($"sugar key is invalid,key: {key}");

        return $"{keys[0]}:{keys[1]}";
    }

    private void SaveHashKey(
        string key)
    {
        var cache = Connect();

        key = key.Split(":").JoinAsString(".") + ".";

        cache.HashSet(HashKey, key, 1);
    }


    public void Add<V>(string key, V value)
    {
        var cache = Connect();

        var redisKey = GetHashKey(key);
        SaveHashKey(redisKey);
        cache.HashSet(redisKey, key, JsonSerializer.Serialize(value));
    }

    public void Add<V>(string key, V value, int cacheDurationInSeconds)
    {
        var cache = Connect();

        var redisKey = GetHashKey(key);
        SaveHashKey(redisKey);
        cache.HashSet(redisKey, key, JsonSerializer.Serialize(value));
    }

    public bool ContainsKey<V>(string key)
    {
        var cache = Connect();
        return cache.HashExists(GetHashKey(key), key);
    }

    public V? Get<V>(string key)
    {
        var cache = Connect();

        var value = cache.HashGet(GetHashKey(key), key);
        if (value.HasValue)
            return JsonSerializer.Deserialize<V>(value!);

        return default;
    }

    public IEnumerable<string>? GetAllKey<V>()
    {
        var cache = Connect();

        return cache.HashGetAll(HashKey).Select(t => t.Name.ToString()).ToList();
    }

    public void Remove<V>(string key)
    {
        var cache = Connect();

        var redisKey = GetHashKey(key);
        cache.KeyDelete(redisKey);

        cache.HashDelete(HashKey, key);
    }

    public V? GetOrCreate<V>(string cacheKey, Func<V> create, int cacheDurationInSeconds = 2147483647)
    {
        if (!ContainsKey<V>(cacheKey))
        {
            var value = create();
            Add(cacheKey, value, cacheDurationInSeconds);
            return value;
        }

        return Get<V>(cacheKey);
    }

    public void Dispose()
    {
        _cache?.Multiplexer.Dispose();
    }
}