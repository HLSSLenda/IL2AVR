using System.Runtime.CompilerServices.IL2AVR;
namespace System.Runtime.InteropServices;

public sealed class DefaultAllocator : INativeMemoryAllocator
{
    private unsafe ref struct FreeMemoryBlock(FreeMemoryBlock* nextAddress, ushort size)
    {
        public const nuint MEMORY_CONTENT_OFFSET = sizeof(ushort) * 2;
        public FreeMemoryBlock* NextBlockAddress = nextAddress;
        public ushort Size = size;
    }
    public unsafe void* Alloc(nint size)
    {
        FreeMemoryBlock* freelistBlock = (FreeMemoryBlock*)INativeMemoryAllocator.HeapBegin;
        FreeMemoryBlock* previous = null;
        while (freelistBlock->NextBlockAddress != null)
        {
            if (size <= freelistBlock->Size)
            {
                freelistBlock->NextBlockAddress = (FreeMemoryBlock*)((nuint)freelistBlock + FreeMemoryBlock.MEMORY_CONTENT_OFFSET + (nuint)size);
                freelistBlock->Size = (ushort)size;
                return (void*)((byte*)freelistBlock + FreeMemoryBlock.MEMORY_CONTENT_OFFSET);
            }
            previous = freelistBlock;
            freelistBlock = freelistBlock->NextBlockAddress;
        }
        freelistBlock->NextBlockAddress = (FreeMemoryBlock*)((nuint)freelistBlock + FreeMemoryBlock.MEMORY_CONTENT_OFFSET + (nuint)size);
        freelistBlock->Size = (ushort)size;
        return (void*)((byte*)freelistBlock + FreeMemoryBlock.MEMORY_CONTENT_OFFSET);
    }

    public unsafe void Free(void* pointer)
    {
        FreeMemoryBlock* block = (FreeMemoryBlock*)INativeMemoryAllocator.HeapBegin;
        
    }
}