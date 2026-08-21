namespace System.Diagnostics;

[AttributeUsage(AttributeTargets.Method | AttributeTargets.Class, AllowMultiple = true)]
public sealed class ConditionalAttribute(string conditionString) : Attribute
{
    public string ConditionString { get; } = conditionString;
}