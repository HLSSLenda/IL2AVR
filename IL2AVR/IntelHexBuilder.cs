using System;
using System.IO;
using System.Linq;
using System.Text;
namespace IL2AVR;

public readonly struct IntelHexCompiler : IEmitter
{
    private const string EOF = ":00000001FF";
    public static void ToIntelHex(AVRCompiler machine) => new StringBuilder(":", 12 + Data.Count * 2)
        .Append(Size.ToString("X2"))
        .Append(Address.ToString("X4"))
        .Append("00")
        .AppendJoin(string.Empty, Data.Select(b => b.ToString("X2")))
        .Append((1 + ~(Size + Address + Data.Sum(x => x))).ToString("X2"))
        .AppendLine()
        .ToString();

    public void Compile(AVRCompiler machine, Stream stream)
    {

    }
}