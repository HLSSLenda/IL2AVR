using IL2AVR.IR;
using Mono.Cecil;
using System.Collections.Generic;
namespace IL2AVR;

// TODO: Fix InlineMode to be able to reference locations.

/// <summary>Contains every compiler option.</summary>
/// <param name="BreakBehavior">
/// Sets how the compiler will handle `break` CIL instruction.
/// Equivalent CLI option: None.
/// </param>
/// <param name="CompileTimeValueDefinitions">
/// Sets compile-time value definitions for the target assembly to use.
/// Equivalent CLI option: `--define &lt;type&gt; &lt;name&gt; &lt;value&gt;`.
/// </param>
/// <param name="EnableDefaultIntrinsics">
/// Enables the default intrinsics of the compiler. If you disable it, some AVR-specific things or impossible-to-express C# code won't be implemented.
/// Equivalent CLI option: None.
/// </param>
/// <param name="IgnoreNops">
/// If <see langword="true" />, the compiler ignores every `nop` CIL instruction.
/// Equivalent CLI option: None.
/// </param>
/// 
public record CompilerOptions(
    BreakBehavior BreakBehavior = BreakBehavior.Break,
    IReadOnlyDictionary<string, CompileTimeValueDefinition>? CompileTimeValueDefinitions = null,
    bool EnableDefaultIntrinsics = true,
    bool IgnoreNops = false,
    InlineMode InlineMode = InlineMode.AgressiveInlining,
    IReadOnlyDictionary<MethodReference, ExternalImplementation>? Intrinsics = null,
    short MaxReadonlyInliningSize = 8,
    bool WarningAsError = false
);
