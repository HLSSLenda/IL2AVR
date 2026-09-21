using Mono.Cecil;
namespace IL2AVR;

/// <summary>Represents any closed entity.</summary>
public interface IClosedEntity
{
    /// <summary>The underlying <see cref="MemberReference" /> of this entity.</summary>
    MemberReference MemberReference { get; }
    /// <summary>The name of this entity.</summary>
    string Name { get; }
}