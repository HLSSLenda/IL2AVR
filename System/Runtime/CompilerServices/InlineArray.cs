namespace System.Runtime.CompilerServices;

[AttributeUsage(AttributeTargets.Struct, AllowMultiple=false)]
public sealed class InlineArrayAttribute(int length) : Attribute
{
    public readonly int Length = length;
}

#pragma warning disable CS9084
[InlineArray(1)]
public struct InlineArray1<T>
{
    private T value;
    public unsafe ref T this[byte index] => ref index == 0 ? ref value : ref Unsafe.NullRef<T>();
}

[InlineArray(2)]
public struct InlineArray2<T>
{
    private T value;
    public unsafe ref T this[byte index] => ref index < 2 ? ref Unsafe.Add(ref value, index) : ref Unsafe.NullRef<T>();
}

[InlineArray(3)]
public struct InlineArray3<T>
{
    private T value;
    public unsafe ref T this[byte index] => ref index < 3 ? ref Unsafe.Add(ref value, index) : ref Unsafe.NullRef<T>();
}

[InlineArray(4)]
public struct InlineArray4<T>
{
    private T value;
    public unsafe ref T this[byte index] => ref index < 4 ? ref Unsafe.Add(ref value, index) : ref Unsafe.NullRef<T>();
}

[InlineArray(5)]
public struct InlineArray5<T>
{
    private T value;
    public unsafe ref T this[byte index] => ref index < 5 ? ref Unsafe.Add(ref value, index) : ref Unsafe.NullRef<T>();
}

[InlineArray(6)]
public struct InlineArray6<T>
{
    private T value;
    public unsafe ref T this[byte index] => ref index < 6 ? ref Unsafe.Add(ref value, index) : ref Unsafe.NullRef<T>();
}

[InlineArray(7)]
public struct InlineArray7<T>
{
    private T value;
    public unsafe ref T this[byte index] => ref index < 7 ? ref Unsafe.Add(ref value, index) : ref Unsafe.NullRef<T>();
}

[InlineArray(8)]
public struct InlineArray8<T>
{
    private T value;
    public unsafe ref T this[byte index] => ref index < 8 ? ref Unsafe.Add(ref value, index) : ref Unsafe.NullRef<T>();
}

[InlineArray(9)]
public struct InlineArray9<T>
{
    private T value;
    public unsafe ref T this[byte index] => ref index < 9 ? ref Unsafe.Add(ref value, index) : ref Unsafe.NullRef<T>();
}

[InlineArray(10)]
public struct InlineArray10<T>
{
    private T value;
    public unsafe ref T this[byte index] => ref index < 10 ? ref Unsafe.Add(ref value, index) : ref Unsafe.NullRef<T>();
}

[InlineArray(11)]
public struct InlineArray11<T>
{
    private T value;
    public unsafe ref T this[byte index] => ref index < 11 ? ref Unsafe.Add(ref value, index) : ref Unsafe.NullRef<T>();
}

[InlineArray(12)]
public struct InlineArray12<T>
{
    private T value;
    public unsafe ref T this[byte index] => ref index < 12 ? ref Unsafe.Add(ref value, index) : ref Unsafe.NullRef<T>();
}

[InlineArray(13)]
public struct InlineArray13<T>
{
    private T value;
    public unsafe ref T this[byte index] => ref index < 13 ? ref Unsafe.Add(ref value, index) : ref Unsafe.NullRef<T>();
}

[InlineArray(14)]
public struct InlineArray14<T>
{
    private T value;
    public unsafe ref T this[byte index] => ref index < 14 ? ref Unsafe.Add(ref value, index) : ref Unsafe.NullRef<T>();
}

[InlineArray(15)]
public struct InlineArray15<T>
{
    private T value;
    public unsafe ref T this[byte index] => ref index < 15 ? ref Unsafe.Add(ref value, index) : ref Unsafe.NullRef<T>();
}

[InlineArray(16)]
public struct InlineArray16<T>
{
    private T value;
    public unsafe ref T this[byte index] => ref index < 16 ? ref Unsafe.Add(ref value, index) : ref Unsafe.NullRef<T>();
}
#pragma warning restore CS9084