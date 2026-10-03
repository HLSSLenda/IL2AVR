using System.Collections.Generic;
namespace IL2AVR.IR;

public sealed record SwitchInstruction(
    IReadOnlyList<IntermediateInstruction> Targets,
    string? Label = null,
    IntermediateInstruction? Next = null,
    int Position = -1,
    IntermediateInstruction? Previous = null
) : IntermediateInstruction(InstructionId.Switch, Label, Next, Position, Previous, []);