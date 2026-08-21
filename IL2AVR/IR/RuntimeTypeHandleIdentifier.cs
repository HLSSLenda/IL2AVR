using System.Collections.Generic;
namespace IL2AVR.IR;

public sealed class RuntimeTypeHandleIdentifier : IValue
{
    private static readonly Dictionary<ClosedType, RuntimeTypeHandleIdentifier> _typeIds = [];
    public readonly ushort Id;
    public readonly ClosedType HandleType;
    private readonly ClosedType _type;
    ClosedType IValue.Type => _type;

    private RuntimeTypeHandleIdentifier(ClosedTypeSystem typeSystem, ushort id, ClosedType type)
    {
        Id = id;
        HandleType = type;
        _type = typeSystem.UInt16;
    }
    public static RuntimeTypeHandleIdentifier Get(ClosedTypeSystem typeSystem, ClosedType type)
    {
        if (!_typeIds.TryGetValue(type, out RuntimeTypeHandleIdentifier? typeHandle))
            _typeIds[type] = typeHandle = new(typeSystem, (ushort)_typeIds.Count, type);
        return typeHandle;
    }
}
