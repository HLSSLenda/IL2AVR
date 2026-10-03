using Mono.Cecil;

namespace IL2AVR;

public sealed class ClosedPointerType(IClosedType type) : IClosedType
{
    public IClosedType ElementType => type;

    public ClosedTypeKind Kind => ClosedTypeKind.Pointer;

    public MemberReference MemberReference => UnderlyingType;

    public string Name => $"{ElementType}*";

    public ushort MemorySize => 2;

    public ushort Size => 2;

    public TypeReference UnderlyingType { get; } = new PointerType(type.UnderlyingType);

    public override string ToString() => Name;
}
