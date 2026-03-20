using System.Text.Json;
using EcommerceEnterprise.Application.Common.Interfaces;
using Microsoft.Extensions.Caching.Distributed;

namespace EcommerceEnterprise.Infrastructure.Caching;

public class RedisCacheService(IDistributedCache cache) : ICacheService
{
    private static readonly JsonSerializerOptions _options = new()
    {
        PropertyNameCaseInsensitive = true
    };

    public async Task<T?> GetAsync<T>(
        string key, CancellationToken ct = default)
    {
        var data = await cache.GetStringAsync(key, ct);
        return data is null
            ? default
            : JsonSerializer.Deserialize<T>(data, _options);
    }

    public async Task SetAsync<T>(
        string key, T value,
        TimeSpan? expiry = null,
        CancellationToken ct = default)
    {
        var options = new DistributedCacheEntryOptions
        {
            AbsoluteExpirationRelativeToNow =
                expiry ?? TimeSpan.FromMinutes(30)
        };

        await cache.SetStringAsync(
            key,
            JsonSerializer.Serialize(value, _options),
            options,
            ct);
    }

    public Task RemoveAsync(string key, CancellationToken ct = default)
        => cache.RemoveAsync(key, ct);

    public Task RemoveByPrefixAsync(
        string prefix, CancellationToken ct = default)
    {
        // Production: dùng Redis SCAN
        // Development: đơn giản hóa
        return Task.CompletedTask;
    }
}