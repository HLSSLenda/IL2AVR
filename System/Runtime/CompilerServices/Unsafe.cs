namespace System.Runtime.CompilerServices;

/// <summary>
/// The official docs is at https://learn.microsoft.com/en-us/dotnet/api/system.runtime.compilerservices.unsafe.
/// 
/// The default code generation for this class is documented in README.md.
/// While the default code generation can be disabled in the <c>Compiler</c> class, it's not recommended to do so
/// because <see cref="Unsafe" /> is used in a lot of places.
/// 
/// "With great powers, comes great responsibility." ~ <c>sudo</c>.
/// </summary>
[Intrinsic]
public static class Unsafe
{
    [Intrinsic]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static extern ref T Add<T>(ref T obj, int offset) where T : allows ref struct;
    [Intrinsic]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static extern ref T Add<T>(ref T obj, nint offset) where T : allows ref struct;
    [Intrinsic]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static extern ref T Add<T>(ref T obj, nuint offset) where T : allows ref struct;
    [Intrinsic]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static extern ref T AddByteOffset<T>(ref T obj, nint offset) where T : allows ref struct;
    [Intrinsic]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static extern ref T AddByteOffset<T>(ref T obj, nuint offset) where T : allows ref struct;
    [Intrinsic]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static extern bool AreSame<T>(ref readonly T lhs, ref readonly T rhs) where T : allows ref struct;
    [Intrinsic]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static extern ref TTo As<TFrom, TTo>(ref TFrom source)
        where TFrom : allows ref struct
        where TTo : allows ref struct;
    [Intrinsic]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static extern ref T As<T>(object? obj) where T : allows ref struct;
    [Intrinsic]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static extern unsafe void* AsPointer<T>(ref T source) where T : allows ref struct;
    [Intrinsic]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static extern ref T AsRef<T>(scoped ref readonly T reference) where T : allows ref struct;
    [Intrinsic]
    public static extern void CopyBlockUnaligned<T>(ref T destination, ref readonly T source, uint byteCount);
    [Intrinsic]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static extern bool IsNullRef<T>(ref readonly T reference) where T : allows ref struct;
    [Intrinsic]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static extern int SizeOf<T>() where T : allows ref struct;
    [Intrinsic]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static extern void SkipInit<T>(out T value) where T : allows ref struct;
    [Intrinsic]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static extern ref T Subtract<T>(ref T obj, int offset) where T : allows ref struct;
    [Intrinsic]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static extern ref T Subtract<T>(ref T obj, nint offset) where T : allows ref struct;
    [Intrinsic]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static extern ref T Subtract<T>(ref T obj, nuint offset) where T : allows ref struct;
    [Intrinsic]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static extern ref T SubtractByteOffset<T>(ref T obj, nint offset) where T : allows ref struct;
    [Intrinsic]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static extern ref T SubtractByteOffset<T>(ref T obj, nuint offset) where T : allows ref struct;
#pragma warning disable CS8500  // unsafe api therefore fuck safety
    [Intrinsic]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static unsafe void* Add(void* ptr, nint offset) => (byte*)ptr + offset;
    [Intrinsic]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static unsafe ref T AsRef<T>(void* pointer) where T : allows ref struct => ref *(T*)pointer;
    [Intrinsic]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static unsafe void Copy<T>(ref T destination, void* source) => destination = *(T*)source;
    [Intrinsic]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static unsafe void Copy<T>(void* destination, ref readonly T source) => *(T*)destination = source;
    [Intrinsic]
    public static unsafe void CopyBlockUnaligned(void* destination, void* source, uint byteCount)
    {
        byte* sp = (byte*)source;
        byte* dp = (byte*)destination;
        for (uint i = 0; i < byteCount; ++i)
            dp[i] = sp[i];
    }
    [Intrinsic]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static unsafe ref T NullRef<T>() where T : allows ref struct => ref *(T*)null;
    [Intrinsic]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static unsafe T Read<T>(void* ptr) where T : allows ref struct => *(T*)ptr;
    [Intrinsic]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static unsafe void* Subtract(void* ptr, nint offset) => (byte*)ptr - offset;
    [Intrinsic]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static unsafe void Write<T>(void* ptr, T value) where T : allows ref struct => *(T*)ptr = value;
#pragma warning restore CS8500
}