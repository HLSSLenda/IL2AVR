using System.Runtime.CompilerServices;
namespace System;

public readonly struct Char
{
    public static implicit operator byte(char self) => Unsafe.As<char, byte>(ref self);
}