using System.Runtime.CompilerServices;
namespace System;

public readonly struct Int32
{
    public static implicit operator byte(int self) => Unsafe.As<int, byte>(ref self);
}