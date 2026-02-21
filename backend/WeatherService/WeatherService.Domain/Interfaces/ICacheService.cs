namespace WeatherService.Domain.Interfaces
{
    // Contract for caching operations (Singleton Pattern will use this)
    public interface ICacheService
    {
        Task<T?> GetAsync<T>(string key);
        Task SetAsync<T>(string key, T value, TimeSpan expiration);
        Task RemoveAsync(string key);
    }
}