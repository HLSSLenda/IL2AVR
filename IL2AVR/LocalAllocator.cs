using Mono.Cecil;
using System.Collections.Generic;
namespace IL2AVR;

public sealed class LocalAllocator(IProcessor processor, IReadOnlyList<ClosedParameter?> parameters)
{
    private ushort totalRegs = 0, totalStack = 0;
    private readonly List<Local> mem = [];
    public readonly IReadOnlyDictionary<ClosedParameter, Local> Arguments = new Dictionary<ClosedParameter, Local>();
    public Local Add(TypeSystem typeSystem, ClosedType type, MemoryLocation? location = null, ushort size = 0)
    {
        if (size == 0)
            size = type.Size;
        Local memory;
        // more than 8 bytes will go to sram (stack or heap) no matter what
        if (totalRegs + size < processor.RegisterRange.End.Value - processor.RegisterRange.Start.Value &&
            location is MemoryLocation l ? l == MemoryLocation.Register : size <= sizeof(ulong))
        {
            mem.Add(memory = new(type, MemoryLocation.Register, totalRegs, size));
            totalRegs += size;
        }
        else
        {        
            mem.Add(memory = new(type, MemoryLocation.SRAM, totalStack, size));
            totalStack += size;
        }
        return memory;
    }
}