using System;
using System.Threading;
using System.Threading.Tasks;

namespace MovementHomeAssignment.API.Abstract;

public interface ICacheService
{
    Task<T> GetAsync<T>(string key, CancellationToken ct = default);
    Task SetAsync<T>(string key, T value, CancellationToken ct = default);
}

