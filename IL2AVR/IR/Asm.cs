using System.Collections.Generic;

namespace IL2AVR.IR;

public sealed class Asm(int id, OpCode opcode) : IInstruction
{
    public int Id => id;
    public readonly OpCode OpCode = opcode;

    public IReadOnlyCollection<IValue> Values => throw new System.NotImplementedException();

    public override string ToString() => $"asm {OpCode}";
}
