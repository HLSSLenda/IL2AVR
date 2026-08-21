using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Runtime.CompilerServices.IL2AVR;
namespace System;

public readonly unsafe ref struct ReadOnlySpan<T> : IEnumerable<T> where T : allows ref struct
{
    public ref struct Enumerator(ref T reference, int length) : IEnumerator<T>
    {
        private int index = -1;
        private readonly void* reference = Unsafe.AsPointer(ref reference);
        public readonly ref readonly T Current => ref Unsafe.Add(ref Unsafe.AsRef<T>(reference), length);
        readonly T IEnumerator<T>.Current => Current;
        readonly object IEnumerator.Current => null!;
        public bool MoveNext() => ++index < length;
        public void Reset() => index = 0;
    }
    private readonly ushort length;
    [TypeAs(typeof(ushort))]
    public int Length => length;
    internal readonly void* reference;

    public ReadOnlySpan(ref readonly T reference)
    {
        this.reference = Unsafe.AsPointer(ref Unsafe.AsRef(in reference));
        length = 1;
    }
    internal ReadOnlySpan(ref T reference, int length)
    {
        this.reference = Unsafe.AsPointer(ref reference);
        this.length = length;
    }
    public unsafe ReadOnlySpan(void* pointer, int length)
    {
        reference = pointer;
        this.length = length;
    }

    public ref readonly T this[int index] => ref Unsafe.Add(ref Unsafe.AsRef<T>(reference), index);
    public ReadOnlySpan<T> Slice(int start, int end) => new(Unsafe.Add(reference, start), end - start);

    public Enumerator GetEnumerator() => new(ref Unsafe.AsRef<T>(reference), length);
    IEnumerator<T> IEnumerable<T>.GetEnumerator() => null!;
    IEnumerator IEnumerable.GetEnumerator() => null!;
}