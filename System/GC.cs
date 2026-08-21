using System.Runtime.CompilerServices;
using System.Runtime.CompilerServices.IL2AVR;
using System.Runtime.InteropServices;
namespace System;

public static class GC
{
    public static T?[]? AllocateArray<T>([TypeAs(typeof(ushort))] int length)
    {
        T?[]? array = AllocateUninitializedArray<T>(length);
        if (array is not null)
            for (ushort i = 0; i < length; ++i)
                array[i] = default;
        return array;
    }
    public static unsafe T?[]? AllocateUninitializedArray<T>([TypeAs(typeof(ushort))] int length)
    {
        void* memory = NativeMemory.Alloc(sizeof(ushort) + length * Unsafe.SizeOf<T>());
        if (memory is null)
            return null;
        Unsafe.AsRef<ushort>(memory) = (ushort)length;
        return Unsafe.AsRef<T[]>(memory);
    }
}
