using System;
using System.Collections.Generic;
namespace IL2AVR.IR;

public sealed record CompileTimeObjectInstance : ObjectInstance, ICompileTimeValue
{
    private static void CheckType(ClosedType type)
    {
        if (!type.Kind.IsAggregateKind())
            throw new InvalidOperationException("Cannot construct an instance of a non-aggregate type!");
    }
    public CompileTimeObjectInstance(ClosedType type) : base(type)
    {
        CheckType(type);
        if (!type.Kind.IsReferenceKind())
            throw new InvalidOperationException("A constructor-less instance of a type instance can only be used on value types.");
        Type = type;
    }
    public CompileTimeObjectInstance(in CompilerBag bag, ClosedMethod ctor, params IReadOnlyList<ICompileTimeValue> constructorArguments) : base(ctor.DeclaringType)
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

        CompileTimeMethodEvaluator method = new(ctor, [this, ..constructorArguments], bag.Options.Processor.TotalMemory());
        method.Run();
    }
    private readonly Dictionary<ClosedField, ICompileTimeValue> fields;
    public IReadOnlyDictionary<ClosedField, ICompileTimeValue> Fields => fields;

    /// <summary>Sets a field value to a specified value.</summary>
    /// <param name="fld">The field to set.</param>
    /// <param name="value">The value of the field.</param>
    /// <exception cref="InvalidOperationException">The value can't be converted to the field type.</exception>
    /// <remarks>This method doesn't honor the <see cref="Mono.Cecil.FieldDefinition.IsInitOnly" /> property of fields.</remarks>
    public void SetField(ClosedField fld, ICompileTimeValue value)
    {
        if (!value.Type.CanConvertTo(fld.FieldType))
            throw new InvalidOperationException($"Type {value.Type} cannot be converted to {fld.FieldType}.");
        fields[fld] = value;
    }

    public bool TrySerialize(Span<byte> span)
    {
        throw new NotImplementedException();
    }

    public object? Value => this;
}
