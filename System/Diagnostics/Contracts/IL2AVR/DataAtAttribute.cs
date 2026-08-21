namespace System.Diagnostics.Contracts.IL2AVR;

[AttributeUsage(AttributeTargets.Field | AttributeTargets.Parameter)]
#pragma warning disable CS9113
public sealed class DataAtAttribute(DataLocation location) : Attribute;
#pragma warning restore CS9113

