namespace System;

public interface IEquatable<in T> where T : allows ref struct
{
    bool Equals(T other);
}