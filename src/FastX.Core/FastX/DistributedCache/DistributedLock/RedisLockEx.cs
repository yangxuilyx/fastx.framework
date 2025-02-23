namespace FastX.DistributedCache.DistributedLock
{
    /// <summary>
    /// redis锁扩展
    /// </summary>
    public static class RedisLockEx
    {
        /// <summary>
        /// 创建redis分布式锁
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="serviceProvider">服务接口</param>
        /// <param name="action">扩展</param>
        /// <returns></returns>
        public static async Task<T> CreateRedisLock<T>(
            this IServiceProvider serviceProvider,
            Action<T>? action = null) where T : RedisLock, new()
        {
            var redis = new T();
            if (action != null)
                action(redis);
            await redis.Lock(serviceProvider);
            return redis;
        }
    }
}
