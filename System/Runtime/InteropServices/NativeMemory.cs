using System.Runtime.CompilerServices;
using System.Runtime.CompilerServices.IL2AVR;
namespace System.Runtime.InteropServices;

public static unsafe class NativeMemory
{
    public static void* Alloc(nint size) => INativeMemoryAllocator.Default.Alloc(size);
    public static void Free(void* pointer) => INativeMemoryAllocator.Default.Free(pointer);
}
