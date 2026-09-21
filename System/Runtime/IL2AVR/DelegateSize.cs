namespace System.Runtime.IL2AVR;

/// <summary>Changes the delegate default method list size.</summary>
/// <param name="size">The size of the delegate. The IL2AVR compiler will emit an error if it's less than 1.</param>
/// <remarks>If the attribute is not specified, the default size is 1.</remarks>
[AttributeUsage(AttributeTargets.Delegate)]
#pragma warning disable CS9113
public sealed class DelegateSize(int size) : Attribute;
#pragma warning restore CS9113