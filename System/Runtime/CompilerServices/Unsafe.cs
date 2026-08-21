namespace System.Runtime.CompilerServices;

public static class Unsafe
{
    [Intrinsic]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static extern ref T Add<T>(ref T obj, int offset) where T : allows ref struct;
    [Intrinsic]
    public static extern ref T Add<T>(ref T obj, nint offset) where T : allows ref struct;
    [Intrinsic]
    public static extern ref T Add<T>(ref T obj, nuint offset) where T : allows ref struct;
    [Intrinsic]
    public static extern ref T AddByteOffset<T>(ref T obj, nint offset) where T : allows ref struct;
    [Intrinsic]
    public static extern ref T AddByteOffset<T>(ref T obj, nuint offset) where T : allows ref struct;
    [Intrinsic]
    public static extern bool AreSame<T>(ref readonly T lhs, ref readonly T rhs) where T : allows ref struct;
    [Intrinsic]
    public static extern ref TTo As<TFrom, TTo>(ref TFrom source)
        where TFrom : allows ref struct
        where TTo : allows ref struct;
    [Intrinsic]
    public static extern ref T As<T>(object? obj) where T : allows ref struct;
    [Intrinsic]
    public static extern unsafe void* AsPointer<T>(ref T source) where T : allows ref struct;
    [Intrinsic]
    public static extern ref T AsRef<T>(scoped ref readonly T reference) where T : allows ref struct;
    [Intrinsic]
    public static extern bool IsNullRef<T>(ref readonly T reference) where T : allows ref struct;
    [Intrinsic]
    public static extern void SkipInit<T>(out T value) where T : allows ref struct;
    [Intrinsic]
    public static extern ref T Subtract<T>(ref T obj, int offset) where T : allows ref struct;
    [Intrinsic]
    public static extern ref T Subtract<T>(ref T obj, nint offset) where T : allows ref struct;
    [Intrinsic]
    public static extern ref T Subtract<T>(ref T obj, nuint offset) where T : allows ref struct;
    [Intrinsic]
    public static extern ref T SubtractByteOffset<T>(ref T obj, nint offset) where T : allows ref struct;
    [Intrinsic]
    public static extern ref T SubtractByteOffset<T>(ref T obj, nuint offset) where T : allows ref struct;
#pragma warning disable CS8500  // unsafe api therefore fuck safety
    [Intrinsic]
    public static unsafe void* Add(void* ptr, nint offset) => (byte*)ptr + offset;
    [Intrinsic]
    public static unsafe ref T AsRef<T>(void* pointer) where T : allows ref struct => ref *(T*)pointer;
    [Intrinsic]
    public static unsafe ref T NullRef<T>() where T : allows ref struct => ref *(T*)null;
    [Intrinsic]
    public static unsafe T Read<T>(void* ptr) where T : allows ref struct => *(T*)ptr;
    [Intrinsic]
    public static unsafe int SizeOf<T>() where T : allows ref struct => sizeof(T);
    [Intrinsic]
    public static unsafe void* Subtract(void* ptr, nint offset) => (byte*)ptr - offset;
    [Intrinsic]
    public static unsafe void Write<T>(void* ptr, T value) where T : allows ref struct => *(T*)ptr = value;
#pragma warning restore CS8500
}