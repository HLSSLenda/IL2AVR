using System.Runtime.CompilerServices;
namespace System;

public abstract class Array
{
    public ushort Length => Unsafe.As<ushort>(this);
    public long LongLength => Length;
}