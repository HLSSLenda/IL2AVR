using IL2AVR.IR;
using Mono.Cecil;
using Mono.Cecil.Cil;
using Mono.Collections.Generic;
using System;
using System.Collections.Generic;
using System.Linq;
namespace IL2AVR;

public sealed class ClosedMethodBody
{
    public readonly MethodBody UnderlyingBody;
    public readonly IReadOnlyList<ClosedVariable> Variables;
    public readonly IReadOnlyList<ClosedMethodBasicBlock> Blocks;
    public IntermediateInstruction EntryInstruction => Blocks[0].Instructions[0];
    public IEnumerable<IntermediateInstruction> Instructions => Blocks.SelectMany(static x => x.Instructions);
    public readonly bool IsCompileTimeEvaluable;

    internal ClosedMethodBody(CompilerBag bag, ClosedMethod declaringMethod, ClosedTypeSystem typeSystem, bool isInline)
    {
        UnderlyingBody = declaringMethod.RawBody;
        Variables = [.. declaringMethod.RawBody.Variables.Select(variable => new ClosedVariable(variable, declaringMethod.GenericArguments))];

        Instruction currentLeader = declaringMethod.RawBody.Instructions[0];
        Stack<IValue> cilStack = [];
        // variable/local references across the entire method
        Dictionary<int, ClosedVariable> vars = [];
        InstructionStream instructions = new();
        NumberSystem intNumbers = new(typeSystem.Int32);
        NumberSystem longNumbers = new(typeSystem.Int64);
        TextSystem texts = new(typeSystem);
        for (int i = 0; i < declaringMethod.RawBody.Instructions.Count; i++)
        {
            Instruction instruction = declaringMethod.RawBody.Instructions[i];
            switch (instruction.OpCode.Code)
            {
                case Code.Nop:
                    instructions.Add(new(InstructionId.Nop));
                    break;
                case Code.Break:
                    instructions.Add(new(InstructionId.Break));
                    break;
                case Code.Ldarg_0:
                    cilStack.Push(new Arg(declaringMethod.Parameters[0]));
                    break;
                case Code.Ldarg_1:
                    cilStack.Push(new Arg(declaringMethod.Parameters[1]));
                    break;
                case Code.Ldarg_2:
                    cilStack.Push(new Arg(declaringMethod.Parameters[2]));
                    break;
                case Code.Ldarg_3:
                    cilStack.Push(new Arg(declaringMethod.Parameters[3]));
                    break;
                case Code.Ldloc_0:
                    break;
                case Code.Ldloc_1:
                    break;
                case Code.Ldloc_2:
                    break;
                case Code.Ldloc_3:
                    break;
                case Code.Stloc_0:
                    break;
                case Code.Stloc_1:
                    break;
                case Code.Stloc_2:
                    break;
                case Code.Stloc_3:
                    break;
                case Code.Ldarg_S:
                    cilStack.Push(new Arg(new((ParameterDefinition)instruction.Operand)));
                    break;
                case Code.Ldarga_S:
                    break;
                case Code.Starg_S:
                    break;
                case Code.Ldloc_S:
                    break;
                case Code.Ldloca_S:
                    break;
                case Code.Stloc_S:
                    break;
                case Code.Ldnull:
                    cilStack.Push(new Null(typeSystem.Object));
                    break;
                case Code.Ldc_I4_M1:
                    cilStack.Push(intNumbers[unchecked((ulong)-1)]);
                    break;
                case Code.Ldc_I4_0:
                    cilStack.Push(intNumbers[0]);
                    break;
                case Code.Ldc_I4_1:
                    cilStack.Push(intNumbers[1]);
                    break;
                case Code.Ldc_I4_2:
                    cilStack.Push(intNumbers[2]);
                    break;
                case Code.Ldc_I4_3:
                    cilStack.Push(intNumbers[3]);
                    break;
                case Code.Ldc_I4_4:
                    cilStack.Push(intNumbers[4]);
                    break;
                case Code.Ldc_I4_5:
                    cilStack.Push(intNumbers[5]);
                    break;
                case Code.Ldc_I4_6:
                    cilStack.Push(intNumbers[6]);
                    break;
                case Code.Ldc_I4_7:
                    cilStack.Push(intNumbers[7]);
                    break;
                case Code.Ldc_I4_8:
                    cilStack.Push(intNumbers[8]);
                    break;
                case Code.Ldc_I4_S:
                    cilStack.Push(intNumbers[(ulong)(sbyte)instruction.Operand]);
                    break;
                case Code.Ldc_I4:
                    cilStack.Push(intNumbers[(ulong)(int)instruction.Operand]);
                    break;
                case Code.Ldc_I8:
                    cilStack.Push(longNumbers[(ulong)(long)instruction.Operand]);
                    break;
                case Code.Ldc_R4:
                    cilStack.Push(new Number(typeSystem.Single, ((float)instruction.Operand).Reinterpret<float, ulong>()));
                    break;
                case Code.Ldc_R8:
                    cilStack.Push(new Number(typeSystem.Double, ((double)instruction.Operand).Reinterpret<double, ulong>()));
                    break;
                case Code.Dup:
                    cilStack.Push(cilStack.Peek());
                    break;
                case Code.Pop:
                    _ = cilStack.Pop();
                    break;
                case Code.Jmp:
                    // Although it makes sense to ret from a inlined method, because we can just replace it with nothing (if the called method is void) or put the CIL result directly into the evaluation plane (if the called method returns a value), a jmp doesn't make sense because it is basically a glorified ret that uses the same stackframe of the caller.
                    if (isInline)
                        throw new NotSupportedException();
                    // TODO: 
                    break;
                case Code.Call:
                    MethodReference method = (MethodReference)instruction.Operand;
                    
                    break;
                case Code.Calli:
                    CallSite callSite = (CallSite)instruction.Operand;
                    // prepare callsite
                    // ...
                    break;
                case Code.Ret:
                    instructions.Add(new(InstructionId.Ret,
                        Values: declaringMethod.ReturnType.Kind == ClosedTypeKind.Void ? [] : [cilStack.Pop()]));
                    break;
                case Code.Br_S:
                    break;
                case Code.Brfalse_S:
                    break;
                case Code.Brtrue_S:
                    break;
                case Code.Beq_S:
                    break;
                case Code.Bge_S:
                    break;
                case Code.Bgt_S:
                    break;
                case Code.Ble_S:
                    break;
                case Code.Blt_S:
                    break;
                case Code.Bne_Un_S:
                    break;
                case Code.Bge_Un_S:
                    break;
                case Code.Bgt_Un_S:
                    break;
                case Code.Ble_Un_S:
                    break;
                case Code.Blt_Un_S:
                    break;
                case Code.Br:
                    break;
                case Code.Brfalse:
                    break;
                case Code.Brtrue:
                    break;
                case Code.Beq:
                    break;
                case Code.Bge:
                    break;
                case Code.Bgt:
                    break;
                case Code.Ble:
                    break;
                case Code.Blt:
                    break;
                case Code.Bne_Un:
                    break;
                case Code.Bge_Un:
                    break;
                case Code.Bgt_Un:
                    break;
                case Code.Ble_Un:
                    break;
                case Code.Blt_Un:
                    break;
                case Code.Switch:
                    break;
                case Code.Ldind_I1:
                    break;
                case Code.Ldind_U1:
                    break;
                case Code.Ldind_I2:
                    break;
                case Code.Ldind_U2:
                    break;
                case Code.Ldind_I4:
                    break;
                case Code.Ldind_U4:
                    break;
                case Code.Ldind_I8:
                    break;
                case Code.Ldind_I:
                    break;
                case Code.Ldind_R4:
                    break;
                case Code.Ldind_R8:
                    break;
                case Code.Ldind_Ref:
                    break;
                case Code.Stind_Ref:
                    break;
                case Code.Stind_I1:
                    break;
                case Code.Stind_I2:
                    break;
                case Code.Stind_I4:
                    break;
                case Code.Stind_I8:
                    break;
                case Code.Stind_R4:
                    break;
                case Code.Stind_R8:
                    break;
                case Code.Add:
                    if (!cilStack.TryPop(out IValue? v1) || !cilStack.TryPop(out IValue? v2))
                        throw new WtfException();
                    Number? n1 = v1 as Number, n2 = v2 as Number;
                    if (n1 is null)
                    {
                        if (n2 is null)
                            instructions.Add(new Add(instructions.Count, v1, v2));
                    }
                    break;
                case Code.Sub:
                    break;
                case Code.Mul:
                    break;
                case Code.Div:
                    break;
                case Code.Div_Un:
                    break;
                case Code.Rem:
                    break;
                case Code.Rem_Un:
                    break;
                case Code.And:
                    break;
                case Code.Or:
                    break;
                case Code.Xor:
                    break;
                case Code.Shl:
                    break;
                case Code.Shr:
                    break;
                case Code.Shr_Un:
                    break;
                case Code.Neg:
                    break;
                case Code.Not:
                    break;
                case Code.Conv_I1:
                    Conv(cilStack, typeSystem, typeSystem.SByte);
                    break;
                case Code.Conv_I2:
                    Conv(cilStack, typeSystem, typeSystem.Int16);
                    break;
                case Code.Conv_I4:
                    Conv(cilStack, typeSystem, typeSystem.Int32);
                    break;
                case Code.Conv_I8:
                    Conv(cilStack, typeSystem, typeSystem.Int64);
                    break;
                case Code.Conv_R4:
                    Conv(cilStack, typeSystem, typeSystem.Single);
                    break;
                case Code.Conv_R8:
                    Conv(cilStack, typeSystem, typeSystem.Double);
                    break;
                case Code.Conv_U4:
                    Conv(cilStack, typeSystem, typeSystem.UInt32);
                    break;
                case Code.Conv_U8:
                    Conv(cilStack, typeSystem, typeSystem.UInt64);
                    break;
                case Code.Callvirt:
                    break;
                case Code.Cpobj:
                    break;
                case Code.Ldobj:
                    break;
                case Code.Ldstr:
                    cilStack.Push(texts[(string)instruction.Operand]);
                    break;
                case Code.Newobj:
                    break;
                case Code.Castclass:
                    break;
                case Code.Isinst:
                    break;
                case Code.Conv_R_Un:
                    break;
                case Code.Unbox:
                    break;
                case Code.Throw:
                    break;
                case Code.Ldfld:
                    break;
                case Code.Ldflda:
                    break;
                case Code.Stfld:
                    break;
                case Code.Ldsfld:
                    break;
                case Code.Ldsflda:
                    break;
                case Code.Stsfld:
                    break;
                case Code.Stobj:
                    break;
                case Code.Conv_Ovf_I1_Un:
                    break;
                case Code.Conv_Ovf_I2_Un:
                    break;
                case Code.Conv_Ovf_I4_Un:
                    break;
                case Code.Conv_Ovf_I8_Un:
                    break;
                case Code.Conv_Ovf_U1_Un:
                    break;
                case Code.Conv_Ovf_U2_Un:
                    break;
                case Code.Conv_Ovf_U4_Un:
                    break;
                case Code.Conv_Ovf_U8_Un:
                    break;
                case Code.Conv_Ovf_I_Un:
                    break;
                case Code.Conv_Ovf_U_Un:
                    break;
                case Code.Box:
                    break;
                case Code.Newarr:
                    instructions.Add(new CallInstruction(bag.AllocateArray.Close(new Dictionary<GenericParameter, IClosedType>(1)
                    {
                        { bag.AllocateArray.GenericParameters[0], ((TypeReference)instruction.Operand).Close() }
                    }), Values: [cilStack.Pop()]));
                    break;
                case Code.Ldlen:
                    break;
                case Code.Ldelema:
                    break;
                case Code.Ldelem_I1:
                    break;
                case Code.Ldelem_U1:
                    break;
                case Code.Ldelem_I2:
                    break;
                case Code.Ldelem_U2:
                    break;
                case Code.Ldelem_I4:
                    break;
                case Code.Ldelem_U4:
                    break;
                case Code.Ldelem_I8:
                    break;
                case Code.Ldelem_I:
                    break;
                case Code.Ldelem_R4:
                    break;
                case Code.Ldelem_R8:
                    break;
                case Code.Ldelem_Ref:
                    break;
                case Code.Stelem_I:
                    break;
                case Code.Stelem_I1:
                    break;
                case Code.Stelem_I2:
                    break;
                case Code.Stelem_I4:
                    break;
                case Code.Stelem_I8:
                    break;
                case Code.Stelem_R4:
                    break;
                case Code.Stelem_R8:
                    break;
                case Code.Stelem_Ref:
                    break;
                case Code.Ldelem_Any:
                    break;
                case Code.Stelem_Any:
                    break;
                case Code.Unbox_Any:
                    break;
                case Code.Conv_Ovf_I1:
                    break;
                case Code.Conv_Ovf_U1:
                    break;
                case Code.Conv_Ovf_I2:
                    break;
                case Code.Conv_Ovf_U2:
                    break;
                case Code.Conv_Ovf_I4:
                    break;
                case Code.Conv_Ovf_U4:
                    break;
                case Code.Conv_Ovf_I8:
                    break;
                case Code.Conv_Ovf_U8:
                    break;
                case Code.Refanyval:
                    break;
                case Code.Ckfinite:
                    break;
                case Code.Mkrefany:
                    break;
                case Code.Ldtoken:
                    if (instruction.Operand is not TypeReference type)
                        throw new NotSupportedException();
                    cilStack.Push(RuntimeTypeHandle.Get(type.InstantiateGeneric(declaringMethod.GenericArguments)));
                    break;
                case Code.Conv_U2:
                    Conv(cilStack, typeSystem, typeSystem.UInt16);
                    break;
                case Code.Conv_U1:
                    Conv(cilStack, typeSystem, typeSystem.Byte);
                    break;
                case Code.Conv_I:
                    break;
                case Code.Conv_Ovf_I:
                    break;
                case Code.Conv_Ovf_U:
                    break;
                case Code.Add_Ovf:
                    break;
                case Code.Add_Ovf_Un:
                    break;
                case Code.Mul_Ovf:
                    break;
                case Code.Mul_Ovf_Un:
                    break;
                case Code.Sub_Ovf:
                    break;
                case Code.Sub_Ovf_Un:
                    break;
                case Code.Endfinally:
                    break;
                case Code.Leave:
                    break;
                case Code.Leave_S:
                    break;
                case Code.Stind_I:
                    break;
                case Code.Conv_U:
                    break;
                case Code.Arglist:
                    break;
                case Code.Ceq:
                    break;
                case Code.Cgt:
                    break;
                case Code.Cgt_Un:
                    break;
                case Code.Clt:
                    break;
                case Code.Clt_Un:
                    break;
                case Code.Ldftn:
                    break;
                case Code.Ldvirtftn:
                    break;
                case Code.Ldarg:
                    break;
                case Code.Ldarga:
                    break;
                case Code.Starg:
                    break;
                case Code.Ldloc:
                    break;
                case Code.Ldloca:
                    break;
                case Code.Stloc:
                    break;
                case Code.Localloc:
                    if (!cilStack.TryPeek(out IValue? v))
                        throw new WtfException();
                    // constant localloc are already allocated at the stack frame
                    if (v is not Number)
                    {
                        // TODO
                    }
                    break;
                case Code.Endfilter:
                    break;
                case Code.Unaligned:
                    // No-op on purpose! AVR doesn't care about alignment.
                    break;
                case Code.Volatile:
                    break;
                case Code.Tail:
                    break;
                case Code.Initobj:
                    break;
                case Code.Constrained:
                    break;
                case Code.Cpblk:
                    break;
                case Code.Initblk:
                    break;
                case Code.No:
                    break;
                case Code.Rethrow:
                    break;
                case Code.Sizeof:
                    cilStack.Push(intNumbers[((TypeReference)instruction.Operand).Close().Size]);
                    break;
                case Code.Refanytype:
                    break;
                case Code.Readonly:
                    break;
/*
#region Control flow
                case Code.Beq:
                case Code.Beq_S:
                case Code.Bge:
                case Code.Bge_S:
                case Code.Bge_Un:
                case Code.Bge_Un_S:
                case Code.Bgt:
                case Code.Bgt_S:
                case Code.Bgt_Un:
                case Code.Bgt_Un_S:
                case Code.Ble:
                case Code.Ble_S:
                case Code.Ble_Un:
                case Code.Ble_Un_S:
                case Code.Blt:
                case Code.Blt_Un_S:
                case Code.Bne_Un:
                case Code.Bne_Un_S:
                case Code.Br:
                case Code.Br_S:
                case Code.Brfalse:
                case Code.Brfalse_S:
                case Code.Brtrue:
                case Code.Brtrue_S:
                    Instruction target = (Instruction)instruction.Operand;
                    if (target.Offset < i) {
                        if (!blocks.TryGetValue(target, out ClosedMethodBlock? block))
                        {
                            ClosedMethodBlock nearestLeader = blocks[blocks.Keys.Where(instruction => instruction.Offset <= i).OrderByDescending(static instruction => instruction.Offset).First()];
                            ClosedMethodBlockKind newLeaderToKind = nearestLeader.ToKind;
                            Instruction newLeaderEnd = nearestLeader.End;
                            nearestLeader.ToKind = ClosedMethodBlockKind.Entry;
                            nearestLeader.End = target.Offset == 0 ? target : target.Previous;
                            block = nearestLeader.To = blocks[target] = new(newLeaderToKind, target, newLeaderEnd, nearestLeader.To, nearestLeader.VariableReferences);
                        }
                        blocks[currentLeader] = new(ClosedMethodBlockKind.Branch, currentLeader, instruction, block, null);
                    }
                    else
                    {
                        blocks[currentLeader] = new(ClosedMethodBlockKind.Branch, currentLeader, instruction, to: null, null);
                    }
                    currentLeader = instruction.Next!;
                    break;

                case Code.Jmp:
                case Code.Ret:
                    blocks[currentLeader] = new(ClosedMethodBlockKind.Return, currentLeader, instruction, to: null, null);
                    currentLeader = instruction.Next!;
                    break;

                case Code.Throw:
                    blocks[currentLeader] = new(ClosedMethodBlockKind.Throw, currentLeader, instruction, to: null, null);
                    break;
#endregion
                // Variable references
                case Code.Ldloc:
                case Code.Ldloc_S:
                case Code.Ldloca:
                case Code.Ldloca_S:
                case Code.Stloc:
                case Code.Stloc_S:
                    vars[i] = Variables[((VariableReference)instruction.Operand).Index];
                    break;
                case Code.Ldloc_0:
                case Code.Stloc_0:
                    vars[i] = Variables[0];
                    break;
                case Code.Ldloc_1:
                case Code.Stloc_1:
                    vars[i] = Variables[1];
                    break;
                case Code.Ldloc_2:
                case Code.Stloc_2:
                    vars[i] = Variables[2];
                    break;
                case Code.Ldloc_3:
                case Code.Stloc_3:
                    vars[i] = Variables[3];
                    break;
                */
            }
        }
        foreach (ClosedMethodBlock block in blocks.Values)
            if (block.ToKind == ClosedMethodBlockKind.Branch && block.To is null)
                block.To = blocks[block.End];
    }
}
