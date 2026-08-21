namespace System.Runtime.CompilerServices.IL2AVR;

[AttributeUsage(AttributeTargets.Field | AttributeTargets.Parameter | AttributeTargets.Property, AllowMultiple = false)]
public sealed class TypeAsAttribute(Type type) : Attribute;