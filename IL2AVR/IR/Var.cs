namespace IL2AVR.IR;

public sealed class Var(ClosedVariable variable) : ILocal
{
    public int Index => Variable.UnderlyingVariable.Index;
    public IClosedType Type => Variable.VariableType;
    public readonly ClosedVariable Variable = variable;

    public override string ToString() => $"var{Index}";
}
