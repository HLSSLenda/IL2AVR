using System.Runtime.CompilerServices;
namespace System;

/// <summary>
/// The official docs is at https://learn.microsoft.com/en-us/dotnet/api/system.object.
/// </summary>
/// <remarks>It doesn't declares <c>GetHashCode</c> because this BCL lacks standard collections.</remarks>
public class Object
{
    /// <summary>
    /// The official docs is at https://learn.microsoft.com/en-us/dotnet/api/system.object.equals#system-object-equals(system-object-system-object).
    /// </summary>
    public static bool Equals(object? lhs, object? rhs) => lhs is null ? rhs is null : lhs.Equals(rhs);
    /// <summary>
    /// The official docs is at https://learn.microsoft.com/en-us/dotnet/api/system.object.equals#system-object-equals(system-object).
    /// </summary>
    public virtual bool Equals(object? other) => ReferenceEquals(this, other);
    /// <summary>
    /// The official docs is at https://learn.microsoft.com/en-us/dotnet/api/system.object.gettype.
    /// </summary>
    public unsafe Type GetType() =>
        new(Unsafe.Read<ushort>(Unsafe.AsPointer(ref Unsafe.Add(ref Unsafe.As<ushort>(this), 1))));
    /// <summary>
    /// The official docs is at https://learn.microsoft.com/en-us/dotnet/api/system.object.referenceequals.
    /// </summary>
    public static bool ReferenceEquals(object? lhs, object? rhs) => Unsafe.AreSame(in lhs, in rhs);
    /// <summary>
    /// The official docs is at https://learn.microsoft.com/en-us/dotnet/api/system.object.tostring.
    /// </summary>
    public virtual string? ToString() => null;
}