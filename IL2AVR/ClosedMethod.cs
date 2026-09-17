using Mono.Cecil;
using Mono.Cecil.Cil;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
namespace IL2AVR;

public sealed class ClosedMethod : IEquatable<ClosedMethod>, IEquatable<MethodReference>
{
    private static readonly Dictionary<MethodReference, ClosedMethod> _methods = new(new MethodReferenceEqualityComparer());

    public readonly MethodDefinition UnderlyingMethod;

    private ClosedMethodBody _body;
    public ClosedMethodBody Body => _body ??= new();
    public MethodBody RawBody => UnderlyingMethod.Body;
    public readonly ClosedType DeclaringType;
    public readonly IReadOnlyDictionary<GenericParameter, ClosedType> ClassGenericArguments;
    public readonly IReadOnlyDictionary<GenericParameter, ClosedType> GenericArguments;
    public bool HasBody => UnderlyingMethod.HasBody;
    public readonly IReadOnlyList<ClosedParameter> Parameters;
    public readonly IClosedType ReturnType;
    private readonly HashSet<ClosedType> _thrownExceptions;
    public IReadOnlyCollection<ClosedType> ThrownExceptions => _thrownExceptions;

    // TODO: Cache this the same way ClosedType gets cached
    internal ClosedMethod(MethodReference method, IReadOnlyDictionary<GenericParameter, ClosedType> declaringTypeGenericArguments)
    {
        UnderlyingMethod = method.Resolve();
        Dictionary<GenericParameter, ClosedType> args = new(declaringTypeGenericArguments.Count + UnderlyingMethod.GenericParameters.Count, new GenericParameterEqualityComparer());
        foreach (KeyValuePair<GenericParameter, ClosedType> kv in declaringTypeGenericArguments)
            args[kv.Key] = kv.Value;
        if (method is GenericInstanceMethod t)
            for (int i = 0; i < UnderlyingMethod.GenericParameters.Count; ++i)
                args[UnderlyingMethod.GenericParameters[i]] = (ClosedType)t.GenericArguments[i].Close(declaringTypeGenericArguments);
        GenericArguments = args;
        IEnumerable<ClosedParameter> parameters = method.Parameters.Select(param => new ClosedParameter(param, GenericArguments));
        Parameters = UnderlyingMethod.IsStatic ?
            [.. parameters] :
            [new(new ParameterDefinition(method.DeclaringType), new Dictionary<GenericParameter, ClosedType>()), .. parameters];
        ReturnType = method.ReturnType.Close(declaringTypeGenericArguments);
    }

    public bool AddException(ClosedType type) => _thrownExceptions.Add(type);

    public static ClosedMethod Get(MethodReference method, IReadOnlyDictionary<GenericParameter, ClosedType> declaringTypeGenericArguments)
    {
        ref ClosedMethod? closedMethod = ref CollectionsMarshal.GetValueRefOrAddDefault(_methods, method, out bool exists);
        if (!exists)
            closedMethod = new(method, declaringTypeGenericArguments);
        return closedMethod!;
    }

    public bool Equals(ClosedMethod? other)
    {
        throw new NotImplementedException();
    }

    public bool Equals(MethodReference? other)
    {
        throw new NotImplementedException();
    }

    public override bool Equals(object? obj)
    {
        return base.Equals(obj);
    }

    public override int GetHashCode()
    {
        return base.GetHashCode();
    }

    public override string ToString()
    {
        return base.ToString();
    }
}
