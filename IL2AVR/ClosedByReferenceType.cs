using Mono.Cecil;

namespace IL2AVR;

public sealed class ClosedByReferenceType(IClosedType type) : IClosedType
{
    public IClosedType ElementType => type;

    public ClosedTypeKind Kind => ClosedTypeKind.ByReference;

    public ushort MemorySize => 2;

    public ushort Size => 2;

    public TypeReference UnderlyingType { get; } = new ArrayType(type.UnderlyingType);

    public override string ToString() => $"(ref {ElementType})";
}
