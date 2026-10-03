using Mono.Cecil;

namespace IL2AVR;

public sealed class ClosedArrayType(IClosedType type, int length = -1) : IClosedType
{
    public readonly bool IsConstantLength = length >= 0;

    public IClosedType ElementType => type;

    public ClosedTypeKind Kind => ClosedTypeKind.Array;

    public MemberReference MemberReference => UnderlyingType;

    public ushort MemorySize => (ushort)(IsConstantLength ? length : 2);

    public string Name => $"{ElementType}[]";

    public ushort Size => MemorySize;

    public TypeReference UnderlyingType { get; } = new ArrayType(type.UnderlyingType);

    public override string ToString() => Name;
}
