using System;

namespace MovementHomeAssignment.API.Cache;

/// <summary>
/// Linked list that tracks LRU order by cache key.
/// </summary>
public class CacheKeyLinkedList
{
    private Node head;
    private Node tail;
    private int count;

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
            head = newNode;
        }
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
            tail = newNode;
        }
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
            head = tail = null;
        }
        else
        {
            Node current = head;
            while (current.Next != tail) current = current.Next;
            current.Next = null;
            tail = current;
        }
        count--;
    }

    /// <summary>
    /// Removes a key if it exists.
    /// </summary>
    public bool Remove(int data)
    {
        if (head == null)
        {
            return false;
        }
        if (head.Data == data)
        {
            if (head == tail)
            {
                tail = null;
            }

            head = head.Next;
            count--;
            return true;
        }

        Node current = head;
        while (current.Next != null && current.Next.Data != data)
        {
            current = current.Next;
        }

        if (current.Next != null)
        {
            if (current.Next == tail) tail = current;
            current.Next = current.Next.Next;
            count--;
            return true;
        }
        return false;
    }
}