namespace System.Runtime.CompilerServices;

[AttributeUsage(AttributeTargets.Method | AttributeTargets.Constructor, AllowMultiple = false, Inherited = false)]
public sealed class MethodImplAttribute(MethodImplOptions value) : Attribute
{
    public MethodCodeType MethodCodeType;
    public readonly MethodImplOptions Value = value;
}
