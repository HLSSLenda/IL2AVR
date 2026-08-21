using Mono.Cecil;
namespace IL2AVR;

public interface IClosedType
{
    IClosedType ElementType { get; }
    TypeKind Kind { get; }
        ushort Size { get; }
    TypeReference UnderlyingType { get; }
}
