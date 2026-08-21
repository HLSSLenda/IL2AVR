namespace System.IO;

public abstract class Stream : IDisposable
{
    public static readonly Stream Null = new NullStream();
    public abstract bool CanRead { get; }
    public abstract bool CanSeek { get; }
    public abstract bool CanWrite { get; }
    public virtual void Dispose()
    {
    }
    public abstract int Length { get; }
    public abstract int Position { get; }
    public abstract int Read(Span<byte> bytes);
    public abstract void Write(ReadOnlySpan<byte> bytes);
}