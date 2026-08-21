namespace System.Runtime.CompilerServices;

public interface ITuple
{
    object? this[int index] { get; }
    ushort Length { get; }
}