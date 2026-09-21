using Mono.Cecil;
namespace IL2AVR;

public readonly record struct CompilerDiagnostic(CompilerDiagnosticKind Kind, ModuleDefinition Module, IClosedEntity Entity, CompilerDiagnosticId Id, params object[] Context)
{
    public override string ToString() => Id switch
    {
        CompilerDiagnosticId.EExternEntryPoint => "Entry point doesn't have an IL body.",
        CompilerDiagnosticId.ENoEntryPoint => "Module doesn't have an entry point.",
        CompilerDiagnosticId.ENoImplementation => "Method has no IL body or an intrinsic definition.",
        CompilerDiagnosticId.EUnsupportedProcessorOpCode => $"Target {Context[0]} doesn't support {Context[1]}.",
        _ => throw new WtfException()
    };
}
