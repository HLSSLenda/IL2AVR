namespace IL2AVR.IR;

public readonly record struct Text : IValue
{
    public Text(ClosedTypeSystem typeSystem, string value)
    {
        Type = typeSystem.String;
        Value = value;
    }
    public IClosedType Type { get; }
    public readonly string Value;
}
