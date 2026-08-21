using System.Collections;
using System.Collections.Generic;
namespace IL2AVR;

public sealed class MetaStack<T>(IList<T>? backingList = null) : IList<T>
{
    private readonly IList<T> _backingList = backingList ?? [];
    private int _stackIndex = 0;

    public T this[int index]
    {
        get => _backingList[index];
        set => _backingList[index] = value;
    }

    public int Count => _backingList.Count;
    public bool IsReadOnly => _backingList.IsReadOnly;

    public void Add(T item)
    {
        if (_stackIndex < _backingList.Count)
            _backingList[_stackIndex] = item;
        else
            _backingList.Add(item);
        ++_stackIndex;
    }

    public void Clear()
    {
        _backingList.Clear();
        _stackIndex = 0;
    }

    public bool Contains(T item) => _backingList.Contains(item);
    public void CopyTo(T[] array, int arrayIndex) => _backingList.CopyTo(array, arrayIndex);
    public IEnumerator<T> GetEnumerator() => _backingList.GetEnumerator();
    public int IndexOf(T item) => _backingList.IndexOf(item);

    public void Insert(int index, T item)
    {
        _backingList.Insert(index, item);
        if (index < _stackIndex)
            ++_stackIndex;
    }

    public bool Remove(T item) => _backingList.Remove(item);
    public void RemoveAt(int index) => _backingList.RemoveAt(index);
    IEnumerator IEnumerable.GetEnumerator() => _backingList.GetEnumerator();
}
