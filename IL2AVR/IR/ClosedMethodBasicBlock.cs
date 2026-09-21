using System.Collections.Generic;
namespace IL2AVR.IR;

public sealed class ClosedMethodBasicBlock(
    ClosedMethodBlockKind toKind,
    ClosedMethodBasicBlock? to,
    IReadOnlyList<IntermediateInstruction> instructions)
{
    public ClosedMethodBlockKind ToKind = toKind;
    public ClosedMethodBasicBlock? To = to;
    public IReadOnlyList<IntermediateInstruction> Instructions = instructions;
}
