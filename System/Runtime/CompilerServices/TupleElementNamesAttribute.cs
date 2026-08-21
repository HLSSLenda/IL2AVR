namespace System.Runtime.CompilerServices;

[AttributeUsage(AttributeTargets.Class | AttributeTargets.Field | AttributeTargets.ReturnValue |
                AttributeTargets.Parameter | AttributeTargets.Struct)]
public sealed class TupleElementNamesAttribute(string?[] names) : Attribute
{
    public readonly string?[] Names = names;
}