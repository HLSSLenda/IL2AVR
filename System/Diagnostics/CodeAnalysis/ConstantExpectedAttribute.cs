namespace System.Diagnostics.CodeAnalysis;

[AttributeUsage(AttributeTargets.Parameter, AllowMultiple = false)]
public sealed class ConstantExpectedAttribute : Attribute
{
    public object? Min;
    public object? Max;
}