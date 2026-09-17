using Mono.Cecil;

namespace IL2AVR;

public sealed class ClosedArrayType(IClosedType type, int length = -1) : IClosedType
{
    public readonly bool IsConstantLength = length >= 0;

    public IClosedType ElementType => type;

    public TypeKind Kind => TypeKind.Array;

    public ushort MemorySize => (ushort)(IsConstantLength ? length : 2);

    public ushort Size => MemorySize;

    public TypeReference UnderlyingType { get; } = new ArrayType(type.UnderlyingType);

    public override string ToString() => $"{ElementType}[]";
}
