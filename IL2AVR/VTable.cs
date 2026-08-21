using System.Collections.Generic;
namespace IL2AVR;

public readonly record struct VTable(ushort Id)
{
    public readonly IList<ClosedMethod> Methods = [];
}
