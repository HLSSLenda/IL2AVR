using IL2AVR.IR;
using Mono.Cecil;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
namespace IL2AVR;

public static class Extensions
{
    /// <summary>Closes an arbitrary <see cref="TypeReference" />.</summary>
    /// <param name="self">The <see cref="TypeReference" /> to close.</param>
    /// <param name="genericArguments">The generic arguments that's used for this entire type instance.</param>
    /// <returns>A <see cref="IClosedType" /> representing the closed type.</returns>
    public static IClosedType Close(this TypeReference self, IReadOnlyDictionary<GenericParameter, IClosedType>? genericArguments = null)
    {
        genericArguments ??= EmptyDictionary<GenericParameter, IClosedType>.Instance;
        Stack<ClosedTypeKind> kinds = new();
        // first we decompose the type into separate parts
        while (true)
        {
            if (self is ArrayType t1)
            {
                kinds.Push(ClosedTypeKind.Array);
                self = t1.ElementType;
            }
            else if (self is ByReferenceType t2)
            {
                kinds.Push(ClosedTypeKind.ByReference);
                self = t2.ElementType;
            }
            else if (self is PointerType t3)
            {
                kinds.Push(ClosedTypeKind.Pointer);
                self = t3.ElementType;
            }
            else
                break;
        }
        // now we make a concrete GenericInstanceType from the type
        GenericInstanceType newType = new(self);
        if (self is GenericInstanceType type)
        {
            foreach (TypeReference arg in type.GenericArguments)
            {
                if (arg is GenericParameter parameter)
                {
                    if (!genericArguments.TryGetValue(parameter, out IClosedType? t) ||
                            (!IsAggregateKind(t.Kind) && t.Kind != ClosedTypeKind.Array))
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
                if (!genericArguments.TryGetValue(parameter, out IClosedType? t) ||
                        (!IsAggregateKind(t.Kind) && t.Kind != ClosedTypeKind.Array))
                    throw new WtfException();
                newType.GenericArguments.Add(t.UnderlyingType);
            }
        }
        // now we construct the final type
        IClosedType finalType = ClosedType.Get(newType);
        while (kinds.TryPop(out ClosedTypeKind kind))
        {
            switch (kind)
            {
                case ClosedTypeKind.Array:
                    finalType = new ClosedArrayType(finalType);
                    break;
                case ClosedTypeKind.ByReference:
                    finalType = new ClosedByReferenceType(finalType);
                    break;
                case ClosedTypeKind.Pointer:
                    finalType = new ClosedPointerType(finalType);
                    break;
            }
        }

        return finalType;
    }
    /// <summary>Checks if a closed type can be converted to another.</summary>
    /// <param name="src">The source type.</param>
    /// <param name="dst">The destination type.</param>
    /// <returns><see langword="true" /> if <paramref name="src" /> can be converted to <paramref name="dst"/>.</returns>
    /// <remarks>
    /// A type can be converted to another if:
    /// (TODO)
    /// </remarks>
    public static bool CanConvertTo(this IClosedType src, IClosedType dst)
    {
        if (src == dst || src.Equals(dst) ||
                ((src.Kind == ClosedTypeKind.ByReference || src.Kind == ClosedTypeKind.Pointer) &&
                 (dst.Kind == ClosedTypeKind.ByReference || dst.Kind == ClosedTypeKind.Pointer)) ||
                 (src.Kind == ClosedTypeKind.Sentinel && dst.Kind == ClosedTypeKind.Sentinel))
            return true;
        ClosedType srcRoot = GetRootElementType(src);
        ClosedType dstRoot = GetRootElementType(dst);
        if (IsNumberKind(srcRoot.Kind))
            return IsNumberKind(dstRoot.Kind) && srcRoot.Size <= dstRoot.Size;
        ClosedType? currentBase = srcRoot;
        while ((currentBase = currentBase!.BaseType) is not null)
            if (currentBase == dstRoot)
                return true;
        return false;
    }
    public static ClosedMethod Close(this MethodReference method, IReadOnlyDictionary<GenericParameter, IClosedType>? genericArguments = null) => ClosedMethod.Get(method, genericArguments);
    /// <summary>Gets the default compile-time value for a <see cref="IClosedType" />.</summary>
    /// <param name="type">The type.</param>
    /// <returns><see cref="Null" /> for reference-types, <see cref="Number" /> for primitive types, and an unitialized <see cref="CompileTimeObjectInstance" /> for value types.</returns>
    /// <exception cref="InvalidOperationException">Tried to get the default value of a sentinel or the void type.</exception>
    public static ICompileTimeValue DefaultCompileTimeValue(this IClosedType type)
    {
        if (type.Kind == ClosedTypeKind.Sentinel)
            throw new InvalidOperationException("Cannot get the default value of a sentinel.");
        if (type.Kind == ClosedTypeKind.Void)
            throw new InvalidOperationException("Cannot get the default value of void.");
        if (IsReferenceKind(type.Kind))
            return new Null(type);
        if (IsNumberKind(type.Kind))
            return new Number(type, 0);
        return new CompileTimeObjectInstance((ClosedType)type);
    }
    public static bool Extends(this ClosedType child, ClosedType father)
    {
        ClosedType? current = child;
        while (current is not null)
        {
            if (current == father)
                return true;
            current = current.BaseType;
        }
        return false;
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
    /// <summary>Gets a member (field, method or property) by a name string from a module and its references.</summary>
    /// <param name="module">The module definition to search.</param>
    /// <param name="name">The name to search.</param>
    /// <returns>A valid <see cref="MemberReference" /> from the name, or <see langword="null" /> if not found.</param>
    public static MemberReference? GetMemberByName(this ModuleDefinition module, ReadOnlySpan<char> name)
    {
        Stack<ModuleDefinition> modules = new(1 + module.ModuleReferences.Count);
        HashSet<ModuleReference> visited = [];
        modules.Push(module);
        while (modules.TryPop(out module))
        {
            if (visited.Add(module))
                continue;
            int currentIndex = 0;
            TypeDefinition? type = null;
            while ((currentIndex = name[currentIndex..].IndexOf('.')) != -1)
            {
                if ((type = module.GetType(name[currentIndex..].ToString())) is not null)
                    break;
                name = name[currentIndex..];
            }
            if (type is not null)
            {
                while ((currentIndex = name[currentIndex..].IndexOf('.')) != -1)
                {
                    string n = name[currentIndex..].ToString();
                    try
                    {
                        return type.Fields.First(fld => fld.Name == n);
                    }
                    catch (InvalidOperationException)
                    {
                    }
                    try
                    {
                        return type.Methods.First(method => method.Name == n);
                    }
                    catch (InvalidOperationException)
                    {
                    }
                    try
                    {
                        return type.Properties.First(method => method.Name == n);
                    }
                    catch (InvalidOperationException)
                    {
                    }
                    name = name[currentIndex..];
                }
                return type;
            }
            foreach (ModuleReference reference in module.ModuleReferences)
                modules.Push(module.AssemblyResolver.Resolve(new(reference.Name, version: null)).MainModule);
        }
        return null;
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

    /// <summary>Gets the root element type of a <see cref="IClosedType" />.</summary>
    /// <param name="type">The closed type.</param>
    /// <returns>The root closed type.</returns>
    public static ClosedType GetRootElementType(this IClosedType type)
    {
        while (type is not ClosedType || type.ElementType is not null)
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

    // see ClosedTypeKind.cs
    public static bool IsAggregateKind(this ClosedTypeKind kind) => kind == ClosedTypeKind.AggregateReference || kind == ClosedTypeKind.AggregateValue;
    public static bool IsBasicKind(this ClosedTypeKind kind) => kind >= ClosedTypeKind.TypedReference;
    public static bool IsReferenceKind(this ClosedTypeKind kind) =>
        kind == ClosedTypeKind.AggregateReference || (kind >= ClosedTypeKind.Array && kind <= ClosedTypeKind.Pointer) ||
        kind >= ClosedTypeKind.Object;
    public static bool IsFloatingPointKind(this ClosedTypeKind kind) => kind == ClosedTypeKind.Double || kind == ClosedTypeKind.Single;
    public static bool IsIntegerKind(this ClosedTypeKind kind) => kind >= ClosedTypeKind.Int16 && kind <= ClosedTypeKind.UIntPtr;
    public static bool IsNumberKind(this ClosedTypeKind kind) => kind >= ClosedTypeKind.Int16 && kind <= ClosedTypeKind.Single;
    public static bool IsSignedKind(this ClosedTypeKind kind) => (kind >= ClosedTypeKind.Int16 && kind <= ClosedTypeKind.SByte) || IsFloatingPointKind(kind);
    public static bool IsUnsignedKind(this ClosedTypeKind kind) => kind >= ClosedTypeKind.Int16 && kind <= ClosedTypeKind.SByte;

    /// <summary>Gets the low byte of this ushort (n &amp; 0xff).</summary>
    /// <param name="n">The ushort to get the low byte.</param>
    /// <returns>The low byte of <paramref name="n" />.</returns>
    public static byte Low(this ushort n) => (byte)(n & 0xff);

    public static void Log<T>(T logger, CompilerDiagnosticCollection diagnostics) where T : ILogger
    {
        if (diagnostics.Fatals is not null)
            foreach (CompilerDiagnostic diagnostic in diagnostics.Fatals)
                logger.Fatal("(FATAL) {0}", diagnostic);
    }
    public static IClosedType Max(IClosedType a, IClosedType b) => a.Size >= b.Size ? a : b;
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
    /// <summary>Reinterprets a value of a specific type as another type.</summary>
    /// <typeparam name="TFrom">The source parameter type.</typeparam>
    /// <typeparam name="TTo">The destination return type.</typeparam>
    /// <param name="src">The value to reinterpret.</param>
    /// <returns><paramref name="src" /> reinterpreted as <typeparamref name="TTo" />.</returns>
    public static TTo Reinterpret<TFrom, TTo>(this TFrom src)
        where TFrom : unmanaged, allows ref struct
        where TTo : unmanaged, allows ref struct =>
            Unsafe.As<TFrom, TTo>(ref src);

    /// <summary>Gets the total SRAM memory of a processor.</summary>
    /// <param name="processor">The processor.</param>
    /// <returns>The total SRAM memory.</returns>
    public static ushort TotalMemory(this IProcessor processor) => (ushort)(processor.RamEnd - processor.RamStart);
}
