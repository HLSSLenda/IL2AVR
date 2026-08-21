using System;
using System.Collections.Generic;
namespace IL2AVR;

public class CompilerException(string message) : Exception(message)
{
    public readonly IReadOnlyList<CompilerDiagnostic> Diagnostics;
}
