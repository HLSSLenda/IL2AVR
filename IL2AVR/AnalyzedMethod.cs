using System.Collections.Generic;
namespace IL2AVR;

public readonly record struct AnalyzedMethod(
    ClosedMethod Method, bool CanInline, IReadOnlyCollection<LifeInfo> Lives);
