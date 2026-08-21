namespace System.Runtime;

[AttributeUsage(AttributeTargets.Constructor | AttributeTargets.Delegate |
                AttributeTargets.Method | AttributeTargets.Parameter, AllowMultiple = true)]
public sealed class ThrowsAttribute(params Type[] types) : Attribute;