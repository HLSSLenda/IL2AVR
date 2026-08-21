namespace IL2AVR;

public sealed record Boxed<T>(T Value) where T : struct
{
    public T Value = Value;

    public static implicit operator Boxed<T>(T value) => new(value);
    public static implicit operator T(Boxed<T> value) => value.Value;
}
