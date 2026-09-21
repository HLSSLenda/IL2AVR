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
    public virtual string? ReadLine(Span<char> chars)
    {
        int read = 0, value;
        while (read < chars.Length && (value = Read()) != -1)
            if ((chars[read++] = (char)value) == '\n')
                return new(chars[.. (read - 1)]);
        return null;
    }
}