using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MovementHomeAssignment.Abstract;
using MovementHomeAssignment.API.Abstract;
using MovementHomeAssignment.API.InMemory;
using MovementHomeAssignment.API.Services;
using MovementHomeAssignment.Converters;
using MovementHomeAssignment.DTOs;

namespace MovementHomeAssignment.API;

/// <summary>
/// Provides extension methods for configuring dependency injection services.
/// </summary>
public static class DependencyInjection
{
    /// <summary>
    /// Registers in-memory cache services and configures cache capacity options from configuration.
    /// </summary>
    /// <param name="services">The service collection to add caches to.</param>
    /// <param name="configuration">The application configuration containing cache settings.</param>
    /// <returns>The service collection for method chaining.</returns>
    public static IServiceCollection AddInMemoryCaches(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        services.Configure<InMemoryCacheOptions>(configuration.GetSection("InMemoryCache"));
        services.AddSingleton<InMemoryCache<UserDto>>();

        return services;
    }

    /// <summary>
    /// Registers Redis cache services and configures the Redis connection from configuration.
    /// </summary>
    /// <param name="services">The service collection to add Redis services to.</param>
    /// <param name="configuration">The application configuration containing Redis connection settings.</param>
    /// <returns>The service collection for method chaining.</returns>
    public static IServiceCollection AddRedis(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        services.Configure<RedisOptions>(configuration.GetSection("RedisOptions"));

        services.AddStackExchangeRedisCache(options =>
        {
            options.Configuration = configuration.GetConnectionString("Redis");
        });

        services.AddScoped<ICacheService, RedisCacheService>();

        return services;
    }

    /// <summary>
    /// Registers application services for dependency injection.
    /// </summary>
    /// <param name="services">The service collection to add services to.</param>
    /// <returns>The service collection for method chaining.</returns>
    public static IServiceCollection AddServices(
        this IServiceCollection services)
    {
        services.AddScoped<IUserService, UserService>();

        return services;
    }

    /// <summary>
    /// Registers converter services for dependency injection.
    /// </summary>
    /// <param name="services">The service collection to add converters to.</param>
    /// <returns>The service collection for method chaining.</returns>
    public static IServiceCollection AddConverters(
        this IServiceCollection services)
    {
        services.AddScoped<IUserConverter, UserConverter>();

        return services;
    }

}
