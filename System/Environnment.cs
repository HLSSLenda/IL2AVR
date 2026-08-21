using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.Intrinsics.AVR;
namespace System;

public static class Environnment
{
    [DoesNotReturn]
    public static void Exit(int code)
    {
        Interrupts.Disable();
        while (true)
        {
        }
    }
    [DoesNotReturn]
    public static void FailFast(string message)
    {
        Interrupts.Disable();
        Console.Error?.WriteLine(message);
        Debugger.Break();
        while (true)
        {
        }
    }
    public const string NewLine = "\n";
    public const int ProcessorCount = 1;
}