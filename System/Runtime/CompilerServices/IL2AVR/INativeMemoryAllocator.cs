namespace System.Runtime.CompilerServices.IL2AVR;

public unsafe partial interface INativeMemoryAllocator
{
    public static extern INativeMemoryAllocator Default { [ValueFromCompilerInvocation] get; }

    protected static extern void* HeapBegin { [ValueFromCompilerInvocation] get; }
    protected static extern nint HeapSize { [ValueFromCompilerInvocation] get; }
    protected static void* HeapEnd
    {
        get
        {
            Assembly.Asm(OpCodeId.In, out byte spl, SPL);
            Assembly.Asm(OpCodeId.In, out byte sph, SPH);
            ushort sp = (sph << 8) | spl;
            return (void*)(HeapSize == 0 ? sp : Math.Min((int)Unsafe.Add(HeapBegin, HeapSize), sp));
        }
    }
    void* Alloc(nint size);
    void Free(void* pointer);
}