using Microsoft.Extensions.Caching.Memory;

namespace web_api.web.Services
{
    public class CacheService
    {
        private readonly IMemoryCache _memoryCache;

        public CacheService(IMemoryCache memoryCache)
        {
            _memoryCache = memoryCache;
        }

        public T? Get<T>(object key)
        {
            return _memoryCache.TryGetValue(key, out T? value) ? value : default;
        }

        public void Set<T>(object key, T value, MemoryCacheEntryOptions? options = null)
        {
            if (options != null)
                _memoryCache.Set(key, value, options);
            else
                _memoryCache.Set(key, value);
        }

        public void Remove(object key)
        {
            _memoryCache.Remove(key);
        }
    }
}
