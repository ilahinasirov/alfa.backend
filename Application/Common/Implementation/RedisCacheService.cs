using Application.Common.Interfaces;
using MessagePack;
using StackExchange.Redis;

namespace Application.Common.Implementation
{
    public class RedisCacheService(IDatabase database)
    {
        public async Task<T?> GetAsync<T>(string key)
        {
            var redisValue = await database.StringGetAsync(key);
            if (redisValue.IsNullOrEmpty)
                return default;

            return MessagePackSerializer.Deserialize<T>(redisValue);
        }

        public async Task SetAsync<T>(string key, T value)
        {
            var serializedData = MessagePackSerializer.Serialize(value);
            await database.StringSetAsync(key, serializedData);
        }

        public Task RemoveAsync(string key) => database.KeyDeleteAsync(key);
    }
}
