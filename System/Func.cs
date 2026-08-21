namespace System;

public delegate TR Func<out TR>()
    where TR : allows ref struct;
public delegate TR Func<in T1, out TR>(T1 _1)
    where T1 : allows ref struct
    where TR : allows ref struct;
public delegate TR Func<in T1, in T2, out TR>(T1 _1, T2 _2)
    where T1 : allows ref struct
    where T2 : allows ref struct;
public delegate TR Func<in T1, in T2, in T3, out TR>(T1 _1, T2 _2, T3 _3)
    where T1 : allows ref struct
    where T2 : allows ref struct
    where T3 : allows ref struct
    where TR : allows ref struct;
public delegate TR Func<in T1, in T2, in T3, in T4, out TR>(T1 _1, T2 _2, T3 _3, T4 _4)
    where T1 : allows ref struct
    where T2 : allows ref struct
    where T3 : allows ref struct
    where T4 : allows ref struct
    where TR : allows ref struct;
public delegate TR Func<in T1, in T2, in T3, in T4, in T5, out TR>(T1 _1, T2 _2, T3 _3, T4 _4, T5 _5)
    where T1 : allows ref struct
    where T2 : allows ref struct
    where T3 : allows ref struct
    where T4 : allows ref struct
    where T5 : allows ref struct
    where TR : allows ref struct;
public delegate TR Func<in T1, in T2, in T3, in T4, in T5, in T6, out TR>(T1 _1, T2 _2, T3 _3, T4 _4, T5 _5, T6 _6)
    where T1 : allows ref struct
    where T2 : allows ref struct
    where T3 : allows ref struct
    where T4 : allows ref struct
    where T5 : allows ref struct
    where T6 : allows ref struct
    where TR : allows ref struct;
public delegate TR Func<in T1, in T2, in T3, in T4, in T5, in T6, in T7, out TR>(T1 _1, T2 _2, T3 _3, T4 _4, T5 _5, T6 _6, T7 _7)
    where T1 : allows ref struct
    where T2 : allows ref struct
    where T3 : allows ref struct
    where T4 : allows ref struct
    where T5 : allows ref struct
    where T6 : allows ref struct
    where T7 : allows ref struct
    where TR : allows ref struct;
public delegate TR Func<in T1, in T2, in T3, in T4, in T5, in T6, in T7, in T8, out TR>(T1 _1, T2 _2, T3 _3, T4 _4, T5 _5, T6 _6, T7 _7, T8 _8)
    where T1 : allows ref struct
    where T2 : allows ref struct
    where T3 : allows ref struct
    where T4 : allows ref struct
    where T5 : allows ref struct
    where T6 : allows ref struct
    where T7 : allows ref struct
    where T8 : allows ref struct
    where TR : allows ref struct;