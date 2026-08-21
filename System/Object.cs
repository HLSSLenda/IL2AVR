using System.Runtime.CompilerServices;
namespace System;

/*
 * IL2AVR object layout:
 * ushort size => the size of the memory block allocated for this object
 * void* vtable => pointer to vtable (contains the typeid too)
 *     vtable structure:
 *     ushort typeid => the typeid of the object
 *     (...) => actual vtable
 * (...) => contents of object
 */

public class Object
{
    public static bool Equals(object? lhs, object? rhs) => lhs is null ? rhs is null : lhs.Equals(rhs);
    public virtual bool Equals(object? other) => ReferenceEquals(this, other);
    public unsafe Type GetType() =>
        new(Unsafe.Read<ushort>(Unsafe.AsPointer(ref Unsafe.Add(ref Unsafe.As<ushort>(this), 1))));
    public static bool ReferenceEquals(object? lhs, object? rhs) => Unsafe.AreSame(in lhs, in rhs);
    public virtual string? ToString() => null;
}