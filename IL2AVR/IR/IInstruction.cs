using System.Collections.Generic;
namespace IL2AVR.IR;

public interface IInstruction 
{
    Boxed<int> Id { get; }
    IReadOnlyCollection<IValue> Values { get; }
}
