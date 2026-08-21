namespace IL2AVR.IR;

public sealed class Arg(ClosedParameter parameter) : IValue
{
    public int Index => parameter.UnderlyingParameter.Index;
    public ClosedType Type => parameter.ParameterType;

    public override string ToString() => $"arg{Index}";
}
