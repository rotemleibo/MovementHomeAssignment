namespace MovementHomeAssignment.API
{
    /// <summary>
    /// Configuration options for the in-memory cache.
    /// </summary>
    public class InMemoryCacheOptions
    {
        /// <summary>
        /// Gets or sets the maximum number of items that can be stored in the cache.
        /// </summary>
        public int Capacity { get; set; } = 3;
    }
}
