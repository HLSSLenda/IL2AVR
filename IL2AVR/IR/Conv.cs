using System.Collections.Generic;
namespace IL2AVR.IR;

public sealed class Conv(int id, IValue value, ClosedType to) : IInstruction
{
    public int Id => id;
    public readonly ClosedType To = to;
    public IReadOnlyCollection<IValue> Values => [value];
}
