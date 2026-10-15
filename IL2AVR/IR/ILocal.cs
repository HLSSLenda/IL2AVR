namespace IL2AVR.IR;

public interface ILocal : IValue
{
    int Index { get; }
}
