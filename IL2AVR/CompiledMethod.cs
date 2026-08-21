using System.Collections.Generic;
namespace IL2AVR;

public readonly record struct CompiledMethod(ushort Address, ClosedMethod? Method, IList<OpCode> OpCodes);