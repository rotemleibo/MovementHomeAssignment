using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MovementHomeAssignment.Abstract;
using MovementHomeAssignment.API.Abstract;
using MovementHomeAssignment.API.InMemory;
using MovementHomeAssignment.API.Services;
using MovementHomeAssignment.Converters;
using MovementHomeAssignment.DTOs;

namespace MovementHomeAssignment.API;

public static class DependencyInjection
{
    public static IServiceCollection AddInMemoryCaches(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        services.Configure<InMemoryCacheOptions>(configuration.GetSection("InMemoryCache"));

        // Add here all the in-memory caches for different DTOs
        services.AddSingleton<InMemoryCache<UserDto>>();

        return services;
    }
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

    public static IServiceCollection AddServices(
        this IServiceCollection services)
    {
        // Add here all the services for different DTOs
        services.AddScoped<IUserService, UserService>();

        return services;
    }

    public static IServiceCollection AddConverters(
        this IServiceCollection services)
    {
        // Add here all the converters
        services.AddScoped<IUserConverter, UserConverter>();

        return services;
    }

}
