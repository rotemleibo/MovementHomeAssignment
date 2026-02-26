using System.Threading;
using System.Threading.Tasks;

namespace MovementHomeAssignment.API.Abstract;

/// <summary>
/// Abstraction for distributed cache operations.
/// </summary>
public interface ICacheService
{
    /// <summary>
    /// Retrieves a cached value by key.
    /// </summary>
    Task<T> GetAsync<T>(string key, CancellationToken ct = default);

    /// <summary>
    /// Stores a value using the configured expiration policy.
    /// </summary>
    Task SetAsync<T>(string key, T value, CancellationToken ct = default);
}