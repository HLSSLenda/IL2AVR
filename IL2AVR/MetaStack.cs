using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
namespace IL2AVR;

public sealed class MetaStack<T>(IList<T>? backingList = null) : IList<T>, IReadOnlyList<T>
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

    public T Peek() => _backingList[_stackIndex - 1];
    public T Pop()
    {
        T item = Peek();
        RemoveAt(Count - 1);
        return item;
    }
    public void Pop(Span<T> span)
    {
        if (!TryPop(span))
            throw new InvalidOperationException($"Cannot pop {span.Length} elements.");
    }
    public bool TryPop(Span<T> span)
    {
        if (span.Length == 0)
            return true;
        if (span.Length > _stackIndex)
            return false;
        if (_backingList is List<T> l)
            CollectionsMarshal.AsSpan(l)[(^ - span.Length) ..].CopyTo(span);
        else
            for (int i = 0; i < span.Length; ++i)
                span[i] = _backingList[^ - (span.Length - i - 1)];
        _stackIndex -= span.Length;
        return true;
    }

    public bool Remove(T item)
    {
        int index = IndexOf(item);
        if (index == -1)
            return false;
        RemoveAt(index);
        return true;
    }
    public void RemoveAt(int index)
    {
        if (index <= _stackIndex)
            --_stackIndex;
        _backingList.RemoveAt(index);
    }

    public bool TryPeek(out T? value)
    {
        if (_stackIndex > 0)
        {
            value = _backingList[_stackIndex - 1];
            return true;
        }
        value = default;
        return false;
    }

    IEnumerator IEnumerable.GetEnumerator() => _backingList.GetEnumerator();
}
