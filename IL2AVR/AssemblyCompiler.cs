using System.Collections.Generic;
using System.IO;
namespace IL2AVR;

public readonly struct AssemblyCompiler : IEmitter
{
    public bool Emit(IReadOnlyList<CompiledMethod> methods, Stream stream)
    {
        throw new System.NotImplementedException();
    }
}
