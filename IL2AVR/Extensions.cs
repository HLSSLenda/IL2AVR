using Mono.Cecil;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
namespace IL2AVR;

public static class Extensions
{
    /// Closes an arbitrary <see cref="TypeReference" />. 
    public static IClosedType Close(this TypeReference self, IReadOnlyDictionary<GenericParameter, ClosedType> genericArguments)
    {
        Stack<TypeKind> kinds = new();
        while (true)
        {
            if (self is ArrayType t1)
            {
                kinds.Push(TypeKind.Array);
                self = t1.ElementType;
            }
            else if (self is ByReferenceType t2)
            {
                kinds.Push(TypeKind.ByReference);
                self = t2.ElementType;
            }
            else if (self is PointerType t3)
            {
                kinds.Push(TypeKind.Pointer);
                self = t3.ElementType;
            }
            else
                break;
        }
        GenericInstanceType newType = new(self);
        if (self is GenericInstanceType type)
        {
            foreach (TypeReference arg in type.GenericArguments)
            {
                if (arg is GenericParameter parameter)
                {
                    if (!genericArguments.TryGetValue(parameter, out ClosedType? t))
                        throw new WtfException();
                    newType.GenericArguments.Add(t.UnderlyingType);
                }
                else
                    newType.GenericArguments.Add(arg);
            }
        }
        else
        {
            TypeDefinition typeDef = self.Resolve();
            foreach (GenericParameter parameter in typeDef.GenericParameters)
            {
                if (!genericArguments.TryGetValue(parameter, out ClosedType? t))
                    throw new WtfException();
                newType.GenericArguments.Add(t.UnderlyingType);
            }
        }

        IClosedType finalType = ClosedType.Get(newType);
        while (kinds.TryPop(out TypeKind kind))
        {
            switch (kind)
            {
                case TypeKind.Array:
                    finalType = new ClosedArrayType(finalType);
                    break;
                case TypeKind.ByReference:
                    finalType = new ClosedByReferenceType(finalType);
                    break;
                case TypeKind.Pointer:
                    finalType = new ClosedPointerType(finalType);
                    break;
            }
        }

        return finalType;
    }
    public static TValue? GetOrAdd<TKey, TValue>(this IDictionary<TKey, TValue> dictionary, TKey key, TValue defaultValue) =>
        dictionary.TryGetValue(key, out TValue? value) ? value : dictionary[key] = defaultValue;
    public static TypeReference? GetSigned(this TypeReference? type, TypeSystem typeSystem)
    {
        if (type == typeSystem.Byte)
            return typeSystem.SByte;
        if (type == typeSystem.UInt16)
            return typeSystem.Int16;
        if (type == typeSystem.UInt32)
            return typeSystem.Int32;
        if (type == typeSystem.UInt64)
            return typeSystem.Int64;
        if (type == typeSystem.UIntPtr)
            return typeSystem.IntPtr;
        return type;
    }
    public static MethodDefinition? GetMethod(
        this TypeDefinition type,
        TypeReference returnType,
        string name,
        int genericCount = 0,
        params IReadOnlyCollection<TypeReference> parameters
    ) => type.Methods.FirstOrDefault(method =>
        method.ReturnType.MetadataToken == returnType.MetadataToken &&
        method.Name == name &&
        method.GenericParameters.Count == genericCount &&
        method.Parameters.Select(static param => param.ParameterType).SequenceEqual(parameters));
    public static ClosedType GetRootElementType(this IClosedType type)
    {
        while (type is not ClosedType)
            type = type.ElementType;
        return (ClosedType)type;
    }
    
