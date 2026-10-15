using Mono.Cecil;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
namespace IL2AVR;

public sealed class GenericParameterEqualityComparer : IEqualityComparer<GenericParameter>
{
    public bool Equals(GenericParameter? x, GenericParameter? y) => x is null ? y is null : x.MetadataToken == y?.MetadataToken;
    public int GetHashCode([DisallowNull] GenericParameter obj) => obj.MetadataToken.GetHashCode();
}
