namespace IL2AVR.IR;

public record Number(ClosedType Type, ulong Value) : IValue
{
    public Number To(ClosedTypeSystem typeSystem, ClosedType numberType)
    {
        if (numberType == typeSystem.SByte)
            return new(numberType, (ulong)(sbyte)Value);
        if (numberType == typeSystem.Byte || numberType == typeSystem.Char)
            return new(numberType, (ulong)Value);
        if (numberType == typeSystem.Int16)
            return new(numberType, (ulong)(short)Value);
        if (numberType == typeSystem.UInt16)
            return new(numberType, (ulong)(ushort)Value);
        if (numberType == typeSystem.Int32)
            return new(numberType, (ulong)(int)Value);
        if (numberType == typeSystem.UInt32)
            return new(numberType, (ulong)(uint)Value);
        if (numberType == typeSystem.Int64)
            return new(numberType, (ulong)(long)Value);
        if (numberType == typeSystem.UInt64)
            return new(numberType, Value);
        throw new WtfException();
    }
    public override string ToString() => Value.ToString();
}
