using System.Collections.Generic;
namespace IL2AVR.IR;

public record Instruction(
    InstructionId Id,
    string? Label,
    Instruction? Next,
    int Position,
    Instruction? Previous,
    IReadOnlyList<IValue> Values)
{
    public Instruction(InstructionId id, string? label, IReadOnlyList<IValue> values) :
        this(id, label, null, -1, null, values)
    {
    }
}