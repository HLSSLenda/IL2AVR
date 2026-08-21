namespace IL2AVR.IR;

public readonly record struct Text : IValue
{
    public Text(ClosedTypeSystem typeSystem, string value)
    {
        Type = typeSystem.String;
        Value = value;
    }
    public ClosedType Type { get; }
    public readonly string Value;
}
