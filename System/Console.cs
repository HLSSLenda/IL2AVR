using System.IO;
namespace System;

public static class Console
{
#pragma warning disable CA2211  // IL2AVR is still at its beginning phases..
    public static TextReader? In = null;
    public static TextWriter? Out = null;
    public static TextWriter? Error = null;
#pragma warning restore CA2211
    public static string? ReadLine(Span<byte> buffer) => In?.ReadLine(buffer);
    public static void Write(ReadOnlySpan<char> text) => Out?.Write(text);
    public static void WriteLine(ReadOnlySpan<char> text) => Out?.WriteLine(text);
}