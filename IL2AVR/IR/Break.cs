using System.Collections.Generic;
namespace IL2AVR.IR;

// Hardware breakpoints aren't supported everywhere! It depends on the processor.
public sealed class Break() : IInstruction
{
    public int Id => 0;
    public IReadOnlyCollection<IValue> Values => [];

    public override string ToString() => "break;";
}
