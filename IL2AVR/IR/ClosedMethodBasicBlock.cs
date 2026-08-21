using Mono.Cecil.Cil;
using System.Collections.Generic;
namespace IL2AVR.IR;

public sealed class ClosedMethodBasicBlock(
    ClosedMethodBlockKind toKind,
    ClosedMethodBasicBlock? to,
    IReadOnlyCollection<IInstruction> instructions)
{
    public ClosedMethodBlockKind ToKind = toKind;
    public ClosedMethodBasicBlock? To = to;
    public IReadOnlyCollection<IInstruction> Instructions = instructions;

}
