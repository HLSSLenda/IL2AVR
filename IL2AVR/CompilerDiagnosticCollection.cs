using Mono.Cecil;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
namespace IL2AVR;

public readonly ref struct CompilerDiagnosticCollection(
    CompilerDiagnosticKind logLevel = CompilerDiagnosticKind.Fatal | CompilerDiagnosticKind.Error | CompilerDiagnosticKind.Warning,
    bool warnAsError = false
) : IReadOnlyCollection<CompilerDiagnostic>
{
    private readonly List<CompilerDiagnostic>? _fatals = (logLevel & CompilerDiagnosticKind.Fatal) != 0 ? [] : null;
    public IReadOnlyCollection<CompilerDiagnostic>? Fatals => _fatals;
    public void Fatal(ModuleDefinition module, CompilerDiagnosticId id, ClosedType? type = null, ClosedMethod? method = null, params object[] context) => _fatals?.Add(new(CompilerDiagnosticKind.Fatal, module, type, method, id, context));

    private readonly List<CompilerDiagnostic>? _errors = (logLevel & CompilerDiagnosticKind.Error) != 0 ? [] : null;
    public IReadOnlyCollection<CompilerDiagnostic>? Errors => _errors;
    public void Error(ModuleDefinition module, CompilerDiagnosticId id, ClosedType? type = null, ClosedMethod? method = null, params object[] context) => _errors?.Add(new(CompilerDiagnosticKind.Error, module, type, method, id, context));

    private readonly List<CompilerDiagnostic>? _warnings = (logLevel & CompilerDiagnosticKind.Warning) != 0 ? [] : null;
    public IReadOnlyCollection<CompilerDiagnostic>? Warnings => _warnings;
    public void Warning(ModuleDefinition module, CompilerDiagnosticId id, ClosedType? type = null, ClosedMethod? method = null, params object[] context) => (warnAsError ? _errors : _warnings)?.Add(new(CompilerDiagnosticKind.Warning, module, type, method, id, context));

    private readonly List<CompilerDiagnostic>? _informations = (logLevel & CompilerDiagnosticKind.Info) != 0 ? [] : null;
    public IReadOnlyCollection<CompilerDiagnostic>? Informations => _informations;
    public void Information(ModuleDefinition module, CompilerDiagnosticId id, ClosedType? type = null, ClosedMethod? method = null, params object[] context) => _informations?.Add(new(CompilerDiagnosticKind.Info, module, type, method, id, context));

    private readonly List<CompilerDiagnostic>? _debugs = (logLevel & CompilerDiagnosticKind.Debug) != 0 ? [] : null;
    public IReadOnlyCollection<CompilerDiagnostic>? Debugs => _debugs;
    public void Debug(ModuleDefinition module, CompilerDiagnosticId id, ClosedType? type = null, ClosedMethod? method = null, params object[] context) => _debugs?.Add(new(CompilerDiagnosticKind.Debug, module, type, method, id, context));

    private static int CountCollection(IReadOnlyCollection<CompilerDiagnostic>? collection) => collection is null ? 0 : collection.Count;
    public int Count => CountCollection(_fatals) + CountCollection(_errors) + CountCollection(_warnings) + CountCollection(_informations) + CountCollection(_debugs);

    public void ThrowIfCompilerError()
    {
        if (_fatals.Count > 0 || _errors.Count > 0)
            throw new CompilerException([.. _fatals.Concat(_errors) ]);
    }

    public IEnumerator<CompilerDiagnostic> GetEnumerator()
    {
        if (_fatals is not null)
            foreach (CompilerDiagnostic diagnostic in _fatals)
                yield return diagnostic;
        if (_errors is not null)
            foreach (CompilerDiagnostic diagnostic in _errors)
                yield return diagnostic;
        if (_warnings is not null)
            foreach (CompilerDiagnostic diagnostic in _warnings)
                yield return diagnostic;
        if (_informations is not null)
            foreach (CompilerDiagnostic diagnostic in _informations)
                yield return diagnostic;
        if (_debugs is not null)
            foreach (CompilerDiagnostic diagnostic in _debugs)
                yield return diagnostic;
    }
    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
}
