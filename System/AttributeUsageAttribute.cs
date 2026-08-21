namespace System;

[AttributeUsage(AttributeTargets.All)]
public sealed class AttributeUsageAttribute(AttributeTargets targets) : Attribute
{
    public readonly AttributeTargets Targets = targets;
    public bool Inherited;
    public bool AllowMultiple;
}