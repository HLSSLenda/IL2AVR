using System.Runtime.CompilerServices;
namespace System;

public interface ISpanParsable<T> : IParsable<T> where T : ISpanParsable<T>, allows ref struct
{
    T? IParsable<T>.Parse(string? str) => str is null ? default : Parse(str.AsSpan());
    bool IParsable<T>.TryParse(string? str, out T value)
    {
        if (str is null)
        {
            Unsafe.SkipInit(out value);
            return false;
        }
        return TryParse(str.AsSpan(), out value);
    }
    T? Parse(ReadOnlySpan<char> str);
    bool TryParse(ReadOnlySpan<char> str, out T value);
}