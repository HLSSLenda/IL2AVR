using System;
using System.IO;
namespace Arduino;

public sealed unsafe partial class HardwareSerial : Stream
{
    public HardwareSerial(int baud = 9600)
    {
        
    }
    public override bool CanRead => true;
    public override bool CanSeek => false;
    public override bool CanWrite => true;
    public override int Length => throw new System.NotImplementedException();
    public override int Position => throw new System.NotImplementedException();
    public override int Read(Span<byte> bytes)
    {
        
    }
    public override void Write(ReadOnlySpan<byte> bytes)
    {
        
    }
}