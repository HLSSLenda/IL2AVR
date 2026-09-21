using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
namespace System;

public static class MemoryExtensions
{
    public static ReadOnlySpan<char> AsSpan(this string? str) => str is null ? default : str.AsSpan();
    public static void CopyTo<T>(this ReadOnlySpan<T> src, Span<T> dst) where T : allows ref struct
    {
        ref T sp = ref Unsafe.AsRef(in MemoryMarshal.GetReference(src));
        ref T dp = ref MemoryMarshal.GetReference(dst);
        for (nint i = 0; i < Math.Min(src.Length, dst.Length); ++i)
            Unsafe.Add(ref dp, i) = Unsafe.Add(ref sp, i);
    }
    public static unsafe bool SequenceEquals<T>(
        this ReadOnlySpan<T> lhs,
        ReadOnlySpan<T> rhs,
        IEqualityComparer<T>? comparer = null) where T : allows ref struct
    {
        if (lhs.Length != rhs.Length)
            return false;
        if (comparer is null)
        {
            // TODO: Check what Microsoft does in regards of value type and reference types
            if (typeof(T).IsValueType)
            {
                
            }
            else
            {

            }
        }
        else
        {
            ref T lhp = ref Unsafe.AsRef(in MemoryMarshal.GetReference(lhs));
            ref T rhp = ref Unsafe.AsRef(in MemoryMarshal.GetReference(rhs));
            for (int i = 0; i < lhs.Length; ++i)
                if (!comparer.Equals(Unsafe.Add(ref lhp, i), Unsafe.Add(ref rhp, i)))
                    return false;
        }
        return true;
    }
    public static void ToLower(this Span<char> text)
    {
        
    }
    public static string ToString(this ReadOnlySpan<char> str) => new(str);
}