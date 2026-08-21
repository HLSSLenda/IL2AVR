using Mono.Cecil;
namespace IL2AVR;

public readonly record struct CompilerDiagnostic(CompilerDiagnosticKind Kind, ModuleDefinition Module, ClosedType? Type, ClosedMethod? Method, CompilerDiagnosticId Id, params object[] Context)
{
    public override string ToString() => Id switch
    {
        CompilerDiagnosticId.ExternEntryPoint => "Entry point doesn't have an IL body.",
        CompilerDiagnosticId.NoEntryPoint => "Module doesn't have an entry point.",
        CompilerDiagnosticId.NoImplementation => "Method has no IL body or an intrinsic definition.",
        CompilerDiagnosticId.UnsupportedProcessorOpCode => $"Target {Context[0]} doesn't support {Context[1]}.",
        _ => throw new WtfException()
    };
}
