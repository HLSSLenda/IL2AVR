using System.Collections.Generic;
using IL2AVR.IR;
namespace IL2AVR;

public readonly record struct CompiledMethod(ushort Address, ClosedMethod? Method, IList<OpCode> OpCodes);