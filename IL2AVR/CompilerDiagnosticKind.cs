using System;
namespace IL2AVR;

[Flags]
public enum CompilerDiagnosticKind
{
    Fatal = 1,
    Error = 2,
    Warning = 4,
    Info = 8,
    Debug = 16
}
