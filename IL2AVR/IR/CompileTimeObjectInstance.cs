using System;
using System.Collections.Generic;
namespace IL2AVR.IR;

/// <summary>Represents an object instance that lives at compile-time.</summary>
public sealed record CompileTimeObjectInstance : ObjectInstance, ICompileTimeValue
{
    private static void CheckType(ClosedType type)
    {
        if (!type.Kind.IsAggregateKind())
            throw new InvalidOperationException("Cannot construct an instance of a non-aggregate type!");
    }
    private void PopulateFields(ClosedType type)
    {
        foreach (ClosedField fld in type.Fields)
            fields[fld] = fld.FieldType.DefaultCompileTimeValue();
    }
    /// <summary>Constructs a <see cref="CompileTimeObjectInstance" /> from a value-type <see cref="ClosedType" />.</summary>
    /// <param name="type">The value-type to construct an instance from.</param>
    /// <exception cref="InvalidOperationException">Attempted to construct an unitialized compile-time type instance of a reference type.</exception>
    public CompileTimeObjectInstance(ClosedType type) : base(type)
    {
        CheckType(type);
        if (!type.Kind.IsReferenceKind())
            throw new InvalidOperationException("A constructor-less instance of a type instance can only be used on value types.");
        Type = type;
        fields = new(type.Fields.Count);
        PopulateFields(type);
    }
    /// <summary>Constructs a <see cref="CompileTimeObjectInstance" /> from a constructor reference.</summary>
    /// <param name="bag">The compiler bag.</param>
    /// <param name="ctor">The type constructor to use.</param>
    /// <param name="constructorArguments">The constructor arguments.</param>
    /// <exception cref="InvalidOperationException"><paramref name="ctor" /> is not a constructor.</exception>
    public CompileTimeObjectInstance(in CompilerBag bag, ClosedMethod ctor, params IReadOnlyList<ICompileTimeValue> constructorArguments) : base(ctor.DeclaringType)
    {
        CheckType(ctor.DeclaringType);
        if (!ctor.IsConstructor)
            throw new InvalidOperationException("A constructor method is necessary.");
        ClosedType type = (ClosedType)(Type = ctor.DeclaringType);
        fields = new(type.Fields.Count);
        PopulateFields(type);
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

    /// <inheritdoc />
    public bool TrySerialize(Span<byte> span)
    {
        throw new NotImplementedException();
    }

    /// <inheritdoc />
    public object? Value => this;
}
