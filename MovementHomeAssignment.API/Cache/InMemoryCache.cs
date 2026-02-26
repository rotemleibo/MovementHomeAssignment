using Microsoft.Extensions.Options;
using MovementHomeAssignment.API.Cache;
using System;
using System.Collections.Generic;

namespace MovementHomeAssignment.API.InMemory;

/// <summary>
/// In-memory LRU cache optimized for fast lookups.
/// </summary>
public class InMemoryCache<T>
    where T : class
{
    private const int MinCapacity = 3;
    private const int MaxCapacity = 100;

    private readonly object _cacheLock = new object();
    private readonly int _capacity;

    private readonly Dictionary<int, T> _cache;
    private readonly CacheKeyLinkedList _linkedList;

    /// <summary>
    /// Initializes a new instance of the InMemoryCache class with the specified capacity options.
    /// </summary>
    /// <param name="options">Configuration options containing the cache capacity.</param>
    /// <exception cref="ArgumentOutOfRangeException">Thrown when capacity is less than MinCapacity or greater than MaxCapacity.</exception>
    public InMemoryCache(IOptions<InMemoryCacheOptions> options)
    {
        if (options.Value.Capacity < MinCapacity || options.Value.Capacity > MaxCapacity)
        {
            throw new ArgumentOutOfRangeException(nameof(options.Value.Capacity), $"Capacity must be at least {MinCapacity} and at most {MaxCapacity}, but was {options.Value.Capacity}.");
        }

        _capacity = options.Value.Capacity;
        _cache = new Dictionary<int, T>();
        _linkedList = new CacheKeyLinkedList();
    }

    /// <summary>
    /// Retrieves a cached value and updates LRU ordering.
    /// </summary>
    public T Get(int key)
    {
        lock (_cacheLock)
        {
            if (_cache.TryGetValue(key, out var existingValue))
            {
                UpdateLeastRecentlyUsed(key);
                return existingValue;
            }
        }
        return null;
    }

    /// <summary>
    /// Inserts or replaces a cached value and evicts when at capacity.
    /// </summary>
    public void Set(int key, T value)
    {
        lock (_cacheLock)
        {
            if (_cache.TryGetValue(key, out _))
            {
                _cache[key] = value;
                UpdateLeastRecentlyUsed(key);
                return;
            }

            if (_linkedList.Count == _capacity)
            {
                var last = _linkedList.Last;
                _cache.Remove(last);
                _linkedList.RemoveLast();
            }

            _linkedList.AddFirst(key);
            _cache[key] = value;
        }
    }

    /// <summary>
    /// Updates the key to be most-recently used.
    /// </summary>
    private void UpdateLeastRecentlyUsed(int key)
    {
        _linkedList.Remove(key);
        _linkedList.AddFirst(key);
    }
}
