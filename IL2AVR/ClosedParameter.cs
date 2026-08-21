using Mono.Cecil;
using System.Collections.Generic;
namespace IL2AVR;

public sealed class ClosedParameter(ParameterReference parameter, IReadOnlyDictionary<GenericParameter, ClosedType> declaringObjectsGenericArguments)
{
    public readonly ParameterDefinition UnderlyingParameter = parameter.Resolve();
    public readonly ClosedType ParameterType = parameter.ParameterType.Close(declaringObjectsGenericArguments);
}
