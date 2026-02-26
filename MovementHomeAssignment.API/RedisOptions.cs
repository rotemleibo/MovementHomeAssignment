namespace MovementHomeAssignment.API;

/// <summary>
/// Configuration options for Redis cache behavior.
/// </summary>
public class RedisOptions
{
    /// <summary>
    /// Absolute TTL in minutes for cached entries.
    /// </summary>
    public int TTL = 5;
}