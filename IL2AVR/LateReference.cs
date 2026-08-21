using Mono.Cecil;
using System;
namespace IL2AVR;

public readonly struct LateReference : IEquatable<LateReference>
{
    public readonly string? String;
    public readonly ClosedField? Field;
    public LateReference(string str)
    {
        String = str;
        Field = null;
    }
    public LateReference(ClosedField fld)
    {
        String = null;
        Field = fld;
    }
    public static bool operator ==(LateReference left, LateReference right) => left.Equals(right);
    public static bool operator !=(LateReference left, LateReference right) => !left.Equals(right);
    public override bool Equals(object? obj) => obj is LateReference other && Equals(other);
    public bool Equals(LateReference other) => Field == other.Field && String == other.String;
    public override int GetHashCode() => Field is null ? String!.GetHashCode() : Field.GetHashCode();
}