namespace IL2AVR.IR;

public sealed record ConvInstruction(IValue Value, ClosedType To, string? Label = null, IntermediateInstruction? Next = null, int Position = -1, IntermediateInstruction? Previous = null) : IntermediateInstruction(InstructionId.Conv, Label, Next, Position, Previous, [Value])
{
    public override string ToString() => $"({To})({Value})";
}