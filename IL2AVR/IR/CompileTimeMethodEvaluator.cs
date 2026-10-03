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
    public record struct StackFrame(ClosedMethod Method, IReadOnlyList<ICompileTimeValue> arguments, ushort framePointer)
    {
        public readonly IReadOnlyList<ICompileTimeValue> Arguments => arguments;
        public IntermediateInstruction? Instruction { get; internal set; } = Method.Body.EntryInstruction;
        public readonly ushort FramePointer = framePointer;
    }
    public CompileTimeMethodEvaluator(ClosedMethod method, IReadOnlyList<ICompileTimeValue> arguments, CompileTimeMemory eeprom, int memorySize, int maxCallDepth = byte.MaxValue, ArrayPool<byte>? arrayPool = null)
    {
        if (arguments.Count != method.Parameters.Count)
            throw new ArgumentException($"Expected {method.Parameters.Count} parameters, got {arguments.Count}", nameof(arguments));
        for (int i = 0; i < method.Parameters.Count; ++i)
            if (!arguments[i].Type.CanConvertTo(method.Parameters[i].ParameterType))
                throw new InvalidOperationException($"Parameter {i} of type {method.Parameters[i].ParameterType} isn't compatible with {arguments[i].Type}.");
        _eeprom = eeprom;
        Arguments = arguments;
        MaxCallDepth = maxCallDepth;
        Memory = new(memorySize, pool: arrayPool);
        Method = method;
        Reset();
    }

    public void Dispose() => Memory.Dispose();

    public readonly IReadOnlyList<ICompileTimeValue> Arguments;

    private readonly CompileTimeMemory _eeprom;
    private readonly MetaStack<StackFrame> _frames = new([]);
    public IReadOnlyList<StackFrame> Frames => _frames;
    public readonly int MaxCallDepth;
    public readonly CompileTimeMemory Memory;
    public readonly ClosedMethod Method;

    public void Reset()
    {
        ObjectDisposedException.ThrowIf(Memory is null, this);
        _frames.Clear();
        _frames.Add(new(Method, Arguments, (ushort)(Memory.Length - 1)));
    }
    public CurrentStep Step()
    {
        StackFrame currentFrame = _frames.Peek();
        if (currentFrame.Instruction is null || _frames.Count == 0)
            return new(StepResult.NotRunning);
        start:
        ICompileTimeValue? value = null;
        switch (currentFrame.Instruction!.Id)
        {
            case InstructionId.Add:
                if (currentFrame.Instruction.Values is not [Number n1, Number n2])
                    throw new WtfException();
                currentFrame.AddLocal(value = new Number(Extensions.Max(n1.Type, n2.Type), n1.Value + n2.Value));
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
                // explicit break here, do nothing
                break;
            case InstructionId.Call:
                if (_frames.Count >= MaxCallDepth)
                    throw new InvalidOperationException("Call limit overflow!");
                ClosedMethod called = ((CallInstruction)currentFrame.Instruction).Target;
                List<Local> args = new(called.Parameters.Count);
                currentFrame._locals.Pop(CollectionsMarshal.AsSpan(args));
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
            case InstructionId.Cpobj:
                break;
            case InstructionId.Div:
                break;
            case InstructionId.Dup:
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
            case InstructionId.Ldobj:
                break;
            case InstructionId.Ldvirtftn:
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
                // we ignore nops
                currentFrame.Instruction = currentFrame.Instruction.Next;
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
                if (currentFrame.Method.ReturnType.Kind == ClosedTypeKind.Void ?
                        currentFrame.Instruction.Values.Count > 0 :
                        currentFrame.Instruction.Values.Count == 0)
                    throw new WtfException("Invalid return kind.");
                value = currentFrame._locals.Pop();
                _ = _frames.Pop();
                if (_frames.TryPeek(out currentFrame))
                    currentFrame.AddLocal(value);
                break;
            case InstructionId.Rethrow:
                break;
            case InstructionId.Shl:
                break;
            case InstructionId.Shr:
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
    public CurrentStep Run()
    {
        CurrentStep step;
        while ((step = Step()).Result != StepResult.Ok)
        {
        }
        return step;
    }
}
