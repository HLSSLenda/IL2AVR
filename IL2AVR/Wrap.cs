namespace IL2AVR;

/// <summary>Represents an explicitly wrapped type.</summary>
/// <param name="Value">The value to box.</param>
public sealed record Wrap<T>(T Value)
{
    /// <summary>The wrapped value.</summary>
    public T Value = Value;

    public static implicit operator Wrap<T>(T value) => new(value);
    public static implicit operator T(Wrap<T> value) => value.Value;
}
