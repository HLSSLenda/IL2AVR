namespace IL2AVR.IR;

public sealed class Arg(ClosedParameter parameter) : ILocal
{
    public int Index => Parameter.UnderlyingParameter.Index;
    public IClosedType Type => Parameter.ParameterType;
    public readonly ClosedParameter Parameter = parameter;

    public override string ToString() => $"arg{Index}";
}
