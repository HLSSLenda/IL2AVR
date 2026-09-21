using System;
using System.Collections.Generic;
namespace IL2AVR.IR;

public sealed class CompileTimeClosedTypeInstance : ICompileTimeValue
{
    private static void CheckType(ClosedType type)
    {
        if (!type.Kind.IsAggregateKind())
            throw new InvalidOperationException("Cannot construct an instance of a non-aggregate type!");
    }
    public CompileTimeClosedTypeInstance(ClosedType type)
    {
        CheckType(type);
        if (!type.Kind.IsReferenceKind())
            throw new InvalidOperationException("A constructor-less instance of a type instance can only be used on value types.");
        Type = type;
    }
    public CompileTimeClosedTypeInstance(ClosedMethod ctor, params IReadOnlyList<ICompileTimeValue> constructorArguments)
    {
        CheckType(ctor.DeclaringType);
        if (!ctor.IsConstructor)
            throw new InvalidOperationException("A constructor method is necessary.");
        ClosedType type = (ClosedType)(Type = ctor.DeclaringType);
        fields = new(type.Fields.Count);
        foreach (ClosedField fld in type.Fields)
            fields[fld] = fld.FieldType.DefaultCompileTimeValue();            
        // TODO: ctor resolution and evaluation
        // HACK: CompileTimeClosedTypeInstance and ClosedMethod are tightly coupled somehow
        CompileTimeMethodEvaluator s = new(ctor, constructorArguments);
        s.Run();
    }
    private readonly Dictionary<ClosedField, ICompileTimeValue> fields;
    public IReadOnlyDictionary<ClosedField, ICompileTimeValue> Fields => fields;

    /// <summary>Sets a field value to a specified value.</summary>
    /// 
    /// <remarks>This method doesn't honor the <see cref="Mono.Cecil.FieldDefinition.IsInitOnly" /> property of fields.</remarks>
    public void SetField(ClosedField fld, ICompileTimeValue value)
    {
        if (!value.Type.CanConvertTo(fld.FieldType))
            throw new InvalidOperationException($"Type {value.Type} cannot be converted to {fld.FieldType}.");
        fields[fld] = value;
    }
    public readonly ClosedType Type;
    IClosedType IValue.Type => Type;
    object ICompileTimeValue.Value => this;
}
