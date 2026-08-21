namespace System.Runtime.CompilerServices;

[AttributeUsage(AttributeTargets.Class | AttributeTargets.Struct, AllowMultiple = false)]
public sealed class StructLayoutAttribute(LayoutKind value) : Attribute
{
    public int Pack;
    public readonly LayoutKind Value = value;
}