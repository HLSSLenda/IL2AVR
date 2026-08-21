using System.Text;
namespace System.IO;

public abstract class TextWriter : IDisposable
{
    public abstract Encoding Encoding { get; }
    public virtual void Dispose()
    {
    }
    public virtual void Write(byte chr)
    {
    }
    public virtual void Write(ReadOnlySpan<byte> chars)
    {
        foreach (byte b in chars)
            Write(b);
    }
}