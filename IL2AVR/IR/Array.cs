using System;
using System.Collections;
using System.Collections.Generic;
namespace IL2AVR.IR;

public sealed class Array : ICompileTimeValue, IReadOnlyList<ICompileTimeValue>
{
    public Array(IClosedType type, int length)
    {
        if (type.Kind != ClosedTypeKind.Array)
            throw new InvalidOperationException("Array type cannot be constructed with a non-array type.");
        Type = type;
        _values = new ICompileTimeValue[length];
    }
    public Array(IClosedType type, IReadOnlyCollection<ICompileTimeValue> values) : this(type, values.Count)
    {
        int i = 0;
        foreach (ICompileTimeValue value in values)
            this[i++] = value;
    }
    private readonly ICompileTimeValue[] _values;
    public ICompileTimeValue this[int index]
    {
        get => _values[index];
        set
        {
            if (!value.Type.CanConvertTo(Type))
                throw new InvalidCastException("...");
            _values[index] = value;
        }
    }

    public int Count => _values.Length;
    public IEnumerator<ICompileTimeValue> GetEnumerator() =>
        ((IEnumerable<ICompileTimeValue>)_values).GetEnumerator();
    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    public IClosedType Type { get; }
    object? ICompileTimeValue.Value => this;

    public bool TrySerialize(Span<byte> span)
    {
        
    }
}