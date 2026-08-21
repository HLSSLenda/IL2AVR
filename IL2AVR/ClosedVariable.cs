using System.Collections.Generic;
using Mono.Cecil;
using Mono.Cecil.Cil;
namespace IL2AVR;

public sealed class ClosedVariable(VariableReference variable, IReadOnlyDictionary<GenericParameter, ClosedType> generics)
{
    public readonly VariableReference UnderlyingVariable = variable;
    public readonly ClosedType VariableType = variable.VariableType.InstantiateGeneric(generics);
}
