using System.Runtime.CompilerServices;
namespace System;

#pragma warning disable CS0659 // Type overrides Object.Equals(object o) but does not override Object.GetHashCode()
public sealed class Tuple : IEquatable<Tuple>, IEquatable<ITuple>, ITuple
#pragma warning restore CS0659 // Type overrides Object.Equals(object o) but does not override Object.GetHashCode()
{
    public object? this[int index] => null;
    public ushort Length => 0;
    public bool Equals(Tuple other) => true;
    public bool Equals(ITuple other) => Length == other.Length;
    public override bool Equals(object? other) => other is Tuple;
}

public sealed class Tuple<T>(T item) : ITuple
{
    public T Item1 = item;
    public object? this[int index] => index == 0 ? Item1 : null;
    public ushort Length => 1;
    public void Deconstruct(out T item1) => item1 = Item1;
}

public sealed class Tuple<T1, T2>(T1 item1, T2 item2) : ITuple
{
    public T1 Item1 = item1;
    public T2 Item2 = item2;
    public object? this[int index] => index switch
    {
        0 => Item1,
        1 => Item2,
        _ => null
    };
    public ushort Length => 2;
    public void Deconstruct(out T1 item1, out T2 item2)
    {
        item1 = Item1;
        item2 = Item2;
    }
}

public sealed class Tuple<T1, T2, T3>(T1 item1, T2 item2, T3 item3) : ITuple
{
    public T1 Item1 = item1;
    public T2 Item2 = item2;
    public T3 Item3 = item3;
    public object? this[int index] => index switch
    {
        0 => Item1,
        1 => Item2,
        2 => Item3,
        _ => null
    };
    public ushort Length => 3;
    public void Deconstruct(out T1 item1, out T2 item2, out T3 item3)
    {
        item1 = Item1;
        item2 = Item2;
        item3 = Item3;
    }
}
public sealed class Tuple<T1, T2, T3, T4>(T1 item1, T2 item2, T3 item3, T4 item4) : ITuple
{
    public T1 Item1 = item1;
    public T2 Item2 = item2;
    public T3 Item3 = item3;
    public T4 Item4 = item4;
    public object? this[int index] => index switch
    {
        0 => Item1,
        1 => Item2,
        2 => Item3,
        3 => Item4,
        _ => null
    };
    public ushort Length => 4;
    public void Deconstruct(out T1 item1, out T2 item2, out T3 item3, out T4 item4)
    {
        item1 = Item1;
        item2 = Item2;
        item3 = Item3;
        item4 = Item4;
    }
}
public sealed class Tuple<T1, T2, T3, T4, T5>(T1 item1, T2 item2, T3 item3, T4 item4, T5 item5) : ITuple
{
    public T1 Item1 = item1;
    public T2 Item2 = item2;
    public T3 Item3 = item3;
    public T4 Item4 = item4;
    public T5 Item5 = item5;
    public object? this[int index] => index switch
    {
        0 => Item1,
        1 => Item2,
        2 => Item3,
        3 => Item4,
        4 => Item5,
        _ => null
    };
    public ushort Length => 5;
    public void Deconstruct(out T1 item1, out T2 item2, out T3 item3, out T4 item4, out T5 item5)
    {
        item1 = Item1;
        item2 = Item2;
        item3 = Item3;
        item4 = Item4;
        item5 = Item5;
    }
}
public sealed class Tuple<T1, T2, T3, T4, T5, T6>(T1 item1, T2 item2, T3 item3, T4 item4, T5 item5, T6 item6) : ITuple
{
    public T1 Item1 = item1;
    public T2 Item2 = item2;
    public T3 Item3 = item3;
    public T4 Item4 = item4;
    public T5 Item5 = item5;
    public T6 Item6 = item6;
    public object? this[int index] => index switch
    {
        0 => Item1,
        1 => Item2,
        2 => Item3,
        3 => Item4,
        4 => Item5,
        5 => Item6,
        _ => null
    };
    public ushort Length => 6;
    public void Deconstruct(out T1 item1, out T2 item2, out T3 item3, out T4 item4, out T5 item5, out T6 item6)
    {
        item1 = Item1;
        item2 = Item2;
        item3 = Item3;
        item4 = Item4;
        item5 = Item5;
        item6 = Item6;
    }
}
public sealed class Tuple<T1, T2, T3, T4, T5, T6, T7>(T1 item1, T2 item2, T3 item3, T4 item4, T5 item5, T6 item6, T7 item7) : ITuple
{
    public T1 Item1 = item1;
    public T2 Item2 = item2;
    public T3 Item3 = item3;
    public T4 Item4 = item4;
    public T5 Item5 = item5;
    public T6 Item6 = item6;
    public T7 Item7 = item7;
    public object? this[int index] => index switch
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
    public ushort Length => 7;
    public void Deconstruct(out T1 item1, out T2 item2, out T3 item3, out T4 item4, out T5 item5, out T6 item6, out T7 item7)
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
public sealed class Tuple<T1, T2, T3, T4, T5, T6, T7, TRest>(T1 item1, T2 item2, T3 item3, T4 item4, T5 item5, T6 item6, T7 item7, TRest rest) : ITuple
{
    public T1 Item1 = item1;
    public T2 Item2 = item2;
    public T3 Item3 = item3;
    public T4 Item4 = item4;
    public T5 Item5 = item5;
    public T6 Item6 = item6;
    public T7 Item7 = item7;
    public TRest Rest = rest;
    public object? this[int index] => index switch
    {
        0 => Item1,
        1 => Item2,
        2 => Item3,
        3 => Item4,
        4 => Item5,
        5 => Item6,
        6 => Item7,
        _ => Rest is ITuple tuple ? tuple[index - 7] : Rest
    };
    public ushort Length => Rest is ITuple tuple ? 7 + tuple.Length : 8;
    public void Deconstruct(out T1 item1, out T2 item2, out T3 item3, out T4 item4, out T5 item5, out T6 item6, out T7 item7, out TRest rest)
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