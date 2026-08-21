namespace IL2AVR;

public interface IBinaryOperator : IOperator
{
    IValue Left { get; }
    IValue Right { get; }
}