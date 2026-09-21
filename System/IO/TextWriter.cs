using System.Text;
namespace System.IO;

public abstract class TextWriter : IDisposable
{
    public abstract Encoding Encoding { get; }
    public virtual void Dispose()
    {
    }
    public virtual void Write(char chr)
    {
    }
    public virtual void Write(bool value) => Write(value ? "True" : "False");
    public virtual void Write(ReadOnlySpan<char> chars)
    {
        foreach (char b in chars)
            Write(b);
    }
    public virtual void WriteLine(ReadOnlySpan<char> chars)
    {
        Write(chars);
        Write('\n');
    }
}