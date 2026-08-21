namespace IL2AVR;

public record Local(ClosedType Type, MemoryLocation Location, ushort Offset, ushort Size) : IValue;
