using System;
using System.Collections.Generic;
namespace IL2AVR.IR;

public sealed class InstructionStream
{
    private readonly record struct InsertedInstruction(IInstruction Instruction, string? Label = null);
    private readonly List<InsertedInstruction> _instructions = [];

    public void Add(IInstruction instruction, string? label = null)
    {
        _instructions.Add(new(instruction, label));
    }
}