    public static TypeReference? GetUnsigned(this TypeReference? type, TypeSystem typeSystem)
    {
        if (type == typeSystem.Int16)
            return typeSystem.UInt16;
        if (type == typeSystem.Int32)
            return typeSystem.UInt32;
        if (type == typeSystem.Int64)
            return typeSystem.UInt64;
        if (type == typeSystem.IntPtr)
            return typeSystem.UIntPtr;
        if (type == typeSystem.SByte)
            return typeSystem.Byte;
        return type;
    }
    /// <summary>Gets the high byte of this ushort (n >> 8).</summary>
    /// <param name="n">The ushort to get the high byte.</param>
    /// <returns>The high byte of <paramref name="n" />.</returns>
    public static byte High(this ushort n) => (byte)(n >> 8);
    /// <summary>Gets the low byte of this ushort (n &amp; 0xff).</summary>
    /// <param name="n">The ushort to get the low byte.</param>
    /// <returns>The low byte of <paramref name="n" />.</returns>
    public static byte Low(this ushort n) => (byte)(n & 0xff);
    private static Dictionary<MemberReference, IReadOnlyList<ClosedType>>? instantiator = null;

    /*public static ClosedType InstantiateGeneric(this TypeReference self, IReadOnlyList<ClosedType>? genericArguments)
    {
        if (genericArguments is null)
            return ClosedType.Get(self);
        (instantiator ??= new(1)).Clear();
        if (genericArguments.Count > 0)
            instantiator[genericArguments[0].UnderlyingReference.DeclaringType] = genericArguments;
        return InstantiateGeneric(self, instantiator);
    }
    public static ClosedType InstantiateGeneric(this TypeReference self, IReadOnlyDictionary<MemberReference, IReadOnlyList<ClosedType>>? generics)
    {
        if (generics is null)
            return ClosedType.Get(self);
        if (self is GenericParameter genericParameter)
            return generics[(MemberReference)genericParameter.DeclaringMethod ?? genericParameter.DeclaringType][genericParameter.Position];
        if (self is GenericInstanceType type)
        {
            GenericInstanceType newType = new(self);
            foreach (TypeReference tgeneric in type.GenericArguments)
                newType.GenericArguments.Add(InstantiateGeneric(tgeneric, generics).UnderlyingReference);
            return ClosedType.Get(newType);
        }
        return ClosedType.Get(self);
    }*/
    public static void Log<T>(T logger, CompilerDiagnosticCollection diagnostics) where T : ILogger
    {
        if (diagnostics.Fatals is not null)
            foreach (CompilerDiagnostic diagnostic in diagnostics.Fatals)
                logger.Fatal("(FATAL) {0}", diagnostic);
    }
    public static bool IsImmutable(this TypeDefinition type, TypeSystem typeSystem) =>
        type == typeSystem.Boolean || type == typeSystem.Byte || type == typeSystem.Char || type == typeSystem.Double || type == typeSystem.Int16 ||
        type == typeSystem.Int32 || type == typeSystem.Int64 || type == typeSystem.IntPtr || type == typeSystem.SByte || type == typeSystem.Single ||
        type == typeSystem.UInt16 || type == typeSystem.UInt32 || type == typeSystem.UInt64 || type == typeSystem.UIntPtr ||
        type.Fields.All(field => IsImmutable(field.FieldType.Resolve(), typeSystem));
    public static bool IsReference(this TypeReference type) =>
        type.IsArray || type.IsByReference || type.IsFunctionPointer || type.IsPointer || !type.IsValueType;
    public static bool IsSigned(this TypeReference type, TypeSystem typeSystem) =>
        type == typeSystem.Int16 || type == typeSystem.Int32 || type == typeSystem.Int64 || type == typeSystem.IntPtr || type == typeSystem.SByte;
    public static bool IsUnsigned(this TypeReference type, TypeSystem typeSystem) =>
        type == typeSystem.Byte || type == typeSystem.UInt16 || type == typeSystem.UInt32 || type == typeSystem.UInt64 || type == typeSystem.UIntPtr;
    // could've used Unsafe.As directly, but it only works on ref, and idc about copies bc it'll be used for small types only (stack float reinterpretation)
    public static TTo Reinterpret<TFrom, TTo>(this TFrom src)
        where TFrom : unmanaged, allows ref struct
        where TTo : unmanaged, allows ref struct =>
            Unsafe.As<TFrom, TTo>(ref src);
}
