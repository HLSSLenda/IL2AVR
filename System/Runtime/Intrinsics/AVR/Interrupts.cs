using System.Runtime.CompilerServices;
using System.Runtime.CompilerServices.IL2AVR;
namespace System.Runtime.Intrinsics.AVR;

public static class Interrupts
{
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void Disable() => Assembly.Asm(OpCodeId.Cli);
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void Enable() => Assembly.Asm(OpCodeId.Sei);
}