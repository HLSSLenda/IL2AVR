namespace System.Runtime;

[AttributeUsage(AttributeTargets.Method)]
#pragma warning disable CS9113  // unread parameter; used by IL2AVR
public sealed class InterruptAttribute(byte isr) : Attribute;
#pragma warning restore CS9113