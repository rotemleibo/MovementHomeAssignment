using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;

namespace MovementHomeAssignment.API.Services;

public class InMemoryCache<T>
    where T : class
{
    private const int MinCapacity = 3;
    private const int MaxCapacity = 100;

    private readonly object _cacheLock = new object();
    private readonly int _capacity;

    private readonly Dictionary<int, T> _cache;
    private readonly LinkedList<int> _linkedList;

    public InMemoryCache(IOptions<InMemoryCacheOptions> options)
    {
        if (options.Value.Capacity < MinCapacity || options.Value.Capacity > MaxCapacity)
        {
            throw new ArgumentOutOfRangeException(nameof(options.Value.Capacity), $"Capacity must be at least {MinCapacity} and at most {MaxCapacity}, but was {options.Value.Capacity}.");
        }

        _capacity = options.Value.Capacity;
        _cache = new Dictionary<int, T>();
        _linkedList = new LinkedList<int>();
    }

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
                _cache.Remove(last.Value);
                _linkedList.RemoveLast();
            }

            _linkedList.AddFirst(key);
            _cache[key] = value;
        }
    }

    private void UpdateLeastRecentlyUsed(int key)
    {
        _linkedList.Remove(key);
        _linkedList.AddFirst(key);
    }
}
