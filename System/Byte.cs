using System.Numerics;
namespace System;

public readonly struct Byte : IComparable<byte>, IEquatable<byte>, IMinMaxValue<byte>
{
    public const byte MaxValue = 255, MinValue = 0;
    static byte IMinMaxValue<byte>.MinValue => MinValue;
    static byte IMinMaxValue<byte>.MaxValue => MaxValue;

    public int CompareTo(byte other) => this - other;

    public int CompareTo(object? obj) => obj is byte b ? this - b : 0;
}
