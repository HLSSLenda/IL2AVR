using System;
namespace IL2AVR.IR;

// null is not cute
public sealed class Null : ICompileTimeValue
{
    public Null(IClosedType type)
    {
        if (!type.Kind.IsReferenceKind())
            throw new InvalidOperationException("Value types cannot be null!");
        Type = type;
    }

    public IClosedType Type { get; }
    public object? Value => null!;

    public bool TrySerialize(Span<byte> span)
    {
        if (span.Length < 2)
            return false;
        span[0] = span[1] = 0;  // << 0 because it isnt cute :D
        return true;
    }
}