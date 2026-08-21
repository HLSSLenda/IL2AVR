namespace System.IO;

internal sealed class NullStream : Stream
{
    public override bool CanRead => true;
    public override bool CanWrite => true;
    public override bool CanSeek => true;
    public override int Length => 0;
    public override int Position => 0;
    public override int Read(Span<byte> bytes) => 0;
    public override void Write(ReadOnlySpan<byte> bytes)
    {
    }
}