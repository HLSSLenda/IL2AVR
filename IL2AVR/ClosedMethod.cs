using Mono.Cecil;
using Mono.Cecil.Cil;
using System;
using System.Collections.Generic;
using System.Linq;
namespace IL2AVR;

public sealed class ClosedMethod : IEquatable<ClosedMethod>, IEquatable<MethodReference>
{
    private static readonly Dictionary<MethodReference, ClosedMethod> _methods = new(new MethodReferenceEqualityComparer());

    public readonly MethodDefinition UnderlyingMethod;

    private ClosedMethodBody _body;
    public ClosedMethodBody Body => _body ??= new();
    public MethodBody RawBody => UnderlyingMethod.Body;
    public ClosedType DeclaringType => (ClosedType)UnderlyingMethod.DeclaringType.Close(ClassGenericArguments);
    public readonly IReadOnlyDictionary<GenericParameter, ClosedType> ClassGenericArguments;
    public readonly IReadOnlyDictionary<GenericParameter, ClosedType> GenericArguments;
    public bool HasBody => UnderlyingMethod.HasBody;
    public readonly IReadOnlyList<ClosedParameter> Parameters;
    public readonly IClosedType ReturnType;
    private readonly HashSet<ClosedType> _thrownExceptions;
    public IReadOnlyCollection<ClosedType> ThrownExceptions => _thrownExceptions;

    // TODO: Cache this the same way ClosedType gets cached
    internal ClosedMethod(MethodReference method)
    {
        UnderlyingMethod = method.Resolve();
        Dictionary<GenericParameter, ClosedType> args = new Dictionary<GenericParameter, ClosedType>(declaringTypeGenericArguments.Count + UnderlyingMethod.GenericParameters.Count, new GenericParameterEqualityComparer());
        foreach (KeyValuePair<GenericParameter, ClosedType> kv in declaringTypeGenericArguments)
            args[kv.Key] = kv.Value;
        if (method is GenericInstanceMethod t)
            for (int i = 0; i < UnderlyingMethod.GenericParameters.Count; ++i)
                args[UnderlyingMethod.GenericParameters[i]] = t.GenericArguments[i].InstantiateGeneric(declaringTypeGenericArguments);
        GenericArguments = args;
        IEnumerable<ClosedParameter> parameters = method.Parameters.Select(param => new ClosedParameter(param, GenericArguments));
        Parameters = UnderlyingMethod.IsStatic ?
            [.. parameters] :
            [new(new ParameterDefinition(method.DeclaringType), new Dictionary<GenericParameter, ClosedType>()), .. parameters];
        ReturnType = method.ReturnType.InstantiateGeneric(GenericArguments);
    }

    public bool AddException(ClosedType type) => _thrownExceptions.Add(type);

    public static ClosedMethod Get(MethodReference method, IReadOnlyDictionary<GenericParameter, ClosedType>? declaringTypeGenericArguments = null)
    {

    }
}
