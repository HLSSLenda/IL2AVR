using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Runtime.CompilerServices.IL2AVR;
using System.Runtime.InteropServices;
namespace System;

// todo fix
#pragma warning disable CS0660, CS0661, CA1067  // object doesn't have GetHashCode :D
public sealed class String : IEnumerable<char>, IEquatable<string>
#pragma warning restore CS0660, CS0661, CA1067
{
    private struct Enumerator(ushort length, ref char firstChar) : IEnumerator<char>
    {
        private unsafe readonly void* firstChar = Unsafe.AsPointer(ref firstChar);
        private short index = -1;
        public readonly unsafe char Current => Unsafe.AsRef<char>(Unsafe.Add(firstChar, index));
        readonly object IEnumerator.Current => Current;
        public bool MoveNext() => ++index < length;
        public void Reset() => index = -1;
    }
    public static unsafe readonly string Empty = new(null, 0);
    public readonly ushort Length;
    private readonly nuint firstChar;    // strings will be 8-byte based since AVR is small...

    public String()
    {
        firstChar = default;
        Length = 0;
    }
    public unsafe String(
        [AllocateFrom(at: nameof(firstChar), property: "Length")]
        ReadOnlySpan<char> span
    ) => span.CopyTo(new Span<char>((void*)firstChar, Length = span.Length));
    public unsafe String(
        byte* ptr,
        [AllocateFrom(at: nameof(firstChar))]
        [TypeAs(typeof(ushort))]
        int length) =>
            new ReadOnlySpan<char>(ptr, length).CopyTo(new Span<char>((void*)firstChar, Length = length));
    public unsafe ref readonly byte this[[TypeAs(typeof(ushort))] int index] => ref (index < Length ? ref *(byte*)Unsafe.Add((void*)firstChar, index) : ref Unsafe.NullRef<byte>());
    public static bool operator ==(string lhs, string rhs) => lhs.Equals(rhs);
    public static bool operator !=(string lhs, string rhs) => !lhs.Equals(rhs);
    public bool Equals(string other) => MemoryExtensions.SequenceEquals(AsSpan(), other.AsSpan());
    public override string? ToString() => this;
    public static implicit operator ReadOnlySpan<char>(string self) => self.AsSpan();
    public unsafe ReadOnlySpan<char> AsSpan() => new(ref firstChar, Length);
    public ref char GetPinnableReference() => ref firstChar;

    public IEnumerator<char> GetEnumerator() => new Enumerator(Length, ref firstChar);
    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
}
