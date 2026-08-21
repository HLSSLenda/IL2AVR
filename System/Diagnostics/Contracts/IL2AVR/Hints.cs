using System.Diagnostics.Contracts.IL2AVR;
namespace System.Diagnostics.CodeAnalysis.IL2AVR;

public static class Hints
{
    public static bool Likely(bool x) => x;
    public static bool Unlikely(bool x) => x;
    public static T[] ArrayWithSize<T>(T[] array, [ConstantExpected] int length) => array;
    public static ref T ReferenceFrom<T>(ref T reference, DataLocation location) where T : allows ref struct => ref reference;
    public static ref readonly T ReadonlyReferenceFrom<T>(ref readonly T reference, DataLocation location) where T : allows ref struct => ref reference;
    public static nint PointerFrom(nint ptr, DataLocation location) => ptr; 
    public static nuint PointerFrom(nuint ptr, DataLocation location) => ptr;
    public static unsafe void* PointerFrom(void* ptr, DataLocation location) => ptr;
#pragma warning disable CS8500
    public static unsafe T* PointerFrom<T>(T* ptr, DataLocation location) => ptr;
#pragma warning restore CS8500
}
