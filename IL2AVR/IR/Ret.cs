using System.Collections.Generic;
namespace IL2AVR.IR;

public sealed class Ret(IValue? value = null) : IInstruction
{
    public Boxed<int> Id => -1;
    public readonly IValue? Value = value;
    public IReadOnlyCollection<IValue> Values => [Value!];
}
