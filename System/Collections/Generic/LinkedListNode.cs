namespace System.Collections.Generic;

public sealed class LinkedListNode<T>(T value)
{
    public LinkedList<T>? List { get; internal set; }
    public LinkedListNode<T> Next { get; internal set; }
    public LinkedListNode<T> Previous { get; internal set; }
    public readonly T Value = value;
}