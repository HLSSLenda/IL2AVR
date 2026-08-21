namespace System;

public readonly struct Index(int value, bool fromEnd = false)
{
    public readonly bool IsFromEnd = fromEnd;
    public readonly int Value = value;
    public static implicit operator Index(int value) => new(value);
    public int GetOffset(int length) => Value + length;
}