using Mono.Cecil;

namespace IL2AVR;

public sealed class ClosedArrayType(IClosedType type) : IClosedType
{
    public IClosedType ElementType => type;

    public TypeKind Kind => TypeKind.Array;

    public ushort Size => 2;

    public TypeReference UnderlyingType { get; } = new ArrayType(type.UnderlyingType);
}
