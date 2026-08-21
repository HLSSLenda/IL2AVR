using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
namespace IL2AVR;

public sealed class EmptyDictionary<TKey, TValue> : IReadOnlyDictionary<TKey, TValue> where TKey : notnull
{
    public static EmptyDictionary<TKey, TValue> Instance = new();
    private EmptyDictionary()
    {
    }
    public TValue this[TKey key] => throw new KeyNotFoundException();
    public IEnumerable<TKey> Keys => Enumerable.Empty<TKey>();
    public IEnumerable<TValue> Values => Enumerable.Empty<TValue>();
    public int Count => 0;
    public bool ContainsKey(TKey key) => false;
    public IEnumerator<KeyValuePair<TKey, TValue>> GetEnumerator() => Enumerable.Empty<KeyValuePair<TKey, TValue>>().GetEnumerator();
    public bool TryGetValue(TKey key, [MaybeNullWhen(false)] out TValue value)
    {
        value = default;
        return false;
    }
    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
}

