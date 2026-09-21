using System;
using System.Collections.Generic;
namespace IL2AVR.IR;

public sealed class Array : ICompileTimeValue
{
    public sealed class BackingStorage(IClosedType type, int length)
    {
        public BackingStorage(IClosedType type, IReadOnlyCollection<ICompileTimeValue> values) : this(type, values.Count)
        {
            int i = 0;
            foreach (ICompileTimeValue value in values)
                this[i++] = value;
        }
        private readonly ICompileTimeValue[] values = new ICompileTimeValue[length];
        public ICompileTimeValue this[int index]
        {
            get => values[index];
            set
            {
                if (!value.Type.CanConvertTo(type))
                    throw new InvalidCastException("...");
                values[index] = value;
            }
        }
    }
    public Array(IClosedType type, params IReadOnlyList<ICompileTimeValue> values)
    {
        if (type.Kind != ClosedTypeKind.Array)
            throw new InvalidOperationException("Array type cannot be constructed with a non-array type.");
        Type = type;
        Values = new(type, values);
    }

    public IClosedType Type { get; }
    public readonly BackingStorage Values;
    object? ICompileTimeValue.Value => Values;
}