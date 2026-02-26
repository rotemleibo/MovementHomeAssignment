namespace MovementHomeAssignment.API.Cache;

/// <summary>
/// Node for the LRU key linked list.
/// </summary>
public class Node
{
    /// <summary>
    /// Cached key value.
    /// </summary>
    public int Data;

    /// <summary>
    /// Next node reference.
    /// </summary>
    public Node Next;
    
    /// <summary>
    /// Previous node reference.
    /// </summary>
    public Node Previous { get; set; }

    /// <summary>
    /// Initializes a new instance of the Node class with the specified data.
    /// </summary>
    /// <param name="data">The cached key value.</param>
    public Node(int data)
    {
        Data = data;
    }
}