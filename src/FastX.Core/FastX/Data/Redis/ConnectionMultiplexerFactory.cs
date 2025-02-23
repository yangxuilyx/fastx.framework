using Microsoft.Extensions.Caching.StackExchangeRedis;
using Microsoft.Extensions.Options;
using StackExchange.Redis;

namespace FastX.Data.Redis;

public class ConnectionMultiplexerFactory : IConnectionMultiplexerFactory, IDisposable, IAsyncDisposable
{
    protected IConnectionMultiplexer? Connection { get; private set; }
    private RedisCacheOptions _options;
    private bool _disposed = false;
    private readonly SemaphoreSlim _connectionLock = new(1, 1);

    public ConnectionMultiplexerFactory(IOptions<RedisCacheOptions> options)
    {
        _options = options.Value;
    }

    public async Task<IConnectionMultiplexer> GetConnectionMultiplexer()
    {
        CheckDisposed();

        var connection = Connection;
        if (connection != null)
            return connection;

        await _connectionLock.WaitAsync();

        try
        {
            if (Connection == null)
            {
                if (_options.Configuration == null)
                    throw new ArgumentException(nameof(_options.Configuration));

                Connection = await ConnectionMultiplexer.ConnectAsync(_options.Configuration);
            }
            return Connection;
        }
        finally
        {
            _connectionLock.Release();
        }
    }

    private void CheckDisposed()
    {
        ObjectDisposedException.ThrowIf(this._disposed, this);
    }

    public void Dispose()
    {
        if (_disposed)
            return;

        _disposed = true;
        Connection?.Dispose();
    }

    public async ValueTask DisposeAsync()
    {
        if (_disposed)
            return;

        _disposed = true;
        if (Connection != null) await Connection.DisposeAsync();
    }
}