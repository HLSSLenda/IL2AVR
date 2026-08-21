using IL2AVR.IR;
using System.Collections.Generic;
namespace IL2AVR;

/// <summary>Represents a method that's receives its implementation from an instance of the compiler.</summary>
public delegate void ExternalImplementation(
    in CompilerBag bag,
    bool isInline,
    IReadOnlyList<ClosedMethod>? callGraph,
    IList<IInstruction> instructionStream,
    IReadOnlyList<IValue> args
);
