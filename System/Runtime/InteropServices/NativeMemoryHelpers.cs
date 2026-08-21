using System.Runtime.CompilerServices;
namespace System.Runtime.InteropServices;

public static class NativeMemoryHelpers
{
    public unsafe ref struct UniquePointer<T>(ref T reference, delegate* unmanaged<ref T, void> free) : IDisposable
        where T : allows ref struct
    {
        private void* reference = Unsafe.AsPointer(ref reference);
        public readonly ref readonly T Value => ref Unsafe.AsRef<T>(reference);
        public void Dispose()
        {
            if (reference is not null)
            {
                free(ref Unsafe.AsRef<T>(reference));
                reference = null;
            }
        }
    }
}