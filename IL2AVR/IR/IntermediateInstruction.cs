using System.Collections.Generic;
namespace IL2AVR.IR;

/// <summary>Represents an instruction of the Intermediate Representation (IR) from the compiler.</summary>
/// <param name="Id">The instruction identifier.</param>
/// <param name="Label">The label that this instruction will have.</param>
/// <param name="Next">The instruction that comes after this instruction.</param>
/// <param name="Position">The position of this instruction.</param>
/// <param name="Previous">The instruction that comes before this instruction.</param>
/// <param name="Values">The value list that this instruction will operate upon.</param>
public record IntermediateInstruction(
    InstructionId Id,
    string? Label = null,
    IntermediateInstruction? Next = null,
    int Position = -1,
    IntermediateInstruction? Previous = null,
    IReadOnlyList<IValue>? Values = null)
{
    public IntermediateInstruction? Next { get; internal set; } = Next;
    public int Position { get; internal set; } = Position;
    public IntermediateInstruction? Previous { get; internal set; } = Previous;
    public readonly IReadOnlyList<IValue> Values = Values ?? [];
}
