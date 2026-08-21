using System.Runtime.CompilerServices;
namespace System;

// Type info is, by default, a compile-time construct.
// Unless type info is preserved, Is* will always be false **in runtime**.
[Intrinsic]
public sealed class Type
{
    [Intrinsic]
    public static extern Type GetTypeFromHandle(RuntimeTypeHandle handle);

    public extern bool IsClass
    {
        [Intrinsic]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get;
    }
    public extern bool IsInterface
    {
        [Intrinsic]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get;
    }
    public extern bool IsValueType
    {
        [Intrinsic]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get;
    }
    public extern string? Name
    {
        [Intrinsic]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get;
    }
}
