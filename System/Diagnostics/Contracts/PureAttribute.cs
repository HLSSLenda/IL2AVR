namespace System.Diagnostics.Contracts;

[AttributeUsage(AttributeTargets.Class | AttributeTargets.Constructor | AttributeTargets.Delegate | AttributeTargets.Event | AttributeTargets.Method | AttributeTargets.Parameter | AttributeTargets.Property, AllowMultiple = false, Inherited = true)]
public sealed class PureAttribute : Attribute;