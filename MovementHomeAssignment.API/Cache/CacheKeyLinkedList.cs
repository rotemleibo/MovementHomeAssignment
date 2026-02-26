using System;
using System.Collections.Generic;

namespace MovementHomeAssignment.API.Cache;

/// <summary>
/// Linked list that tracks LRU order by cache key.
/// </summary>
public class CacheKeyLinkedList
{
    private Node head;
    private Node tail;
    private int count;
    private readonly Dictionary<int, Node> nodeMap = new();

    /// <summary>
    /// Current number of keys tracked.
    /// </summary>
    public int Count => count;

    /// <summary>
    /// Adds a key to the head (most-recently used).
    /// </summary>
    public void AddFirst(int data)
    {
        Node newNode = new Node(data);
        if (head == null)
        {
            head = tail = newNode;
        }
        else
        {
            newNode.Next = head;
            head.Previous = newNode;
            head = newNode;
        }
        nodeMap[data] = newNode;
        count++;
    }

    /// <summary>
    /// Adds a key to the tail (least-recently used).
    /// </summary>
    public void AddLast(int data)
    {
        Node newNode = new Node(data);
        if (head == null)
        {
            head = tail = newNode;
        }
        else
        {
            tail.Next = newNode;
            newNode.Previous = tail;
            tail = newNode;
        }
        nodeMap[data] = newNode;
        count++;
    }

    /// <summary>
    /// Gets the least-recently used key.
    /// </summary>
    public int Last => tail != null ? tail.Data : throw new InvalidOperationException("Empty");

    /// <summary>
    /// Removes the least-recently used key.
    /// </summary>
    public void RemoveLast()
    {
        if (head == null)
        {
            return;
        }
        if (head == tail)
        {
            nodeMap.Remove(tail.Data);
            head = tail = null;
        }
        else
        {
            nodeMap.Remove(tail.Data);
            tail = tail.Previous;
            tail.Next = null;
        }
        count--;
    }

    /// <summary>
    /// Removes a key if it exists. O(1) operation.
    /// </summary>
    public bool Remove(int data)
    {
        if (!nodeMap.TryGetValue(data, out Node node))
        {
            return false;
        }

        if (node.Previous != null)
        {
            node.Previous.Next = node.Next;
        }
        else
        {
            head = node.Next;
        }

        if (node.Next != null)
        {
            node.Next.Previous = node.Previous;
        }
        else
        {
            tail = node.Previous;
        }

        nodeMap.Remove(data);
        count--;

        return true;
    }
}