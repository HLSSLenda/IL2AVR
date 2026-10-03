using System;
namespace IL2AVR.IR;

public sealed class Exception : ICompileTimeValue
{
    private readonly CompileTimeObjectInstance _exceptionObject;

    public Exception(in CompilerBag bag, CompileTimeObjectInstance exceptionObject)
    {
        if (!exceptionObject.Type.Extends(bag.Exception))
            throw new InvalidOperationException("Exception is not a exception type!");
        _exceptionObject = exceptionObject;
    }

    public IClosedType Type => _exceptionObject.Type;
    private object? Value()
    {
        return ((ICompileTimeValue)_exceptionObject).GetIL2AVR.IR.ICompileTimeValue.Value();
    }
}