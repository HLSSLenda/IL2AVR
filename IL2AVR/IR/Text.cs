namespace IL2AVR.IR;

public readonly record struct Text : ICompileTimeValue
{
    public Text(ClosedTypeSystem typeSystem, string value)
    {
        Type = typeSystem.String;
        Value = value;
    }
    public IClosedType Type { get; }
    public readonly string Value;
    private readonly object Value()
    {
        return Value;
    }
}
