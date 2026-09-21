namespace System.Runtime.IL2AVR;

/// <summary>Marks that this assembly or module uses exception handling.</summary>
[AttributeUsage(AttributeTargets.Assembly | AttributeTargets.Module)]
public sealed class CanThrowAttribute : Attribute;