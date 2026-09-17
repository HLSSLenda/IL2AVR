namespace IL2AVR.IR;

public sealed record Conv(string Label, Instruction? Next, int Position, Instruction? Previous, IValue Value, ClosedType To) : Instruction(InstructionId.Conv, Label, Next, Position, Previous, [Value])
{
    public Conv(string label, IValue value, ClosedType to) : this(label, null, -1, null, value, to)
    {
    }
    public override string ToString() => $"({To})({Value})";
}
