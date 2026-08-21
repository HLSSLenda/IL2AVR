using System.Collections.Generic;
namespace IL2AVR;

public class ObjectPool<T> : IObjectPool<T> where T : new()
{
    private readonly List<T> _freeList = [];
    private int _freeIndex;

    public T Get() => _freeIndex < _freeList.Count ? _freeList[_freeIndex++] : new();
    public bool Return(T item)
    {
        if (_freeIndex < _freeList.Count)
            _freeList[_freeIndex] = item;
        else
            _freeList.Add(item);
        ++_freeIndex;
        return true;
    }
}
