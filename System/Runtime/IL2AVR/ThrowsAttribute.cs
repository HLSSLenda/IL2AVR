namespace System.Runtime.IL2AVR;

[AttributeUsage(AttributeTargets.Constructor | AttributeTargets.Delegate |
                AttributeTargets.Method | AttributeTargets.Parameter, AllowMultiple = true)]
#pragma warning disable CS9113  // unread parameter; used by IL2AVR
public sealed class ThrowsAttribute(params Type[] types) : Attribute;
#pragma warning restore CS9113