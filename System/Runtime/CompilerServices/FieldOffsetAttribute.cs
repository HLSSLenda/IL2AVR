namespace System.Runtime.CompilerServices;

[AttributeUsage(AttributeTargets.Field, AllowMultiple = false)]
public sealed class FieldOffsetAttribute(int offset) : Attribute
{
    public readonly int Offset = offset;
}