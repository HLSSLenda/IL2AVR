using System;
using System.Collections.Generic;
using System.Linq;
namespace IL2AVR.IR;

/// <summary>
/// A class that allows you to stream <see cref="IntermediateInstruction" /> in order to form a
/// <see cref="IReadOnlyCollection{T}" /> of <see cref="ClosedMethodBasicBlock" />s.
/// </summary>
/// <remarks>This class's constructors always copy instructions!</remarks>
public sealed class InstructionStream
{
    private readonly HashSet<string> _usedLabels;
    private readonly List<IntermediateInstruction> _instructions;
    private int _basicBlockDelimiterCounts = 1;

    public InstructionStream()
    {
        _instructions = [];
        _usedLabels = [];
    }
    public InstructionStream(IReadOnlyList<IntermediateInstruction> instructions)
    {
        _instructions = [.. instructions];
        _usedLabels = [.. instructions.Select(static instruction => instruction.Label)];
    }
    public InstructionStream(IReadOnlyList<ClosedMethodBasicBlock> blocks)
    {
        _instructions = [.. blocks.SelectMany(static block => block.Instructions)];
        _usedLabels = [.. _instructions.Select(static instruction => instruction.Label)];
    }
    public InstructionStream(int capacity)
    {
        _instructions = new(capacity);
        _usedLabels = [];
    }
    
    public IntermediateInstruction Add(IntermediateInstruction instruction)
    {
        if (instruction.Label is not null && _usedLabels.Contains(instruction.Label))
            throw new InvalidOperationException();
        if (instruction.Position < 0)
            instruction.Position = _instructions.Count;
        _instructions.Insert(instruction.Position, instruction);
        for (int i = instruction.Position + 1; i < _instructions.Count; ++i)
            ++_instructions[i].Position;
        switch (instruction.Id)
        {
            case InstructionId.Br:
                _basicBlockDelimiterCounts += 2;
                break;
            case InstructionId.Ret:
                ++_basicBlockDelimiterCounts;   // ret always generate one more block
                break;
        }
        return instruction;
    }

    public void Clear()
    {
        _instructions.Clear();
        _usedLabels.Clear();
        _basicBlockDelimiterCounts = 1;
    }

    public void Remove(IntermediateInstruction instruction)
    {
        if (instruction.Position < _instructions.Count)
        {
            _instructions.RemoveAt(instruction.Position);
            for (int i = instruction.Position; i < _instructions.Count; ++i)
                --_instructions[i].Position;
        }
    }

    public IReadOnlyList<ClosedMethodBasicBlock> Emit()
    {
        ClosedMethodBasicBlock[] blocks = new ClosedMethodBasicBlock[_basicBlockDelimiterCounts];
        List<IntermediateInstruction> pendingBlocks = [];
        int currentBlockIndex = 0;
        IntermediateInstruction? prev = null;
        foreach (IntermediateInstruction instruction in _instructions)
        {
            switch (instruction.Id)
            {
                case InstructionId.Br:
                    pendingBlocks.Add(((BrInstruction)instruction).Target);
                    blocks[currentBlockIndex++] = new(ClosedMethodBlockKind.Branch, null, currentBlockInstructions);
                    break;
                case InstructionId.Ret:
                    blocks[currentBlockIndex++] = new(ClosedMethodBlockKind.Return, null, currentBlockInstructions);
                    break;
            }
            instruction.Previous = prev;
            prev?.Next = instruction;
            prev = instruction;
        }
        _instructions[^ - 1].Next = null;
        return blocks;
    }
}
