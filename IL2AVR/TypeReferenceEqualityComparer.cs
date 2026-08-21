using Mono.Cecil;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
namespace IL2AVR;

public readonly struct TypeReferenceEqualityComparer : IEqualityComparer<TypeReference>
{
    public bool Equals(TypeReference? x, TypeReference? y) => x?.FullName == y?.FullName;
    public int GetHashCode([DisallowNull] TypeReference obj) => obj.FullName.GetHashCode();
}