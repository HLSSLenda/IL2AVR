using System;
using System.IO;

namespace IL2AVR;

public sealed class TextWriterLogger(TextWriter writer, bool warningAsError = false) : ILogger
{
    private void Write(ReadOnlySpan<char> prefix, ReadOnlySpan<char> fmt, ReadOnlySpan<object> args)
    {
	if (!prefix.IsEmpty)
	{
            writer.Write(prefix);
            writer.Write(' ');
	}
	writer.WriteLine(fmt.ToString(), args);
    }
    public void Debug(ReadOnlySpan<char> fmt, params scoped ReadOnlySpan<object> args) => Write("(DEBUG)", fmt, args);

    public void Error(ReadOnlySpan<char> fmt, params scoped ReadOnlySpan<object> args) => Write("(ERROR)", fmt, args);

    public void Fatal(ReadOnlySpan<char> fmt, params scoped ReadOnlySpan<object> args) => Write("(FATAL)", fmt, args);

    public void Info(ReadOnlySpan<char> fmt, params scoped ReadOnlySpan<object> args) => Write([], fmt, args);

    public void Warning(ReadOnlySpan<char> fmt, params scoped ReadOnlySpan<object> args) => Write(warningAsError ? "(ERROR)" : "(WARNING)", fmt, args);
}
