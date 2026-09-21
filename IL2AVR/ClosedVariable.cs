using System.Collections.Generic;
using Mono.Cecil;
using Mono.Cecil.Cil;
namespace IL2AVR;

public sealed class ClosedVariable(VariableReference variable, IReadOnlyDictionary<GenericParameter, IClosedType> generics)
{
    public readonly VariableReference UnderlyingVariable = variable;
    public readonly IClosedType VariableType = variable.VariableType.Close(generics);
}
