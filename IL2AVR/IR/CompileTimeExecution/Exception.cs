using System;
namespace IL2AVR.IR;

public sealed class Exception : ICompileTimeValue
{
    private readonly CompileTimeClosedTypeInstance _exceptionObject;

    public Exception(in CompilerBag bag, CompileTimeClosedTypeInstance exceptionObject)
    {
        if (!exceptionObject.Type.Extends(bag.Exception))
            throw new InvalidOperationException("Exception is not a exception type!");
        _exceptionObject = exceptionObject;
    }

    public IClosedType Type => _exceptionObject.Type;
    object? ICompileTimeValue.Value => ((ICompileTimeValue)_exceptionObject).Value;
}