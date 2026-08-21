using System.Collections.Generic;
namespace IL2AVR.IR;

public sealed class Nop : IInstruction
{
    public Boxed<int> Id => 0;
    public IReadOnlyCollection<IValue> Values => [];

    public override string ToString() => "nop;";
}
