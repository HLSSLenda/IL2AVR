namespace System.Numerics;

public interface IMinMaxValue<T> where T : IMinMaxValue<T>
{
    static abstract T MinValue { get; }
    static abstract T MaxValue { get; }
}