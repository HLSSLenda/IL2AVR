using Mono.Cecil;
namespace IL2AVR;

public abstract record CompileTimeValueDefinition
{
    internal CompileTimeValueDefinition()
    {
    }
    internal abstract bool Validate(ClosedType type);

    public sealed record Boolean(bool Value) : CompileTimeValueDefinition
    {
        internal override bool Validate(ClosedType type)
        {
            throw new System.NotImplementedException();
        }
    }

    public sealed record Byte(byte Value) : CompileTimeValueDefinition
    {
        internal override bool Validate(ClosedType type)
        {
            throw new System.NotImplementedException();
        }
    }

    public sealed record SByte(sbyte Value) : CompileTimeValueDefinition
    {
        internal override bool Validate(ClosedType type)
        {
            throw new System.NotImplementedException();
        }
    }

    public sealed record Int16(short Value) : CompileTimeValueDefinition
    {
        internal override bool Validate(ClosedType type)
        {
            throw new System.NotImplementedException();
        }
    }

    public sealed record UInt16(ushort Value) : CompileTimeValueDefinition
    {
        internal override bool Validate(ClosedType type)
        {
            throw new System.NotImplementedException();
        }
    }

    public sealed record Int32(int Value) : CompileTimeValueDefinition
    {
        internal override bool Validate(ClosedType type)
        {
            throw new System.NotImplementedException();
        }
    }

    public sealed record UInt32(ushort Value) : CompileTimeValueDefinition
    {
        internal override bool Validate(ClosedType type)
        {
            throw new System.NotImplementedException();
        }
    }

    public sealed record Int64(long Value) : CompileTimeValueDefinition
    {
        internal override bool Validate(ClosedType type)
        {
            throw new System.NotImplementedException();
        }
    }

    public sealed record UInt64(ulong Value) : CompileTimeValueDefinition
    {
        internal override bool Validate(ClosedType type)
        {
            throw new System.NotImplementedException();
        }
    }

    public sealed record String(string? Value) : CompileTimeValueDefinition
    {
        internal override bool Validate(ClosedType type)
        {
            throw new System.NotImplementedException();
        }
    }

    public sealed record Type(TypeReference type) : CompileTimeValueDefinition
    {
        internal override bool Validate(ClosedType type)
        {
            throw new System.NotImplementedException();
        }
    }

    public sealed record Array<T>(params T[] Values) : CompileTimeValueDefinition where T : CompileTimeValueDefinition
    {
        internal override bool Validate(ClosedType type)
        {
            throw new System.NotImplementedException();
        }
    }

}
