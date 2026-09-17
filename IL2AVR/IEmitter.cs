using System.Collections.Generic;
using System.IO;
namespace IL2AVR;

public interface IEmitter
{
    bool Emit(IReadOnlyList<CompiledMethod> methods, Stream stream);
}