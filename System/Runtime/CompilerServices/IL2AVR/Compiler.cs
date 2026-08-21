using System.Diagnostics.CodeAnalysis;
namespace System.Runtime.CompilerServices.IL2AVR;

[Intrinsic]
public static class Compiler
{
    public static extern string Callee
    {
        [Intrinsic]
        [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.InternalCall)]
        get;
    }
    public static extern string Caller
    {
        [Intrinsic]
        [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.InternalCall)]
        get;
    }

    [Intrinsic]
    [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.InternalCall)]
    public static extern void Write([ConstantExpected] string message);

    [Intrinsic]
    [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.InternalCall)]
    public static void WriteLine([ConstantExpected] string message)
    {
        Write(message);
        Write("\n");
    }

    public static bool Likely(bool value) => value;
    public static bool Unlikely(bool value) => value;
}