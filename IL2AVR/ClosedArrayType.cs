using Mono.Cecil;
namespace IL2AVR;

/// <summary>Represents a closed array type, with optional length.</summary>
/// <param name="type">The type of the array.</param>
/// <param name="length">The compile-time length of the array. If it's less than zero, then it's a runtime array.</param>
/// <param name="location">The data location of the array and its contents.</param>
public sealed class ClosedArrayType(IClosedType type, int length = -1, DataLocation location = DataLocation.Unknown) : IClosedType
{
    /// <summary><see langword="true" /> if it's length is available in compile-time.</summary>
    /// <remarks>
    /// If it's <see langword="true" />, then <see cref="MemorySize" /> and <see cref="Size" /> works as if the array members are
    /// unrolled at compile-time.
    /// </remarks>
    public readonly bool IsConstantLength = length >= 0;
    /// <inheritdoc />
    public IClosedType ElementType => type;
    /// <inheritdoc />
    public ClosedTypeKind Kind => ClosedTypeKind.Array;
    /// <inheritdoc />
    public DataLocation Location => location;
    /// <inheritdoc />
    public MemberReference MemberReference => UnderlyingType;
    /// <inheritdoc />
    public ushort MemorySize => (ushort)(IsConstantLength ? type.MemorySize * length : 2);
    /// <inheritdoc />
    public string Name => $"{ElementType}[]";
    /// <inheritdoc />
    public ushort Size => (ushort)(IsConstantLength ? type.Size * length : 2);
    /// <inheritdoc />
    public TypeReference UnderlyingType { get; } = new ArrayType(type.UnderlyingType);
    /// <inheritdoc />
    public override string ToString() => Name;
}
