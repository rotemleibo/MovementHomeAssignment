using Microsoft.Extensions.Options;
using MovementHomeAssignment.API;
using MovementHomeAssignment.API.InMemory;
using MovementHomeAssignment.DTOs;
using System;

namespace MovementHomeAssignment.Tests;

[TestClass]
public class InMemoryCacheTests
{
    [TestMethod]
    public void Constructor_CapacityBelowMinimum_ThrowsArgumentOutOfRangeException()
    {
        Assert.ThrowsExactly<ArgumentOutOfRangeException>(() => CreateCache(2));
    }

    [TestMethod]
    public void Constructor_CapacityAboveMaximum_ThrowsArgumentOutOfRangeException()
    {
        Assert.ThrowsExactly<ArgumentOutOfRangeException>(() => CreateCache(101));
    }

    [TestMethod]
    public void Constructor_CapacityAtMinimum_DoesNotThrow()
    {
        var cache = CreateCache(3);
        Assert.IsNotNull(cache);
    }

    [TestMethod]
    public void Constructor_CapacityAtMaximum_DoesNotThrow()
    {
        var cache = CreateCache(100);
        Assert.IsNotNull(cache);
    }

    [TestMethod]
    public void Get_NonExistentKey_ReturnsNull()
    {
        var cache = CreateCache(3);
        Assert.IsNull(cache.Get(999));
    }

    [TestMethod]
    public void Set_ThenGet_ReturnsStoredValue()
    {
        var cache = CreateCache(3);
        var user = CreateUser(1);
        cache.Set(1, user);
        Assert.AreSame(user, cache.Get(1));
    }

    [TestMethod]
    public void Set_SameKeyTwice_UpdatesValue()
    {
        var cache = CreateCache(3);
        var original = CreateUser(1);
        var updated = CreateUser(1);
        updated.LastName = "Updated";
        cache.Set(1, original);
        cache.Set(1, updated);
        Assert.AreSame(updated, cache.Get(1));
    }

    [TestMethod]
    public void Set_ExceedsCapacity_EvictsLeastRecentlyUsedItem()
    {
        var cache = CreateCache(3);
        cache.Set(1, CreateUser(1));
        cache.Set(2, CreateUser(2));
        cache.Set(3, CreateUser(3));
        cache.Set(4, CreateUser(4)); // should evict key 1

        Assert.IsNull(cache.Get(1));
        Assert.AreEqual(2, cache.Get(2)?.Id);
        Assert.AreEqual(3, cache.Get(3)?.Id);
        Assert.AreEqual(4, cache.Get(4)?.Id);
    }

    [TestMethod]
    public void Get_RefreshesItemOrder_PreventsEviction()
    {
        var cache = CreateCache(3);
        cache.Set(1, CreateUser(1));
        cache.Set(2, CreateUser(2));
        cache.Set(3, CreateUser(3));

        // Access key 1 so it becomes most recently used
        cache.Get(1);

        cache.Set(4, CreateUser(4)); // should evict key 2 (now the least recently used)

        Assert.AreEqual(1, cache.Get(1)?.Id);
        Assert.IsNull(cache.Get(2));
        Assert.AreEqual(3, cache.Get(3)?.Id);
        Assert.AreEqual(4, cache.Get(4)?.Id);
    }

    [TestMethod]
    public void Set_ExistingKey_RefreshesItemOrder_PreventsEviction()
    {
        var cache = CreateCache(3);
        cache.Set(1, CreateUser(1));
        cache.Set(2, CreateUser(2));
        cache.Set(3, CreateUser(3));

        // Update key 1 so it becomes most recently used
        var updated = CreateUser(1);
        updated.LastName = "Updated";
        cache.Set(1, updated);

        cache.Set(4, CreateUser(4)); // should evict key 2

        Assert.AreSame(updated, cache.Get(1));
        Assert.IsNull(cache.Get(2));
        Assert.AreEqual(3, cache.Get(3)?.Id);
        Assert.AreEqual(4, cache.Get(4)?.Id);
    }

    [TestMethod]
    public void Set_MultipleEvictions_OnlyKeepsCapacityItems()
    {
        var cache = CreateCache(3);
        for (int i = 1; i <= 6; i++)
            cache.Set(i, CreateUser(i));

        // Only keys 4, 5, 6 should remain
        for (int i = 1; i <= 3; i++)
            Assert.IsNull(cache.Get(i));

        for (int i = 4; i <= 6; i++)
            Assert.AreEqual(i, cache.Get(i)?.Id);
    }
    private static InMemoryCache<UserDto> CreateCache(int capacity) =>
    new(Options.Create(new InMemoryCacheOptions { Capacity = capacity }));

    private static UserDto CreateUser(int id) =>
        new() { Id = id, FirstName = $"First{id}", LastName = $"Last{id}" };
}