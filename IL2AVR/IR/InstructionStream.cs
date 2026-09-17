using System;
using System.Collections.Generic;
namespace IL2AVR.IR;

public sealed class InstructionStream
{
    public sealed class InsertedInstruction(InstructionId id, int position, string? label, IReadOnlyCollection<IValue> values)
    {
        public readonly InstructionId Id = id;
        public string? Label = label;
        public int Position { get; internal set; } = position;
        public IReadOnlyCollection<IValue> Values = values ?? [];
    }
    private readonly HashSet<string> usedLabels = [];
    private readonly List<InsertedInstruction> _instructions = [];
    private int _basicBlockDelimiterCounts = 1;

    public InstructionStream(ClosedMethodBasicBlock)

    public InsertedInstruction Add(InstructionId id, string? label = null, IReadOnlyCollection<IValue>? values = null)
    {
        if (label is not null && usedLabels.Contains(label))
            throw new InvalidOperationException();
        InsertedInstruction instruction = new(id, _instructions.Count, label, values);
        _instructions.Add(instruction);
        if (id == InstructionId.Ret)
            ++_basicBlockDelimiterCounts;
        return instruction;
    }

    public void Remove(InsertedInstruction instruction)
    {
        if (instruction.Position < _instructions.Count)
        {
            _instructions.RemoveAt(instruction.Position);
            for (int i = instruction.Position; i < _instructions.Count; ++i)
                --_instructions[i].Position;
        }
    }

    public IReadOnlyCollection<ClosedMethodBasicBlock> Emit()
    {
        ClosedMethodBasicBlock[] blocks = new ClosedMethodBasicBlock[_basicBlockDelimiterCounts];
        int currentBlockIndex = 0;
        List<Instruction> instructions = [];
        foreach (InsertedInstruction instruction in _instructions)
        {
            switch (instruction.Id)
            {
                case InstructionId.Asm:
                    instructions.Add(new Asm())
                    break;
            }
        }
        return blocks;
    }
}
