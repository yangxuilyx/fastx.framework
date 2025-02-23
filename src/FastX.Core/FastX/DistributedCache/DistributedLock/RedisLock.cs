using System.Text;
using FastX.DistributedCache;
using Microsoft.Extensions.DependencyInjection;
using StackExchange.Redis;

namespace FastX.DistributedCache.DistributedLock
{
    /// <summary>
    /// redis锁
    /// </summary>
    public abstract class RedisLock : IDisposable, IAsyncDisposable
    {
        IDatabase _database;

        /// <summary>
        /// 数据库
        /// </summary>
        protected IDatabase Database => _database;

        /// <summary>
        /// token值
        /// </summary>
        protected virtual RedisValue TokenValue => 1;

        /// <summary>
        /// 休眠时间
        /// </summary>
        protected virtual TimeSpan SleepTime => new TimeSpan(0, 0, 0, 0, 100);

        /// <summary>
        /// 默认超时时间(防死锁)
        /// </summary>
        protected virtual int TimeOutOfSec => 10;

        /// <summary>
        /// 加锁
        /// </summary>
        /// <param name="serviceProvider"></param>
        /// <returns></returns>
        internal async Task Lock(IServiceProvider serviceProvider)
        {
            var connectionMultiplexerFactory = serviceProvider.GetRequiredService<IConnectionMultiplexerFactory>();
            var connectionMultiplexer = await connectionMultiplexerFactory.GetConnectionMultiplexer();
            if (connectionMultiplexer == null)
                return;
            _database = connectionMultiplexer.GetDatabase();

            while (!await _database.LockTakeAsync(Key, TokenValue, new TimeSpan(0, 0, TimeOutOfSec)))
            {
                await Task.Delay(SleepTime);
            }
        }

        /// <summary>
        /// 数据键(可不填)
        /// </summary>
        public string DataKey { get; set; }

        /// <summary>
        /// 内存键
        /// </summary>
        public string Key
        {
            get
            {
                StringBuilder _key = new StringBuilder($"{GetType().FullName.Replace(".", ":")}");
                if (!DataKey.IsNullOrEmpty())
                    _key.Append($":{DataKey}");
                return _key.ToString();
            }
        }

        /// <summary>
        /// 释放资源
        /// </summary>
        /// <param name="dispose"></param>
        protected abstract void Dispose(bool dispose);

        /// <summary>
        /// 释放资源
        /// </summary>
        /// <param name="dispose"></param>
        /// <returns></returns>
        protected abstract ValueTask DisposeAsync(bool dispose);

        /// <summary>
        /// 释放资源
        /// </summary>
        public void Dispose()
        {
            if (_database == null)
                return;
            Dispose(true);
            _database.LockRelease(Key, TokenValue);
            _database = null;
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// 释放资源
        /// </summary>
        /// <returns></returns>
        public async ValueTask DisposeAsync()
        {
            if (_database == null)
                return;
            await DisposeAsync(true);
            await _database.LockReleaseAsync(Key, TokenValue);
            _database = null;
            GC.SuppressFinalize(this);
        }
    }
}
