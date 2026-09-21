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
    object? ICompileTimeValue.Value => null!;
}