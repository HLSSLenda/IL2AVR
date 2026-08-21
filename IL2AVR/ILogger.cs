using System;
namespace IL2AVR;

/// <summary>Allows implementations to log messages with different levels of priority.</summary>
public interface ILogger
{
    /// <summary></summary>
    /// <param name="fmt">The base format string with a format that <see cref="string.Format(string, ReadOnlySpan{object?})" /> understands.</param>
    /// <param name="args">The operands to format the string.</param>
    void Debug(ReadOnlySpan<char> fmt, params scoped ReadOnlySpan<object> args);
    /// <summary></summary>
    /// <param name="fmt">The base format string with a format that <see cref="string.Format(string, ReadOnlySpan{object?})" /> understands.</param>
    /// <param name="args">The operands to format the string.</param>
    void Error(ReadOnlySpan<char> fmt, params scoped ReadOnlySpan<object> args);
    /// <summary></summary>
    /// <param name="fmt">The base format string with a format that <see cref="string.Format(string, ReadOnlySpan{object?})" /> understands.</param>
    /// <param name="args">The operands to format the string.</param>
    void Fatal(ReadOnlySpan<char> fmt, params scoped ReadOnlySpan<object> args);
    /// <summary></summary>
    /// <param name="fmt">The base format string with a format that <see cref="string.Format(string, ReadOnlySpan{object?})" /> understands.</param>
    /// <param name="args">The operands to format the string.</param>
    void Info(ReadOnlySpan<char> fmt, params scoped ReadOnlySpan<object> args);
    /// <summary></summary>
    /// <param name="fmt">The base format string with a format that <see cref="string.Format(string, ReadOnlySpan{object?})" /> understands.</param>
    /// <param name="args">The operands to format the string.</param>
    void Warning(ReadOnlySpan<char> fmt, params scoped ReadOnlySpan<object> args);
}
