namespace System;

public readonly unsafe struct UIntPtr
{
    public const int Size = 2;
    public static readonly UIntPtr Zero = new();
}