using System.Collections.Generic;
using Mono.Cecil;

namespace IL2AVR;

public sealed class ClosedField
{
    public readonly FieldDefinition UnderlyingField;
    public readonly ClosedType DeclaringType;
    public readonly IClosedType FieldType;

    internal ClosedField(FieldReference underlyingField, IReadOnlyDictionary<GenericParameter, IClosedType> declaringTypeGenericArguments)
    {
        UnderlyingField = underlyingField.Resolve();
        DeclaringType = (ClosedType)underlyingField.DeclaringType.Close(declaringTypeGenericArguments);
        FieldType = underlyingField.FieldType.Close(declaringTypeGenericArguments);
        Offset = (ushort)UnderlyingField.Offset;
    }

    internal ClosedField(FieldReference underlyingField, ClosedType declarer, IClosedType fieldType, ushort offset)
    {
        UnderlyingField = underlyingField.Resolve();
        DeclaringType = declarer;
        FieldType = fieldType;
        Offset = offset;
    }

    public readonly ushort Offset;
    
}
