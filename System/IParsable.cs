namespace System;

public interface IParsable<T> where T : IParsable<T>, allows ref struct
{
    T? Parse(string? str);
    bool TryParse(string? str, out T value);
}