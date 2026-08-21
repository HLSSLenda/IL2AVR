using System;
namespace IL2AVR;

public sealed class WtfException : Exception
{
    public WtfException()
    {
    }
    public WtfException(string message) : base(message)
    {
    }
}
