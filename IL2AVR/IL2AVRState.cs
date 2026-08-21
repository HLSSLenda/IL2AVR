using System.Collections.Generic;
namespace IL2AVR;

public record IL2AVRState(
    ClosedMethod Method,
    LocalAllocator Locals,
    Stack<IValue> CILStack);