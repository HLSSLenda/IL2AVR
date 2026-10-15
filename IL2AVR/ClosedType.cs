using Mono.Cecil;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
namespace IL2AVR;

/// <summary>Represents a fully closed aggregate type.</summary>
public sealed class ClosedType : IEquatable<ClosedType>, IEquatable<TypeReference>, IClosedType
{
    private static readonly Dictionary<TypeReference, ClosedType> cachedTypes = new(18, new TypeReferenceEqualityComparer());

    private ushort addFieldOffset = 0;
    private readonly List<ClosedField> fields;
    /// <summary>Every field used of this type.</summary>
    public IReadOnlyCollection<ClosedField> Fields => fields;
    /// <summary>Every generic argument used for the instantiation of this type.</summary>
    public readonly IReadOnlyDictionary<GenericParameter, IClosedType> GenericArguments;
    /// <summary><see langword="true" /> if this type is a primitive type.</summary>
    public readonly bool IsBasic;
    /// <summary><see langword="true" /> if this type is immutable, meaning that no field is changed.</summary>
    /// <remarks>
    /// The type can be immutable even if there are methods that mutate fields.
    /// What makes a type immutable is the code itself not calling method that mutate fields,
    /// or not mutating fields by themselves.
    /// This doesn't apply for <see cref="IR.CompileTimeObjectInstance" /> because it's a purely compile-time construct.
    /// </remarks>
    public bool IsImmutable { get; internal set; } = true;
    /// <inheritdoc />
    public string Name => UnderlyingType.Name;
    /// <inheritdoc />
    public ushort MemorySize { get; private set; }
    /// <inheritdoc />
    public ushort Size { get; private set; }
    public readonly TypeDefinition UnderlyingType;
    /// <inheritdoc />
    public MemberReference MemberReference => UnderlyingType;

    internal ClosedType(TypeReference type, ushort size = 0, ClosedTypeKind kind = ClosedTypeKind.AggregateReference)
    {
        if (type.HasGenericParameters)
            throw new WtfException($"A ClosedType cannot instantiate an open type! {type}");

        UnderlyingType = type.Resolve();
        if (type is GenericInstanceType t)
        {
            Dictionary<GenericParameter, IClosedType> args = new(UnderlyingType.GenericParameters.Count, new GenericParameterEqualityComparer());
            for (int i = 0; i < UnderlyingType.GenericParameters.Count; ++i)
                args[UnderlyingType.GenericParameters[i]] = t.GenericArguments[i].Close();
            GenericArguments = args;
        }
        else
            GenericArguments = EmptyDictionary<GenericParameter, IClosedType>.Instance;

        if (size >= 0)
            MemorySize = (ushort)size;
        if (UnderlyingType.IsAutoLayout)
            fields = new(UnderlyingType.Fields.Count);
        else if (UnderlyingType.IsExplicitLayout)
        {
            fields = [..UnderlyingType.Fields.Select(field => new ClosedField(field, GenericArguments))];
            if (size == 0 && fields.Count > 0)
            {
                IEnumerable<ClosedField> sorted = fields.OrderByDescending(static fld => fld.Offset);
                ushort largestOffset = fields.First().Offset;
                foreach (ClosedField fld in sorted)
                {
                    if (fld.Offset != largestOffset)
                        break;
                    MemorySize = Math.Max(MemorySize, fld.FieldType.Size);
                }
                MemorySize += largestOffset;
            }
        }
        else
        {
            fields = new(UnderlyingType.Fields.Count);
            foreach (FieldReference fld in UnderlyingType.Fields)
                AddFieldInternal(fld);
        }
        Kind = kind;
        Size = UnderlyingType.IsReference() ? (ushort)2 : MemorySize;
        cachedTypes[type] = this;
    }
    /// <summary>Gets a <see cref="ClosedType" /> instance.</summary>
    public static ClosedType Get(TypeReference type)cmpq
    {
        ref ClosedType? closedType = ref CollectionsMarshal.GetValueRefOrAddDefault(cachedTypes, type, out bool exists);
        if (!exists)
            closedType = new(type, kind: type.IsReference() ? ClosedTypeKind.AggregateReference : ClosedTypeKind.AggregateValue);
        return closedType!;
    }
    public static bool operator ==(ClosedType lhs, ClosedType rhs) => lhs.Equals(rhs);
    public static bool operator !=(ClosedType lhs, ClosedType rhs) => !lhs.Equals(rhs);
    public static bool operator ==(ClosedType lhs, TypeReference rhs) => lhs.Equals(rhs);
    public static bool operator !=(ClosedType lhs, TypeReference rhs) => !lhs.Equals(rhs);

