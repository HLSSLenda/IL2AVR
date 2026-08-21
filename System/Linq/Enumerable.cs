using System;
using System.Collections;
using System.Collections.Generic;
namespace System.Linq;

public static class Enumerable
{
    public readonly struct RangeEnumerable(int begin, int end) : IEnumerable<int>
    {
        public struct RangeEnumerator(int begin, int end) : IEnumerator<int>
        {
            private readonly int begin = begin;
            private int current = begin;
            public readonly int Current => current;
            readonly object IEnumerator.Current => Current;
            public bool MoveNext() => ++current != end;
            public void Reset() => current = begin;
        }
        RangeEnumerator GetEnumerator() => new(begin, end);
        IEnumerator<int> IEnumerable<int>.GetEnumerator() => GetEnumerator();
        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }
    public static bool Any<T>(this IEnumerable<T> enumerable) where T : allows ref struct =>
        enumerable.GetEnumerator().MoveNext();
    public static bool Any<T>(this IEnumerable<T> enumerable, Func<T, bool> predicate) where T : allows ref struct
    {
        foreach (T value in enumerable)
            if (predicate(value))
                return true;
        return false;
    }
    public static bool All<T>(this IEnumerable<T> enumerable, Func<T, bool> predicate) where T : allows ref struct
    {
        foreach (T value in enumerable)
            if (!predicate(value))
                return false;
        return true;
    }
    public static int Count<T>(this IEnumerable<T> enumerable) where T : allows ref struct
    {
        int n = 0;
        IEnumerator<T> enumerator = enumerable.GetEnumerator();
        while (enumerator.MoveNext())
            ++n;
        return n;
    }
    public static T MaxBy<T, U>(this IEnumerable<T> enumerable, Func<T, U> classifier)
        where T : allows ref struct
        where U : IComparable<U>, allows ref struct
    {

    }
    public static RangeEnumerable Range(int start, int end) => new(start, end);
}