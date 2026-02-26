using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Options;
using MovementHomeAssignment.API.Abstract;
using System;
using System.Text;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;

namespace MovementHomeAssignment.API.Services;

/// <summary>
/// Redis-backed cache implementation using JSON serialization.
/// </summary>
public sealed class RedisCacheService : ICacheService
{
    private readonly IDistributedCache _cache;
    private readonly RedisOptions _options;

    public RedisCacheService(IDistributedCache cache, IOptions<RedisOptions> options)
    {
        _cache = cache;
        _options = options.Value;
    }

    /// <summary>
    /// Retrieves a value from Redis and deserializes it from JSON.
    /// </summary>
    public async Task<T> GetAsync<T>(string key, CancellationToken ct = default)
    {
        var bytes = await _cache.GetAsync(key, ct);
        if (bytes is null) return default;

        var json = Encoding.UTF8.GetString(bytes);
        return JsonSerializer.Deserialize<T>(json);
    }

    /// <summary>
    /// Stores a value in Redis using a TTL configured in options.
    /// </summary>
    public async Task SetAsync<T>(string key, T value, CancellationToken ct = default)
    {
        var json = JsonSerializer.Serialize(value);
        var bytes = Encoding.UTF8.GetBytes(json);

        var options = new DistributedCacheEntryOptions
        {
            AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(_options.TTL)
        };

        await _cache.SetAsync(key, bytes, options, ct);
    }
}