    public ClosedType? BaseType => (ClosedType?)UnderlyingType.BaseType?.Close(GenericArguments);

    public IEnumerable<ClosedField> IterateFields()
    {
        Stack<ClosedType> typeStack = new();
        ClosedType? t = this;
        while (t is not null)
        {
            typeStack.Push(t);
            t = t.BaseType;
        }
        ushort memorySize = 0;
        foreach (ClosedType type in typeStack)
        {
            ClosedField? last = null;
            foreach (ClosedField fld in type.Fields.OrderBy(static x => x.Offset))
            {
                yield return new(fld.UnderlyingField, fld.DeclaringType, fld.FieldType, (ushort)(memorySize + fld.Offset));
                last = fld;
            }
            if (last is not null)
                memorySize += (ushort)(last.Offset + last.FieldType.MemorySize);
        }
    }

    private ClosedField AddFieldInternal(FieldReference field)
    {
        IClosedType fieldType = field.FieldType.Close(GenericArguments);
        ushort align = Math.Min((ushort)Math.Max((ushort)1, (ushort)UnderlyingType.PackingSize), fieldType.Size);
        if (align > 0 && addFieldOffset % align != 0)
            addFieldOffset += (ushort)(align - (addFieldOffset % align));
        ClosedField fld = new(field, this, fieldType, addFieldOffset);
        fields.Add(fld);
        addFieldOffset += fieldType.Size;
        MemorySize = addFieldOffset;
        return fld;
    }
    public ClosedField? AddField(FieldReference field)
    {
        if (IsBasic || UnderlyingType.IsExplicitLayout || UnderlyingType.IsSequentialLayout || field.DeclaringType.MetadataToken != UnderlyingType.MetadataToken)
            return null;
        try
        {
            return fields.First(x => x.UnderlyingField.Name == field.Name);
        }
        catch (InvalidOperationException)
        {
            return AddFieldInternal(field);
        }
    }

    public bool AddMethod(MethodReference method)
    {
        if (method.DeclaringType.FullName != FullName || !method.Resolve().IsVirtual)
            return false;
        methods.Add(new(method, GenericArguments));
        return true;
    }

    private IReadOnlyCollection<ClosedType>? interfaces = null;
    public IReadOnlyCollection<ClosedType> Interfaces =>
        interfaces ??= [..UnderlyingType.Interfaces.Select(static interf => Get(interf.InterfaceType))];
    private readonly List<ClosedMethod> methods = [];
    public IReadOnlyCollection<ClosedMethod> Methods => methods;
    public string FullName => UnderlyingType.FullName;
    public bool IsInterface => UnderlyingType.IsInterface;
    public bool IsExplicitLayout => UnderlyingType.IsExplicitLayout;
    public ModuleDefinition Module => UnderlyingType.Module;

    public IClosedType ElementType => this;

    public ClosedTypeKind Kind { get; }

    TypeReference IClosedType.UnderlyingType => UnderlyingType;

    public DataLocation Location => throw new NotImplementedException();

    public bool Equals(TypeReference? other) =>
        UnderlyingType.MetadataToken == other?.MetadataToken && UnderlyingType.Module == other.Module &&
        (other is not GenericInstanceType t || t.GenericArguments.Select(static arg => Get(arg)).SequenceEqual(GenericArguments.Values));
    public bool Equals(ClosedType? other) => UnderlyingType.MetadataToken == other?.UnderlyingType.MetadataToken && Fields.SequenceEqual(other.Fields);

    public override bool Equals(object? obj) => obj switch
    {
        TypeReference type => Equals(type),
        ClosedType type => Equals(type),
        _ => false
    };

    public override int GetHashCode() => HashCode.Combine(UnderlyingType.MetadataToken, GenericArguments);

    public override string ToString() => GenericArguments.Count == 0 ? UnderlyingType.FullName : $"{UnderlyingType.FullName}<{string.Join(", ", GenericArguments.Values)}>";
}
