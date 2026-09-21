using System.Collections.Generic;

namespace IL2AVR.IR;

public sealed record CallInstruction(ClosedMethod Target, CallMode Mode = CallMode.Static, string? Label = null, IntermediateInstruction? Next = null, int Position = -1, IntermediateInstruction? Previous = null, IReadOnlyList<IValue>? Values = null): IntermediateInstruction(InstructionId.Call, Label, Next, Position, Previous, Values);
