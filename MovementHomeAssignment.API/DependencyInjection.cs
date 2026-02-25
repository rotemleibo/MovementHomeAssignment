using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MovementHomeAssignment.API.Abstract;
using MovementHomeAssignment.API.Services;
using StackExchange.Redis;

namespace MovementHomeAssignment.API;

public static class DependencyInjection
{
    public static IServiceCollection AddRedis(
    this IServiceCollection services,
    IConfiguration configuration)
    {
        var redisConnectionString = configuration.GetConnectionString("Redis");

        services.AddStackExchangeRedisCache(options =>
        {
            options.Configuration = redisConnectionString;
        });

        services.AddScoped<ICacheService, RedisCacheService>();
        services.AddScoped<RedisOptions>(sp =>
        {
            return new RedisOptions
            {
                TTL = configuration.GetValue<int>("Redis:TTL")
            };
        });

        return services;
    }
}
