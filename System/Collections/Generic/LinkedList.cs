namespace System.Collections.Generic;

public class LinkedList<T>
{
    public LinkedList(IEnumerable<T> enumerable)
    {

    }

    public int Count { get; private set; }
    public LinkedListNode<T>? First { get; private set; }
    public LinkedListNode<T>? Last { get; private set; }
}
