namespace System.Diagnostics.CodeAnalysis;

[AttributeUsage(AttributeTargets.Method, AllowMultiple = false)]
public sealed class DoesNotReturnAttribute : Attribute;