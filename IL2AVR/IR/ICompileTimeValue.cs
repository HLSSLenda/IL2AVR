using System;
namespace IL2AVR.IR;

public interface ICompileTimeValue : IValue
{
    object? Value { get; }
    bool TrySerialize(Span<byte> span);
}