using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;
namespace Arduino;

public static class Pin
{
    public const byte DDRB = 0x4, PORTB = 0x5,
                      DDRC = 0x7, PORTC = 0x8,
                      DDRD = 0xA, PORTD = 0xB;
    public const PortD D0 = PortD.D0,
                       D1 = PortD.D1,
                       D2 = PortD.D2,
                       D3 = PortD.D3,
                       D4 = PortD.D4,
                       D5 = PortD.D5,
                       D6 = PortD.D6,
                       D7 = PortD.D7;
    public const PortB D8 = PortB.B0,
                       D9 = PortB.B1,
                       D10 = PortB.B2,
                       D11 = PortB.B3,
                       D12 = PortB.B4,
                       D13 = PortB.B5;
    public const PortC A0 = PortC.C0, D14 = PortC.C0,
                       A1 = PortC.C1, D15 = PortC.C1,
                       A2 = PortC.C2, D16 = PortC.C2,
                       A3 = PortC.C3, D17 = PortC.C3,
                       A4 = PortC.C4, D18 = PortC.C4,
                       A5 = PortC.C5, D19 = PortC.C5;
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static unsafe void Input(this PortD pin) => *(byte*)DDRD &= ~(1 << (byte)pin);
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static unsafe void Input(this PortB pin) => *(byte*)DDRB &= ~(1 << (byte)pin);
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static unsafe void Input(this PortC pin) => *(byte*)DDRC &= ~(1 << (byte)pin);
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static unsafe void Output(this PortD pin) => *(byte*)DDRD |= 1 << (byte)pin;
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static unsafe void Output(this PortB pin) => *(byte*)DDRB |= 1 << (byte)pin;
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static unsafe void Output(this PortC pin) => *(byte*)DDRC |= 1 << (byte)pin;
    private static unsafe void Mode([ConstantExpected] byte ptr, byte pin, [ConstantExpected] PinMode mode) =>
        *(byte*)ptr = (byte)(mode == PinMode.Input ?
            *(byte*)ptr & ~(1 << pin) : *(byte*)ptr | (1 << pin));
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static unsafe void Mode(this PortD pin, [ConstantExpected] PinMode mode) => Mode(DDRD, (byte)pin, mode);
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static unsafe void Mode(this PortB pin, [ConstantExpected] PinMode mode) => Mode(DDRB, (byte)pin, mode);
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static unsafe void Mode(this PortC pin, [ConstantExpected] PinMode mode) => Mode(DDRC, (byte)pin, mode);
    public static void Mode(int pin, [ConstantExpected] PinMode mode)
    {
        if (pin < 8)
            Mode(DDRD, (byte)pin, mode);
        else if (pin < 14)
            Mode(DDRB, (byte)pin, mode);
        else if (pin < 20)
            Mode(DDRC, (byte)pin, mode);
    }
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static unsafe bool ReadBool(this PortD pin) => (*(byte*)PORTD & (1 << (byte)pin)) == 1;
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static unsafe bool ReadBool(this PortB pin) => (*(byte*)PORTB & (1 << (byte)pin)) == 1;
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static unsafe bool ReadBool(this PortC pin) => (*(byte*)PORTC & (1 << (byte)pin)) == 1;
    public static bool ReadBool(int pin)
    {
        if (pin < 8)
            return ReadBool((PortD)pin);
        return pin < 14 ? ReadBool((PortB)pin) : pin < 20 && ReadBool((PortC)pin);
    }
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static unsafe void WriteTrue(this PortD pin) => *(byte*)PORTD |= (byte)(1 << (byte)pin);
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static unsafe void WriteTrue(this PortB pin) => *(byte*)PORTB |= (byte)(1 << (byte)pin);
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static unsafe void WriteTrue(this PortC pin) => *(byte*)PORTC |= (byte)(1 << (byte)pin);
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static unsafe void WriteFalse(this PortD pin) => *(byte*)PORTD &= (byte)~(1 << (byte)pin);
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static unsafe void WriteFalse(this PortB pin) => *(byte*)PORTB &= (byte)~(1 << (byte)pin);
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static unsafe void WriteFalse(this PortC pin) => *(byte*)PORTC &= (byte)~(1 << (byte)pin);
    private static unsafe void Write([ConstantExpected] byte ptr, byte pin, bool value) =>
        *(byte*)ptr = value ? *(byte*)ptr | (byte)(1 << pin) : *(byte*)ptr & (byte)~(1 << pin);
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void Write(this PortD pin, bool value) => Write(PORTD, (byte)pin, value);
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void Write(this PortB pin, bool value) => Write(PORTB, (byte)pin, value);
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void Write(this PortC pin, bool value) => Write(PORTC, (byte)pin, value);
    public static void Write(int pin, bool value)
    {
        if (pin < 8)
            Write((PortD)pin, value);
        else if (pin < 14)
            Write((PortB)pin, value);
        else if (pin < 20)
            Write((PortC)pin, value);
    }
}