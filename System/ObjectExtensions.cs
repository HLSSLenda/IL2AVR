using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
namespace System;

public static class ObjectExtensions
{
    public static unsafe void Dispose<T>(this T obj) =>
        NativeMemory.Free(Unsafe.AsPointer(ref obj));
}