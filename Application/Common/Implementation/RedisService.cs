using Application.Common.Interfaces;
using Application.Configuration;
using Microsoft.Extensions.Configuration;
using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Application.Common.Implementation
{
    public class RedisService : IRedisService
    {
        private readonly IDatabase _database;
        private readonly RedisOptions _options;

        public RedisService(IConfiguration configuration)
        {
            var options = new RedisOptions();
            configuration.GetSection("RedisOptions").Bind(options);
            _options = options;

            var redis = ConnectionMultiplexer.Connect(_options.Configuration);
            _database = redis.GetDatabase();
        }

        public async Task SetAsync<T>(string key, T value, TimeSpan? expiry = null)
        {
            var json = JsonSerializer.Serialize(value);
            await _database.StringSetAsync(key, json, expiry ?? TimeSpan.FromMinutes(_options.DataExpiryMinutes));
        }

        public async Task<T?> GetAsync<T>(string key)
        {
            var value = await _database.StringGetAsync(key);
            if (value.IsNullOrEmpty) return default;
            return JsonSerializer.Deserialize<T>(value);
        }

        public async Task RemoveAsync(string key)
        {
            await _database.KeyDeleteAsync(key);
        }
    }
}
