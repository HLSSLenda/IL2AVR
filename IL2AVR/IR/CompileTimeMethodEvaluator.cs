using System;
using System.Buffers;
using System.Collections.Generic;
using System.Runtime.InteropServices;
namespace IL2AVR.IR;

public sealed class CompileTimeMethodEvaluator : IDisposable
{
    public enum StepResult
    {
        NotRunning,
        Ok,
        Returned,
        Faulted
    }
    public readonly ref struct CurrentStep(StepResult result, ICompileTimeValue? value = null)
    {
        public readonly StepResult Result = result;
        public readonly ICompileTimeValue? Value = value;
    }
    public readonly record struct Local(ushort Address, ICompileTimeValue Value) : ICompileTimeValue
    {
        public IClosedType Type => Value.Type;
        object? ICompileTimeValue.Value => Value;
    }
    public readonly record struct StackFrame(ClosedMethod Method, IReadOnlyList<ICompileTimeValue> arguments)
    {
        public readonly IReadOnlyList<ICompileTimeValue> Arguments => arguments;
        internal readonly MetaStack<Local> locals = [];
        public readonly IReadOnlyList<Local> Locals => locals;
    }
    public CompileTimeMethodEvaluator(ClosedMethod method, IReadOnlyList<ICompileTimeValue> arguments, int memorySize, int maxCallDepth = byte.MaxValue)
    {
        if (arguments.Count != method.Parameters.Count)
            throw new ArgumentException($"Expected {method.Parameters.Count} parameters, got {arguments.Count}", nameof(arguments));
        for (int i = 0; i < method.Parameters.Count; ++i)
            if (!arguments[i].Type.CanConvertTo(method.Parameters[i].ParameterType))
                throw new InvalidOperationException($"Parameter {i} of type {method.Parameters[i].ParameterType} isn't compatible with {arguments[i].Type}.");
        Arguments = arguments;
        MaxCallDepth = maxCallDepth;
        Memory = ArrayPool<byte>.Shared.Rent(memorySize);
        Reset();
    }

    public void Dispose()
    {
        if (Memory is not null)
        {
            ArrayPool<byte>.Shared.Return(Memory);
            Memory = null;
        }
    }

    public readonly IReadOnlyList<ICompileTimeValue> Arguments;
    public IntermediateInstruction? CurrentInstruction { get; private set; }
    private readonly MetaStack<StackFrame> _frames = new([]);
    public IReadOnlyList<StackFrame> Frames => _frames;
    public readonly int MaxCallDepth;
    public byte[]? Memory { get; private set; }
    public readonly ClosedMethod Method;

    public void Reset()
    {
        ObjectDisposedException.ThrowIf(Memory is null, this);
        CurrentInstruction = Method.Body.EntryInstruction;
        _frames.Clear();
        _frames.Add(new(Method, Arguments));
    }
    public CurrentStep Step()
    {
        start:
        if (CurrentInstruction is null || _frames.Count == 0)
            return new(StepResult.NotRunning);
        ICompileTimeValue? value = null;
        StackFrame currentFrame = _frames.Peek();
        switch (CurrentInstruction.Id)
        {
            case InstructionId.Add:
                if (CurrentInstruction.Values is not [Number n1, Number n2])
                    throw new WtfException();
                currentFrame.locals.Add(new((ushort)currentFrame.Locals.Count, value = new Number(Extensions.Max(n1.Type, n2.Type), n1.Value + n2.Value)));
                break;
            case InstructionId.And:
                break;
            case InstructionId.Arglist:
                break;
            case InstructionId.Asm:
                throw new InvalidOperationException("Cannot evaluate assembly in compile-time!");
            case InstructionId.Box:
                break;
            case InstructionId.Br:
                break;
            case InstructionId.Break:
                break;
            case InstructionId.Call:
                if (_frames.Count >= MaxCallDepth)
                    throw new InvalidOperationException("Call limit overflow!");
                ClosedMethod called = ((CallInstruction)CurrentInstruction).Target;
                List<Local> args = new(called.Parameters.Count);
                currentFrame.locals.Pop(CollectionsMarshal.AsSpan(args));
                _frames.Add(new(called, args));
                break;
            case InstructionId.Calli:
                break;
            case InstructionId.Castclass:
                break;
            case InstructionId.Compare:
                break;
            case InstructionId.Conv:
                break;
            case InstructionId.Cpblk:
                break;
            case InstructionId.Cpobj:
                break;
            case InstructionId.Div:
                break;
            case InstructionId.Dup:
                break;
            case InstructionId.Initblk:
                break;
            case InstructionId.Initobj:
                break;
            case InstructionId.Isinst:
                break;
            case InstructionId.Jmp:
                break;
            case InstructionId.Ldelem:
                break;
            case InstructionId.Ldelema:
                break;
            case InstructionId.Ldfld:
                break;
            case InstructionId.Ldflda:
                break;
            case InstructionId.Ldftn:
                break;
            case InstructionId.Ldind:
                break;
            case InstructionId.Ldlen:
                break;
            case InstructionId.Ldobj:
                break;
            case InstructionId.Ldvirtftn:
                break;
            case InstructionId.Leave:
                break;
            case InstructionId.Localloc:
                break;
            case InstructionId.Mkrefany:
                break;
            case InstructionId.Mul:
                break;
            case InstructionId.Neg:
                break;
            case InstructionId.Newobj:
                break;
            case InstructionId.Nop:
                CurrentInstruction = CurrentInstruction.Next;
                goto start;
            case InstructionId.Not:
                break;
            case InstructionId.Or:
                break;
            case InstructionId.Pop:
                break;
            case InstructionId.Refanytype:
                break;
            case InstructionId.Refanyval:
                break;
            case InstructionId.Rem:
                break;
            case InstructionId.Ret:
                break;
            case InstructionId.Rethrow:
                break;
            case InstructionId.Shl:
                break;
            case InstructionId.Shr:
                break;
            case InstructionId.Sizeof:
                break;
            case InstructionId.Starg:
                break;
            case InstructionId.Stelem:
                break;
            case InstructionId.Stfld:
                break;
            case InstructionId.Stind:
                break;
            case InstructionId.Stloc:
                break;
            case InstructionId.Stobj:
                break;
            case InstructionId.Stsfld:
                break;
            case InstructionId.Sub:
                break;
            case InstructionId.Switch:
                break;
            case InstructionId.Throw:
                break;
            case InstructionId.Unbox:
                break;
            case InstructionId.Xor:
                break;
        }
        CurrentInstruction = CurrentInstruction.Next;
        return new(StepResult.Ok, value);
    }
    public ICompileTimeValue Run()
    {
        CurrentStep step;
        while ((step = Step()).Result != StepResult.Ok)
        {
        }
        return step.Value!;
    }
}