using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
namespace System;

public static class MemoryExtensions
{
    public static void CopyTo<T>(this ReadOnlySpan<T> src, Span<T> dst) where T : allows ref struct
    {
        ref T sp = ref MemoryMarshal.GetReference(src);
        ref T dp = ref MemoryMarshal.GetReference((ReadOnlySpan<T>)dst);
        for (nint i = 0; i < Math.Min(src.Length, dst.Length); ++i)
        {
            dp = sp;
            sp = ref Unsafe.Add(ref sp, 1);
            dp = ref Unsafe.Add(ref dp, 1);
        }
    }
    public static unsafe bool SequenceEquals<T>(this ReadOnlySpan<T> lhs, ReadOnlySpan<T> rhs,
        IEqualityComparer<T>? comparer = null) where T : allows ref struct
    {
        if (lhs.Length != rhs.Length)
            return false;
        if (comparer is null)
        {
            if (typeof(T).IsValueType)
            {
                
            }
            else
            {

            }
        }
        else
        {
            for (int i = 0; i < lhs.Length; ++i)
                if (!comparer.Equals(Unsafe.Add(ref Unsafe.AsRef<T>(lhs.reference), i),
                                     Unsafe.Add(ref Unsafe.AsRef<T>(rhs.reference), i)))
                    return false;
        }
        return true;
    }
    public static void ToLower(this Span<byte> text)
    {
        
    }
    public static string ToString(this ReadOnlySpan<byte> str) => new(str);
}