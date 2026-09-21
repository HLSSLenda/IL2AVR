namespace System.Runtime.CompilerServices;

[AttributeUsage(AttributeTargets.Method, AllowMultiple = false)]
public sealed class UnsafeAccessorAttribute(UnsafeAccessorKind kind) : Attribute
{
    public readonly UnsafeAccessorKind Kind = kind;
    public string? Name;
}