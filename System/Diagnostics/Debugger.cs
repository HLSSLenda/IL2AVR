using System.Runtime.CompilerServices;
using System.Runtime.CompilerServices.IL2AVR;
namespace System.Diagnostics;

public static class Debugger
{
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void Break() => Assembly.Asm(OpCodeId.ExtBreak);
}