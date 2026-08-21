using System.Runtime.CompilerServices;
namespace System;

#pragma warning disable CS0659 // Type overrides Object.Equals(object o) but does not override Object.GetHashCode()
public readonly struct ValueTuple : IEquatable<ValueTuple>, IEquatable<ITuple>, ITuple
#pragma warning restore CS0659 // Type overrides Object.Equals(object o) but does not override Object.GetHashCode()
{
    public readonly object? this[int index] => null;
    public readonly ushort Length => 0;

    public static ValueTuple Create() => default;
    public static ValueTuple<T> Create<T>(T value) => new(value);
    public static (T1, T2) Create<T1, T2>(T1 item1, T2 item2) => (item1, item2);
    public static (T1, T2, T3) Create<T1, T2, T3>(T1 item1, T2 item2, T3 item3) => (item1, item2, item3);
    public static (T1, T2, T3, T4) Create<T1, T2, T3, T4>(T1 item1, T2 item2, T3 item3, T4 item4) => (item1, item2, item3, item4);
    public static (T1, T2, T3, T4, T5) Create<T1, T2, T3, T4, T5>(T1 item1, T2 item2, T3 item3, T4 item4, T5 item5) => (item1, item2, item3, item4, item5);
    public static (T1, T2, T3, T4, T5, T6) Create<T1, T2, T3, T4, T5, T6>(T1 item1, T2 item2, T3 item3, T4 item4, T5 item5, T6 item6) => (item1, item2, item3, item4, item5, item6);
    public static (T1, T2, T3, T4, T5, T6, T7) Create<T1, T2, T3, T4, T5, T6, T7>(T1 item1, T2 item2, T3 item3, T4 item4, T5 item5, T6 item6, T7 item7) => (item1, item2, item3, item4, item5, item6, item7);

    public override bool Equals(object? obj) => obj is ValueTuple;
    public bool Equals(ValueTuple other) => true;
    public bool Equals(ITuple other) => Length == other.Length;
}
public struct ValueTuple<T>(T value) : ITuple
{
    public ValueTuple(Tuple<T> tuple) : this(tuple.Item1)
    {
    }
    public static implicit operator ValueTuple<T>(Tuple<T> tuple) => new(tuple);
    public T Item1 = value;
    public readonly object? this[int index] => index == 0 ? Item1 : null;
    public readonly ushort Length => 1;
    public readonly void Deconstruct(out T value) => value = Item1;
}
public struct ValueTuple<T1, T2>(T1 item1, T2 item2) : ITuple
{
    public ValueTuple(Tuple<T1, T2> tuple) : this(tuple.Item1, tuple.Item2)
    {
    }
    public static implicit operator ValueTuple<T1, T2>(Tuple<T1, T2> tuple) => new(tuple);
    public T1 Item1 = item1;
    public T2 Item2 = item2;
    public readonly object? this[int index] => index switch
    {
        0 => Item1,
        1 => Item2,
        _ => null
    };
    public readonly ushort Length => 2;
    public readonly void Deconstruct(out T1 item1, out T2 item2)
    {
        item1 = Item1;
        item2 = Item2;
    }
}
public struct ValueTuple<T1, T2, T3>(T1 item1, T2 item2, T3 item3) : ITuple
{
    public T1 Item1 = item1;
    public T2 Item2 = item2;
    public T3 Item3 = item3;
    public readonly object? this[int index] => index switch
    {
        0 => Item1,
        1 => Item2,
        2 => Item3,
        _ => null
    };
    public readonly ushort Length => 3;
    public readonly void Deconstruct(out T1 item1, out T2 item2, out T3 item3)
    {
        item1 = Item1;
        item2 = Item2;
        item3 = Item3;
    }
}
public struct ValueTuple<T1, T2, T3, T4>(T1 item1, T2 item2, T3 item3, T4 item4) : ITuple
{
    public T1 Item1 = item1;
    public T2 Item2 = item2;
    public T3 Item3 = item3;
    public T4 Item4 = item4;
    public readonly object? this[int index] => index switch
    {
        0 => Item1,
        1 => Item2,
        2 => Item3,
        3 => Item4,
        _ => null
    };
    public readonly ushort Length => 4;
    public readonly void Deconstruct(out T1 item1, out T2 item2, out T3 item3, out T4 item4)
    {
        item1 = Item1;
        item2 = Item2;
        item3 = Item3;
        item4 = Item4;
    }
}
public struct ValueTuple<T1, T2, T3, T4, T5>(T1 item1, T2 item2, T3 item3, T4 item4, T5 item5) : ITuple
{
    public T1 Item1 = item1;
    public T2 Item2 = item2;
    public T3 Item3 = item3;
    public T4 Item4 = item4;
    public T5 Item5 = item5;
    public readonly object? this[int index] => index switch
    {
        0 => Item1,
        1 => Item2,
        2 => Item3,
        3 => Item4,
        4 => Item5,
        _ => null
    };
    public readonly ushort Length => 5;
    public readonly void Deconstruct(out T1 item1, out T2 item2, out T3 item3, out T4 item4, out T5 item5)
    {
        item1 = Item1;
        item2 = Item2;
        item3 = Item3;
        item4 = Item4;
        item5 = Item5;
    }
}
public struct ValueTuple<T1, T2, T3, T4, T5, T6>(T1 item1, T2 item2, T3 item3, T4 item4, T5 item5, T6 item6) : ITuple
{
    public T1 Item1 = item1;
    public T2 Item2 = item2;
    public T3 Item3 = item3;
    public T4 Item4 = item4;
    public T5 Item5 = item5;
    public T6 Item6 = item6;
    public readonly object? this[int index] => index switch
    {
        0 => Item1,
        1 => Item2,
        2 => Item3,
        3 => Item4,
        4 => Item5,
        5 => Item6,
        _ => null
    };
    public readonly ushort Length => 6;
    public readonly void Deconstruct(out T1 item1, out T2 item2, out T3 item3, out T4 item4, out T5 item5, out T6 item6)
    {
        item1 = Item1;
        item2 = Item2;
        item3 = Item3;
        item4 = Item4;
        item5 = Item5;
        item6 = Item6;
    }
}
public struct ValueTuple<T1, T2, T3, T4, T5, T6, T7>(T1 item1, T2 item2, T3 item3, T4 item4, T5 item5, T6 item6, T7 item7) : ITuple
{
    public T1 Item1 = item1;
    public T2 Item2 = item2;
    public T3 Item3 = item3;
    public T4 Item4 = item4;
    public T5 Item5 = item5;
    public T6 Item6 = item6;
    public T7 Item7 = item7;
    public readonly object? this[int index] => index switch
    {
        0 => Item1,
        1 => Item2,
        2 => Item3,
        3 => Item4,
        4 => Item5,
        5 => Item6,
        6 => Item7,
        _ => null
    };
    public readonly ushort Length => 7;
    public readonly void Deconstruct(out T1 item1, out T2 item2, out T3 item3, out T4 item4, out T5 item5, out T6 item6, out T7 item7)
    {
        item1 = Item1;
        item2 = Item2;
        item3 = Item3;
        item4 = Item4;
        item5 = Item5;
        item6 = Item6;
        item7 = Item7;
    }
}
public struct ValueTuple<T1, T2, T3, T4, T5, T6, T7, TRest>(T1 item1, T2 item2, T3 item3, T4 item4, T5 item5, T6 item6, T7 item7, TRest rest) : ITuple
    where TRest : struct, ITuple
{
    public T1 Item1 = item1;
    public T2 Item2 = item2;
    public T3 Item3 = item3;
    public T4 Item4 = item4;
    public T5 Item5 = item5;
    public T6 Item6 = item6;
    public T7 Item7 = item7;
    public TRest Rest = rest;
    public readonly object? this[int index] => index switch
    {
        0 => Item1,
        1 => Item2,
        2 => Item3,
        3 => Item4,
        4 => Item5,
        5 => Item6,
        6 => Item7,
        _ => Rest[index - 7]
    };
    public readonly ushort Length => 7 + Rest.Length;
    public readonly void Deconstruct(out T1 item1, out T2 item2, out T3 item3, out T4 item4, out T5 item5, out T6 item6, out T7 item7, out TRest rest)
    {
        item1 = Item1;
        item2 = Item2;
        item3 = Item3;
        item4 = Item4;
        item5 = Item5;
        item6 = Item6;
        item7 = Item7;
        rest = Rest;
    }
}