using Mono.Cecil;
namespace IL2AVR;

public readonly record struct CompilerDiagnostic(CompilerDiagnosticKind Kind, ModuleDefinition Module, IClosedEntity Entity, CompilerDiagnosticId Id, params object[] Context)
{
    public override string ToString() => Id switch
    {
        CompilerDiagnosticId.ENoEntryPoint => "Module doesn't have an entry point.",
        CompilerDiagnosticId.ENoMethodImplementation => "Method has no IL body or an intrinsic definition.",
        CompilerDiagnosticId.EUnsupportedProcessorOpCode => $"Target {Context[0]} doesn't support {Context[1]}.",
        _ => throw new WtfException()
    };
}
