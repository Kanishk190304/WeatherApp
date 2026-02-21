using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using StackExchange.Redis;
using WeatherService.Domain.Interfaces;

namespace WeatherService.Infrastructure.Caching
{
    // Singleton Pattern: Single instance of Redis cache service throughout application
    public class RedisCacheService : ICacheService
    {
        private readonly IDatabase _database;
        private static RedisCacheService? _instance;
        private static readonly object _lock = new object();

        // Private constructor for Singleton
        private RedisCacheService(string connectionString)
        {
            var connection = ConnectionMultiplexer.Connect(connectionString);
            _database = connection.GetDatabase();
        }

        // Singleton instance accessor
        public static RedisCacheService GetInstance(string connectionString)
        {
            if (_instance == null)
            {
                lock (_lock)
                {
                    if (_instance == null)
                    {
                        _instance = new RedisCacheService(connectionString);
                    }
                }
            }
            return _instance;
        }

        // Get value from cache
        public async Task<T?> GetAsync<T>(string key)
        {
            var value = await _database.StringGetAsync(key);
            
            if (value.IsNullOrEmpty)
            {
                return default;
            }

            return JsonConvert.DeserializeObject<T>(value!);
        }

        // Set value in cache with expiration
        public async Task SetAsync<T>(string key, T value, TimeSpan expiration)
        {
            var serialized = JsonConvert.SerializeObject(value);
            await _database.StringSetAsync(key, serialized, expiration);
        }

        // Remove value from cache
        public async Task RemoveAsync(string key)
        {
            await _database.KeyDeleteAsync(key);
        }
    }

    // Factory for creating singleton instance via DI
    public class RedisCacheServiceFactory
    {
        private readonly IConfiguration _configuration;

        public RedisCacheServiceFactory(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public ICacheService Create()
        {
            var connectionString = _configuration["Redis:ConnectionString"]!;
            return RedisCacheService.GetInstance(connectionString);
        }
    }
}