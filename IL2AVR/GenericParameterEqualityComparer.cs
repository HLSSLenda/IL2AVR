using Mono.Cecil;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
namespace IL2AVR;

public sealed class GenericParameterEqualityComparer : IEqualityComparer<GenericParameter>
{
    public bool Equals(GenericParameter? x, GenericParameter? y)
    {
        throw new System.NotImplementedException();
    }

    public int GetHashCode([DisallowNull] GenericParameter obj)
    {
        throw new System.NotImplementedException();
    }
}
