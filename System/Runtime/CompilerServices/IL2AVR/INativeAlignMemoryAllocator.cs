namespace System.Runtime.CompilerServices.IL2AVR;

public unsafe interface INativeAlignMemoryAllocator : INativeMemoryAllocator
{
    void* AllocAligned(nint size, ushort align);
}