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

    public Node(int data)
    {
        Data = data;
        Next = null;
    }
}
