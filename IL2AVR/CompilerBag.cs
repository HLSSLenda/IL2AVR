using IL2AVR.IR;
using Mono.Cecil;
using System.Collections.Generic;
using System.Linq;
namespace IL2AVR;

/// <summary>A transient toolbox (with its lifetime defined inside the compiler invocation) contains objects and tools useful for the compiler.</summary>
/// <seealso cref="Compiler" />
public ref struct CompilerBag
{
    /// <summary>The logger used for the compiler.</summary>
    public readonly ILogger Logger;
    /// <summary>The diagnostics collection.</summary>
    public readonly CompilerDiagnosticCollection Diagnostics;
    public readonly CompilerOptions Options;

    public readonly ClosedTypeSystem TypeSystem;
    public readonly TypeDefinition AllocateFromAttribute;
    public readonly TypeDefinition ConstantExpectedAttribute;
    public readonly TypeDefinition DoesNotReturnAttribute;
    public readonly TypeDefinition InlineArrayAttribute;
    public readonly TypeDefinition InterruptAttribute;
    public readonly TypeDefinition NakedAttribute;
    public readonly TypeDefinition PureAttribute;
    public readonly TypeDefinition TypeAsAttribute;
    public readonly TypeDefinition ValueFromCompilerInvocationAttribute;
    public readonly MethodDefinition Alloc;
    public readonly MethodDefinition AllocateArray;
    public readonly MethodDefinition Finalize;

    public readonly Dictionary<MethodReference, ExternalImplementation> DefaultIntrinsics;

    public CompilerBag(ILogger logger, CompilerOptions options, ModuleDefinition mainModule)
    {
        Diagnostics = new(warnAsError: options.WarningAsError);
        Logger = logger;
        Options = options;

        ClosedTypeSystem ts = TypeSystem = new(mainModule.TypeSystem);
        ModuleDefinition corelib = ts.Object.Module;

        AllocateFromAttribute = corelib.GetType("System.Runtime.CompilerServices.IL2AVR.AllocateFromAttribute");
        ConstantExpectedAttribute = corelib.GetType("System.Diagnostics.CodeAnalysis.ConstantExpectedAttribute");
        DoesNotReturnAttribute = corelib.GetType("System.Diagnostics.CodeAnalysis.DoesNotReturnAttribute");
        InlineArrayAttribute = corelib.GetType("System.Runtime.CompilerServices.InlineArrayAttribute");
        InterruptAttribute = corelib.GetType("System.Runtime.InterruptAttribute");
        NakedAttribute = corelib.GetType("System.Runtime.CompilerServices.IL2AVR.NakedAttribute");
        PureAttribute = corelib.GetType("System.Diagnostics.Contracts.PureAttribute");
        TypeAsAttribute = corelib.GetType("System.Runtime.CompilerServices.IL2AVR.TypeAsAttribute");
        ValueFromCompilerInvocationAttribute = corelib.GetType("System.Runtime.CompilerServices.IL2AVR.ValueFromCompilerInvocationAttribute");
        // Allow the user to override a memory allocator fully (they can also define System.Runtime.CompilerServices.IL2AVR.INativeMemoryAllocator.Default through compiler options)
        Alloc = (mainModule.GetType("System.Runtime.InteropServices.NativeMemory") ?? corelib.GetType("System.Runtime.InteropServices.NativeMemory"))
            .GetMethod(new PointerType(ts.Void.UnderlyingType), "Alloc", 0, ts.UIntPtr.UnderlyingType)!;
        AllocateArray = corelib.GetType("System.GC").Methods.First(method => method.Name == "AllocateArray" && method.GenericParameters.Count == 1 && method.Parameters.Count == 1 && method.Parameters[0].ParameterType == ts.Int32.UnderlyingType);
        Finalize = ts.Object.UnderlyingType.GetMethod(ts.Object.UnderlyingType, "Finalize")!;

        TypeDefinition opcodeIdType = corelib.GetType("System.Runtime.CompilerServices.IL2AVR.OpCodeId");
        TypeDefinition unsafeType = corelib.GetType("System.Runtime.CompilerServices.Unsafe");
        TypeDefinition assemblyType = corelib.GetType("System.Runtime.CompilerServices.IL2AVR.Assembly");
        DefaultIntrinsics = new()
        {
            {
                assemblyType.GetMethod(ts.Void.UnderlyingType, "Asm", 0, opcodeIdType)!,
                (in compilerBag, isInline, callGraph, instructionStream, args) =>
                {
                    if (!isInline)
                        compilerBag.Diagnostics.Error();
                    instructionStream.Add(new Asm(0, new((OpCodeId)((Number)args[0]).Value)));
                }
            },

            {
                unsafeType.Methods.First(static method => method.Name == "As" && method.GenericParameters.Count == 2 && method.Parameters.Count == 1 && method.Parameters[0].ParameterType.IsByReference),
                (callStack, args) => []
            },

            {
                unsafeType.Methods.First(static method => method.Name == "AsRef" && method.GenericParameters.Count == 1 && method.Parameters.Count == 1 && method.Parameters[0].ParameterType.IsPointer),
                (callStack, args) => []
            },

            {
                unsafeType.GetMethod(ts.Int32.UnderlyingType, "SizeOf", 1)!,
                (callStack, args) =>
                [
                    new Ret(new Number(ts.UInt16, callStack[^1].GenericArguments.Values.First().Size))
                ]
            }
        };

    }
}
