namespace System;

public readonly struct Nullable<T>(T value) where T : struct
{
    public readonly T Value = value;
    public readonly bool HasValue = true;
}