using System;

namespace MovementHomeAssignment.API.Cache;

public class CacheKeyLinkedList
{
    private Node head;
    private Node tail;
    private int count;

    public int Count => count;

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

    public int Last => tail != null ? tail.Data : throw new InvalidOperationException("Empty");

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