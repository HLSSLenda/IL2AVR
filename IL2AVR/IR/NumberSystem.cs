using System.Runtime.CompilerServices;
namespace IL2AVR.IR;

public sealed class NumberSystem
{
    private const int CACHE = 9;

    [InlineArray(CACHE)]
    private struct NumberArray
    {
        internal Number _0;
    }
    [InlineArray(CACHE)]
    private struct NumberArrayArray
    {
        internal Number[] _0;
    }
    public readonly ClosedType Type;
    private NumberArray _array;
    private NumberArrayArray _arrayArray;

    public NumberSystem(ClosedType type)
    {
        Type = type;
        for (nuint i = 0; i < CACHE; ++i)
            Unsafe.Add(ref _arrayArray._0, i) = [Unsafe.Add(ref _array._0, i) = new(type, i)];
    }

    public Number this[ulong index] => index < CACHE ? Unsafe.Add(ref _array._0, (int)index) : new(Type, index);

    public Number[] Array(ulong index) => index < CACHE ? Unsafe.Add(ref _arrayArray._0, (int)index) : [new(Type, index)];
}
