using System.Runtime.CompilerServices;
namespace System.Runtime.InteropServices;

public static class MemoryMarshal
{
    public static unsafe Span<TTo> As<TFrom, TTo>(Span<TFrom> span)
        where TFrom : allows ref struct
        where TTo : allows ref struct =>
            new(ref Unsafe.AsRef<TTo>(span.reference), span.Length);
    public static unsafe ReadOnlySpan<TTo> As<TFrom, TTo>(ReadOnlySpan<TFrom> span)
        where TFrom : allows ref struct
        where TTo : allows ref struct =>
            new(ref Unsafe.AsRef<TTo>(span.reference), span.Length);
    public static unsafe ref T GetReference<T>(ReadOnlySpan<T> span) where T : allows ref struct => ref Unsafe.AsRef<T>(span.reference);
}