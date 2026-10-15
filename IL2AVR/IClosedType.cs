using Mono.Cecil;
namespace IL2AVR;

/// <summary>Represents any closed type.</summary>
/// <remarks>See the project documentation to see why it exists.</remarks>
public interface IClosedType : IClosedEntity
{
    /// <summary>The element type (or inner type) that this type stores.</summary>
    /// <remarks>Functionality similar to <see cref="TypeReference" /> that contains a <c>ElementType</c> member.</remarks>
    IClosedType ElementType { get; }
    /// <summary>The type kind.</summary>
    ClosedTypeKind Kind { get; }
    /// <summary>The type storage location.</summary>
    DataLocation Location { get; }
    /// <summary>The total/real memory size that this type holds.</summary>
    ushort MemorySize { get; }
    /// <summary>The compile-time size that this type holds.</summary>
    ushort Size { get; }
    /// <summary>The underlying <see cref="TypeReference" /> that this type wraps.</summary>
    TypeReference UnderlyingType { get; }
}
