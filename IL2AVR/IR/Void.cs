using System;
using Mono.Cecil;

namespace IL2AVR.IR;

public sealed class Void(ushort size) : ICompileTimeValue, IClosedType
{
    private sealed class TypeWithCustomSize(ushort size) : IClosedType
    {
        public IClosedType ElementType => throw new NotImplementedException();
        public ClosedTypeKind Kind => throw new NotImplementedException();
        public ushort MemorySize => size;
        public ushort Size => size;
        public TypeReference UnderlyingType => throw new NotImplementedException();
        public MemberReference MemberReference => throw new NotImplementedException();
        public string Name => throw new NotImplementedException();
    }
    public object? Value => throw new InvalidOperationException();
    public IClosedType Type => new TypeWithCustomSize(size);
    public bool TrySerialize(Span<byte> span) => throw new InvalidOperationException();

    public IClosedType ElementType => Type.ElementType;
    public ClosedTypeKind Kind => Type.Kind;
    public ushort MemorySize => Type.MemorySize;
    public ushort Size => Type.Size;
    public TypeReference UnderlyingType => Type.UnderlyingType;
    public MemberReference MemberReference => Type.MemberReference;
    public string Name => Type.Name;
}