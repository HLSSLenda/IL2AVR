using System;
using System.Collections.Generic;

namespace IL2AVR.IR;

public sealed record BrInstruction : IntermediateInstruction
{
    private static IReadOnlyList<IValue> GetValuesFor(IValue? left, IValue? right)
    {
        if (left is null && right is null)
            return [];
        if (left is null)
            return [right];
        if (right is null)
            return [left];
        return [left, right];
    }
    public BrInstruction(IntermediateInstruction target, BranchMode mode = BranchMode.Unconditional, IValue? left = null, IValue? right = null, string? label = null, IntermediateInstruction? next = null, int position = -1, IntermediateInstruction? previous = null) : base(InstructionId.Br, label, next, position, previous, GetValuesFor(left, right))
    {
        if ((mode == BranchMode.Unconditional && (left is not null || right is not null)) ||
                ((mode == BranchMode.True || mode == BranchMode.False) && ((left is null && right is null) || (left is not null && right is not null))) ||
                (left is null || right is null))
            throw new InvalidOperationException($"Invalid conditional declaration! Branch mode: {mode}, left: {left}, right: {right}");
        Mode = mode;
        Target = target;
    }
    public readonly BranchMode Mode;
    public readonly IntermediateInstruction Target;
}