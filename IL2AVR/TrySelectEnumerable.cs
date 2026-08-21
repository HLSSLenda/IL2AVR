using System;
using System.Collections;
using System.Collections.Generic;
namespace IL2AVR;

public readonly struct TrySelectEnumerable<T, U>(IEnumerable<T> enumerable, Func<T, U> selector, Func<U, bool> condition, TrySelectMode mode) : IEnumerable<U>
    where T : allows ref struct
    where U : allows ref struct
{
    public struct StopEnumerator(IEnumerator<T> enumerator, Func<T, U> selector, Func<U, bool> condition) : IEnumerator<U>
    {
        private bool iter = true;
        public readonly U Current => selector(enumerator.Current);
        readonly object? IEnumerator.Current => null;
        public readonly void Dispose() => enumerator.Dispose();
        public bool MoveNext() => iter && enumerator.MoveNext() && (iter = condition(Current));
        public void Reset()
        {
            enumerator.Reset();
            iter = false;
        }
    }
    public readonly struct ThrowEnumerator(IEnumerator<T> enumerator, Func<T, U> selector, Func<U, bool> condition) : IEnumerator<U>
    {
        public U Current => selector(enumerator.Current);
        object? IEnumerator.Current => null;
        public void Dispose() => enumerator.Dispose();
        public bool MoveNext()
        {
            if (enumerator.MoveNext())
            {
                if (condition(Current))
                    return true;
                throw new AggregateException();
            }
            return false;
        }
        public void Reset() => enumerator.Reset();
    }
    public readonly struct WhereEnumerator(IEnumerator<T> enumerator, Func<T, U> selector, Func<U, bool> condition) : IEnumerator<U>
    {
        public U Current => selector(enumerator.Current);
        object? IEnumerator.Current => null;
        public void Dispose() => enumerator.Dispose();
        public bool MoveNext()
        {
            while (enumerator.MoveNext())
                if (condition(Current))
                    return true;
            return false;
        }
        public void Reset() => enumerator.Reset();
    }
    
    public IEnumerator<U> GetEnumerator() => mode switch
    {
        TrySelectMode.Stop => new StopEnumerator(enumerable.GetEnumerator(), selector, condition),
        TrySelectMode.Throw => new ThrowEnumerator(enumerable.GetEnumerator(), selector, condition),
        TrySelectMode.Where => new WhereEnumerator(enumerable.GetEnumerator(), selector, condition),
        _ => throw new InvalidOperationException()
    };
    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();    
}