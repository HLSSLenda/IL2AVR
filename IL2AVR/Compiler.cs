using IL2AVR.IR;
using Mono.Cecil;
using Mono.Cecil.Cil;
using System;
using System.Collections.Generic;
using System.Linq;
namespace IL2AVR;

public sealed class Compiler
{
    private readonly HashSet<ClosedMethod> _compiledMethods = [];

    public Compiler(ILogger logger, CompilerOptions options, ModuleDefinition module)
    {
        if (module.EntryPoint is null)
            throw new ArgumentException("Module must have an entry point.");
        if (!module.EntryPoint.HasBody)
            throw new ArgumentException("Entry point must have a body.");
 
        HashSet<ClosedType> sizeoffed = [];
        Stack<ClosedMethod> pending = new();
        HashSet<ClosedMethod> visitedMethods = [];
        pending.Push(new(module.EntryPoint, EmptyDictionary<GenericParameter, ClosedType>.Instance));
        while (pending.TryPop(out ClosedMethod? method))
        {
            if (!visitedMethods.Add(method))
                continue;
            if ((!method.HasBody || !method.UnderlyingMethod.IsIL))
                throw new CompilerException($"Method {method} doesn't have an implementation.");

            MethodKind kind;
            if ((method.UnderlyingMethod.Attributes & (MethodAttributes.SpecialName | MethodAttributes.RTSpecialName)) != 0 &&
                    (method.UnderlyingMethod.Name == ".ctor" || method.UnderlyingMethod.Name == ".cctor"))
                kind = MethodKind.Constructor;
            else if (method.UnderlyingMethod.Name == "Finalize" &&
                    method.UnderlyingMethod.Overrides.Count > 0 &&
                    method.UnderlyingMethod.Overrides.Any(ovr => ovr.MetadataToken == fuck) &&
                    method.UnderlyingMethod.IsNewSlot)
                kind = MethodKind.Finalizer;
            else
                kind = MethodKind.Normal;
            
            bool canInline;
            if ((method.UnderlyingMethod.ImplAttributes & MethodImplAttributes.NoInlining) != 0)
                canInline = false;
            else if ((method.UnderlyingMethod.ImplAttributes & MethodImplAttributes.AggressiveInlining) != 0)
                canInline = true;
            else
            {
                // TODO: It is a little crude to assume that a method can be inlined only by its instruction count, since some instruction can break havoc in the actual inlining process, but hey, this is a prototype and the prototype status should be respected.
                canInline = method.RawBody.Instructions.Count <= 2;
            }
            foreach (Instruction instruction in method.RawBody.Instructions)
            {
                switch (instruction.OpCode.Code)
                {
                    case Code.Call:
                    case Code.Callvirt:
                        MethodReference called = (MethodReference)instruction.Operand;
                        if (method == called)
                            canInline = false;
                        pending.Push(new(called, method.GenericArguments));
                        break;
                    case Code.Ldftn:
                        pending.Push(new((MethodReference)instruction.Operand, method.GenericArguments));
                        break;
                    case Code.Jmp:
                        canInline = false;
                        break;
                    case Code.Ldfld:
                    case Code.Ldflda:
                        FieldReference rawField = (FieldReference)instruction.Operand;
                        ((ClosedType)rawField.DeclaringType.Close(method.GenericArguments)).AddField(rawField);
                        break;
                    case Code.Stfld:
                        if (kind != MethodKind.Constructor)
                            ((FieldReference)instruction.Operand).DeclaringType.InstantiateGeneric(method.GenericArguments).IsImmutable = false;
                        break;
                    case Code.Sizeof:
                        sizeoffed.Add(((TypeReference)instruction.Operand).Close());
                        break;
                }
            }
        }
        CompilerBag bag = new(logger, options, module);
    }

    
}
