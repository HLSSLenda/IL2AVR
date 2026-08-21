namespace System.IO;

public abstract class TextReader : IDisposable
{
    public virtual void Dispose()
    {
    }
    public virtual int Peek() => -1;
    public virtual int Read() => -1;
    public virtual int Read(Span<byte> bytes)
    {
        int read = 0, value;
        while (read < bytes.Length && (value = Read()) != -1)
            bytes[read++] = (byte)value;
        return read;
    }
    public virtual string? ReadLine(Span<byte> bytes)
    {
        int read = 0, value;
        while (read < bytes.Length && (value = Read()) != -1)
            if ((bytes[read++] = (byte)value) == '\n')
                return new(bytes[.. (read - 1)]);
        return null;
    }
}