namespace IL2AVR.IR;

public interface ICompileTimeValue : IValue
{
    object? Value { get; }
}