using System;
namespace IL2AVR.IR;

public sealed class CompileTimeLocal : ICompileTimeValue
{
    public ICompileTimeValue Value { get; internal set; }
    internal CompileTimeLocal(ICompileTimeValue value, int offset)
    {
        Value = value;
        Offset = offset;
    }
    public IClosedType Type => Value.Type;
    object? ICompileTimeValue.Value => Value;
    public bool TrySerialize(Span<byte> span) => Value is not null && Value.TrySerialize(span);
}
