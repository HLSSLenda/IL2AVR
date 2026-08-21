using System.Diagnostics.CodeAnalysis;
namespace System.Runtime.CompilerServices.IL2AVR;

public static class Assembly
{
    [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.InternalCall)]
    public static extern void Asm([ConstantExpected] OpCodeId opcode);
    [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.InternalCall)]
    public static extern void Asm([ConstantExpected] OpCodeId opcode, byte arg0);
    [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.InternalCall)]
    public static extern void Asm([ConstantExpected] OpCodeId opcode, ushort arg0);
    [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.InternalCall)]
    public static extern void Asm([ConstantExpected] OpCodeId opcode, out byte arg0);
    [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.InternalCall)]
    public static extern void Asm([ConstantExpected] OpCodeId opcode, byte arg0, byte arg1);
    [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.InternalCall)]
    public static extern void Asm([ConstantExpected] OpCodeId opcode, byte arg0, ushort arg1);
    [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.InternalCall)]
    public static extern void Asm([ConstantExpected] OpCodeId opcode, ushort arg0, byte arg1);
    [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.InternalCall)]
    public static extern void Asm([ConstantExpected] OpCodeId opcode, out byte arg0, byte arg1);
    [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.InternalCall)]
    public static extern void Asm([ConstantExpected] OpCodeId opcode, out byte arg0, ushort arg1);
    [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.InternalCall)]
    public static extern void Asm([ConstantExpected] OpCodeId opcode, out ushort arg0, byte arg1);
}
