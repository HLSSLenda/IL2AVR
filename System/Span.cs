using System.Runtime.CompilerServices;
using System.Runtime.CompilerServices.IL2AVR;
namespace System;

public readonly unsafe ref struct Span<T> where T : allows ref struct
{
    private readonly ushort length;
    [TypeAs(typeof(ushort))]
    public int Length => length;
    internal readonly void* reference;

    public Span(ref T reference)
    {
        this.reference = Unsafe.AsPointer(ref reference);
        length = 1;
    }
    internal Span(ref T reference, int length)
    {
        this.reference = Unsafe.AsPointer(ref reference);
        this.length = length;
    }
    public unsafe Span(void* pointer, int length)
    {
        reference = pointer;
        this.length = length;
    }
    public static unsafe implicit operator ReadOnlySpan<T>(Span<T> span) => new(span.reference, span.Length);
    public ref T this[int index] => ref Unsafe.AsRef<T>(Unsafe.Add(reference, Unsafe.SizeOf<T>() * index));
    public Span<T> Slice(int start, int end) => new(Unsafe.Add(reference, start), end - start);
}