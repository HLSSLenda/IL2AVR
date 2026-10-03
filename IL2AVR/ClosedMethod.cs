using IL2AVR.IR;
using Mono.Cecil;
using Mono.Cecil.Cil;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
namespace IL2AVR;

public sealed class ClosedMethod : IClosedEntity, IEquatable<ClosedMethod>, IEquatable<MethodReference>
{
    private static readonly Dictionary<MethodReference, ClosedMethod> _methods = new(new MethodReferenceEqualityComparer());

    public readonly MethodDefinition UnderlyingMethod;
    public MemberReference MemberReference => UnderlyingMethod;

    private ClosedMethodBody _body;
    public ClosedMethodBody Body => _body ??= new();
    public MethodBody RawBody => UnderlyingMethod.Body;
    public readonly ClosedType DeclaringType;
    public readonly IReadOnlyDictionary<GenericParameter, IClosedType> ClassGenericArguments;
    public readonly IReadOnlyDictionary<GenericParameter, IClosedType> GenericArguments;
    public bool HasBody => UnderlyingMethod.HasBody;
    public bool IsConstructor => UnderlyingMethod.IsSpecialName && UnderlyingMethod.Name == ".ctor";
    public string Name => UnderlyingMethod.Name;
    public readonly IReadOnlyList<ClosedParameter> Parameters;
    public readonly IClosedType ReturnType;
    private readonly HashSet<ClosedType> _thrownExceptions;
    public IReadOnlyCollection<ClosedType> ThrownExceptions => _thrownExceptions;

    // TODO: Cache this the same way ClosedType gets cached
    internal ClosedMethod(MethodReference method, IReadOnlyDictionary<GenericParameter, IClosedType>? declaringTypeGenericArguments = null)
    {
        UnderlyingMethod = method.Resolve();
        Dictionary<GenericParameter, IClosedType> args = new((declaringTypeGenericArguments ??= EmptyDictionary<GenericParameter, IClosedType>.Instance).Count + UnderlyingMethod.GenericParameters.Count, new GenericParameterEqualityComparer());
        foreach (KeyValuePair<GenericParameter, IClosedType> kv in declaringTypeGenericArguments)
            args[kv.Key] = kv.Value;
        if (method is GenericInstanceMethod t)
            for (int i = 0; i < UnderlyingMethod.GenericParameters.Count; ++i)
                args[UnderlyingMethod.GenericParameters[i]] = (ClosedType)t.GenericArguments[i].Close(declaringTypeGenericArguments);
        GenericArguments = args;
        IEnumerable<ClosedParameter> parameters = method.Parameters.Select(param => new ClosedParameter(param, GenericArguments));
        Parameters = UnderlyingMethod.IsStatic ?
            [.. parameters] :
            [new(new ParameterDefinition(method.DeclaringType), EmptyDictionary<GenericParameter, IClosedType>.Instance), .. parameters];
        ReturnType = method.ReturnType.Close(declaringTypeGenericArguments);
    }

    public bool AddException(ClosedType type) => _thrownExceptions.Add(type);

    public static ClosedMethod Get(MethodReference method, IReadOnlyDictionary<GenericParameter, IClosedType>? declaringTypeGenericArguments = null)
    {
        ref ClosedMethod? closedMethod = ref CollectionsMarshal.GetValueRefOrAddDefault(_methods, method, out bool exists);
        if (!exists)
            closedMethod = new(method, declaringTypeGenericArguments);
        return closedMethod!;
    }

    public bool Equals(ClosedMethod? other) => other is not null &&
        UnderlyingMethod.MetadataToken == other.UnderlyingMethod.MetadataToken &&
        DeclaringType == other.DeclaringType &&
        GenericArguments.SequenceEqual(other.GenericArguments);

    public bool Equals(MethodReference? other) => other is not null &&
        UnderlyingMethod.MetadataToken == other.MetadataToken &&
        DeclaringType == other.DeclaringType; // TODO: melhorar isso

    public override bool Equals(object? obj) => obj switch
    {
        MethodReference other => Equals(other),
        ClosedMethod other => Equals(other),
        _ => false
    };

    public override int GetHashCode() => HashCode.Combine(DeclaringType, GenericArguments, Name);

    public override string ToString() => Name;
}
