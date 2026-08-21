using Mono.Cecil;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
namespace IL2AVR;

public sealed class MethodReferenceEqualityComparer : IEqualityComparer<MethodReference>
{
    public bool Equals(MethodReference? x, MethodReference? y) => x?.FullName == y?.FullName;

    public int GetHashCode([DisallowNull] MethodReference obj) => obj.FullName.GetHashCode();
}
