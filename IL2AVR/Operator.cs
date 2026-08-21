using Mono.Cecil.Cil;
namespace IL2AVR;

public static class Operator
{
    internal static ClosedType? GetTypeFromBinaryOp(ClosedTypeSystem typeSystem, Code code, IValue lhs, IValue rhs)
    {
        if (lhs.Type is null || lhs.Type == typeSystem.Object || lhs.Type == typeSystem.String || lhs.Type == typeSystem.Void ||
                rhs.Type is null || rhs.Type == typeSystem.Object || rhs.Type == typeSystem.String || rhs.Type == typeSystem.Void)
            return null;
        if (lhs.Type == rhs.Type)
            return lhs.Type;
        if (lhs.Type == typeSystem.IntPtr || lhs.Type == typeSystem.UIntPtr || rhs.Type == typeSystem.IntPtr || rhs.Type == typeSystem.UIntPtr)
            return typeSystem.UIntPtr;
    }
    public static IValue Add(ClosedTypeSystem typeSystem, IValue v1, IValue v2) => (v1, v2) switch
    {
        (Number n1, Number n2) => new Number(GetTypeFromBinaryOp(typeSystem, Code), n1.Value + n2.Value),
        (Number { Value: 0 }, IValue v) => v,
        (IValue v, Number { Value: 0 }) => v,
        /*(Number { Value: ulong x }, IValue v) when ((long)x < 0) => Subtract(new Number(v1.Type, (ulong)-(long)x), v),
        (IValue v, Number { Value: ulong y }) when ((long)y < 0) => Subtract(v, new Number(v2.Type, (ulong)-(long)y)),*/
        _ => new Add(...)
    };
    public static IValue Subtract(ClosedTypeSystem typeSystem, IValue v1, IValue v2) => (v1, v2) switch
    {
        (Number n1, Number n2) => new Number(..., n1.Value - n2.Value),
        (Number { Value: 0 }, IValue v) => v,
        (IValue v, Number { Value: 0 }) => v,
        _ => new Add(...)
    };
}