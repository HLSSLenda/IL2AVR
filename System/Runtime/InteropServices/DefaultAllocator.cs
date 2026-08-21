using System.Runtime.CompilerServices.IL2AVR;
namespace System.Runtime.InteropServices;

public sealed class DefaultAllocator : INativeMemoryAllocator
{
    private struct MemoryBlock
    {
        public ushort Size;
        public nuint Address;
    }
    public unsafe void* Alloc(nint size)
    {
        throw new System.NotImplementedException();
    }

    public unsafe void Free(void* pointer)
    {
        throw new System.NotImplementedException();
    }
}