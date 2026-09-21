namespace IL2AVR;

// As bad design as it is to depend on enum ordering, I don't want to == every single time!
// So please, maintain the strict ordering by these #regions here.

public enum ClosedTypeKind
{
#region Aggregates
    AggregateReference,
    AggregateValue,
#endregion
#region Leaf
    Array,
    ByReference,
    Pointer,
    Sentinel, // grey area
#endregion
#region Basic types
    TypedReference,
    Void,
#endregion
#region Basic signed types
    Int16,
    Int32,
    Int64,
    IntPtr,
    SByte,
#endregion
#region Basic unsigned types
    Boolean,
    Byte,
    Char,
    UInt16,
    UInt32,
    UInt64,
    UIntPtr,
#endregion
#region Basic floating-point types
    Double,
    Single,
#endregion
#region Basic reference types
    Object,
    String,
#endregion
}
