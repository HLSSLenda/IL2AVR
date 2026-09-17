using Mono.Cecil;

namespace IL2AVR;

public sealed class ClosedSentinelType(IClosedType type) : IClosedType
{
    public IClosedType ElementType => type;

    public TypeKind Kind => TypeKind.Sentinel;

    public ushort MemorySize => 0;

    public ushort Size => 0;

    public TypeReference UnderlyingType => new SentinelType(type.UnderlyingType);

    public override string ToString() => $"<sentinel>({UnderlyingType})";
}