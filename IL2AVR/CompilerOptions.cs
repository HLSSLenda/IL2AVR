using IL2AVR.IR;
using Mono.Cecil;
using System.Collections.Generic;
namespace IL2AVR;

public record CompilerOptions(
    BreakBehavior BreakBehavior = BreakBehavior.Break,
    IReadOnlyDictionary<string, CompileTimeValueDefinition>? CompileTimeValueDefinitions = null,
    bool EnableDefaultInstrinsics = true,
    bool IgnoreNops = false,
    InlineMode InlineMode = InlineMode.AgressiveInlining,
    IReadOnlyDictionary<MethodReference, Intrinsic>? Intrinsics = null,
    short MaxReadonlyInliningSize = 8,
    bool WarningAsError = false
);
