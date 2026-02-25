using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MovementHomeAssignment.API.Abstract;
using MovementHomeAssignment.API.Services;

namespace MovementHomeAssignment.API;

public static class DependencyInjection
{
    public static IServiceCollection AddRedis(
    this IServiceCollection services,
    IConfiguration configuration)
    {
        services.Configure<RedisOptions>(configuration.GetSection("RedisOptions"));

        var redisConnectionString = configuration.GetConnectionString("Redis");

        services.AddStackExchangeRedisCache(options =>
        {
            options.Configuration = redisConnectionString;
        });

        services.AddScoped<ICacheService, RedisCacheService>();
        return services;
    }
}
