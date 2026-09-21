using Mono.Cecil;
namespace IL2AVR;

public sealed class ClosedTypeSystem(TypeSystem typeSystem)
{
    public readonly ClosedType Boolean = new(typeSystem.Boolean, 1, ClosedTypeKind.Boolean);
    public readonly ClosedType Byte = new(typeSystem.Byte, 1, ClosedTypeKind.Byte);
    public readonly ClosedType Char = new(typeSystem.Char, 1, ClosedTypeKind.Char);
    public readonly ClosedType Double = new(typeSystem.Double, 4, ClosedTypeKind.Double);
    public readonly ClosedType Int16 = new(typeSystem.Int16, 2, ClosedTypeKind.Int16);
    public readonly ClosedType Int32 = new(typeSystem.Int32, 4, ClosedTypeKind.Int32);
    public readonly ClosedType Int64 = new(typeSystem.Int64, 8, ClosedTypeKind.Int64);
    public readonly ClosedType IntPtr = new(typeSystem.IntPtr, 2, ClosedTypeKind.IntPtr);
    public readonly ClosedType Object = new(typeSystem.Object, 0, ClosedTypeKind.Object);
    public readonly ClosedType SByte = new(typeSystem.SByte, 1, ClosedTypeKind.SByte);
    public readonly ClosedType Single = new(typeSystem.Single, 2, ClosedTypeKind.Single);
    public readonly ClosedType String = new(typeSystem.String, 0, ClosedTypeKind.String);
    public readonly ClosedType TypedReference = new(typeSystem.TypedReference, 0, ClosedTypeKind.TypedReference);
    public readonly ClosedType UInt16 = new(typeSystem.UInt16, 2, ClosedTypeKind.UInt16);
    public readonly ClosedType UInt32 = new(typeSystem.UInt32, 4, ClosedTypeKind.UInt32);
    public readonly ClosedType UInt64 = new(typeSystem.UInt64, 8, ClosedTypeKind.UInt64);
    public readonly ClosedType UIntPtr = new(typeSystem.UIntPtr, 2, ClosedTypeKind.UIntPtr);
    public readonly ClosedType Void = new(typeSystem.Void, 0);
}
