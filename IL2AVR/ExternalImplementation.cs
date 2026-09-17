using IL2AVR.IR;
using System.Collections.Generic;
namespace IL2AVR;

/// <summary>Represents a method that's receives its implementation from an instance of the compiler.</summary>
/// <param name="bag">The compiler bag.</param>
/// <param name="isInline"><see langword="true" /> if the method is being evaluated in an inline context.</param>
/// <param name="callGraph">
/// The call graph, with the root method as the first parameter. Note that this call graph is statically
/// computed at compile-time. For non-inlined methods, this list will only have the current method.
/// </param>
/// <param name="instructionStream">The instruction stream.</param>
/// <param name="args">
/// The arguments that this function received. If this function is not inlined, every argument is just a
/// <see cref="Arg" /> reference.
/// </param>
public delegate void ExternalImplementation(
    in CompilerBag bag,
    bool isInline,
    IReadOnlyList<ClosedMethod>? callGraph,
    InstructionStream instructionStream,
    IReadOnlyList<IValue> args
);
