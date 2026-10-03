using System;
using System.Runtime.InteropServices;

namespace IL2AVR.IR;

public record Number(IClosedType Type, ulong Value) : ICompileTimeValue
{
    object? ICompileTimeValue.Value => Value;

    public Number To(ClosedTypeSystem typeSystem, IClosedType numberType)
    {
        if (numberType.Equals(typeSystem.SByte))
            return new(numberType, (ulong)(sbyte)Value);
        if (numberType.Equals(typeSystem.Byte) || numberType.Equals(typeSystem.Char))
            return new(numberType, (ulong)Value);
        if (numberType.Equals(typeSystem.Int16))
            return new(numberType, (ulong)(short)Value);
        if (numberType.Equals(typeSystem.UInt16))
            return new(numberType, (ulong)(ushort)Value);
        if (numberType.Equals(typeSystem.Int32))
            return new(numberType, (ulong)(int)Value);
        if (numberType.Equals(typeSystem.UInt32))
            return new(numberType, (ulong)(uint)Value);
        if (numberType.Equals(typeSystem.Int64))
            return new(numberType, (ulong)(long)Value);
        if (numberType.Equals(typeSystem.UInt64))
            return new(numberType, Value);
        throw new WtfException();
    }
    public override string ToString() => Value.ToString();

    public bool TrySerialize(Span<byte> span)
    {
        if (span.Length < Type.Size)
            return false;
        switch (Type.Size)
        {
            case 1:
                MemoryMarshal.Write(span, (byte)Value);
                break;
            case 2:
                MemoryMarshal.Write(span, (ushort)Value);
                break;
            case 4:
                MemoryMarshal.Write(span, (uint)Value);
                break;
            case 8:
                MemoryMarshal.Write(span, (ulong)Value);
                break;
        }
        return true;
    }
}
