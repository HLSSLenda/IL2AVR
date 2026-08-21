using Mono.Cecil;
namespace IL2AVR;

public sealed class ClosedTypeSystem(TypeSystem typeSystem)
{
    public readonly ClosedType Boolean = new(typeSystem.Boolean, 1);
    public readonly ClosedType Byte = new(typeSystem.Byte, 1);
    public readonly ClosedType Char = new(typeSystem.Char, 1);
    public readonly ClosedType Double = new(typeSystem.Double, 4);
    public readonly ClosedType Int16 = new(typeSystem.Int16, 2);
    public readonly ClosedType Int32 = new(typeSystem.Int32, 4);
    public readonly ClosedType Int64 = new(typeSystem.Int64, 8);
    public readonly ClosedType IntPtr = new(typeSystem.IntPtr, 2);
    public readonly ClosedType Object = new(typeSystem.Object);
    public readonly ClosedType SByte = new(typeSystem.SByte, 1);
    public readonly ClosedType Single = new(typeSystem.Single, 2);
    public readonly ClosedType String = new(typeSystem.String);
    public readonly ClosedType TypedReference = new(typeSystem.TypedReference);
    public readonly ClosedType UInt16 = new(typeSystem.UInt16, 2);
    public readonly ClosedType UInt32 = new(typeSystem.UInt32, 4);
    public readonly ClosedType UInt64 = new(typeSystem.UInt64, 8);
    public readonly ClosedType UIntPtr = new(typeSystem.UIntPtr, 2);
    public readonly ClosedType Void = new(typeSystem.Void, 0);
}
