using IL2AVR.OpCodes;
using Mono.Cecil;
using System.Collections.Generic;
using System.IO;
namespace IL2AVR;

public readonly struct AssemblyCompiler : IEmitter
{
    public void Compile(AVRCompiler machine, Stream stream)
    {
        StreamWriter writer = new(stream, leaveOpen: true);
        foreach (CompiledMethod info in machine.AnalyzedMethods)
        {
            writer.Write(kv.Key.FullName);
            writer.WriteLine(':');
            foreach (OpCode opcode in kv.Value)
            {
                writer.Write("    ");
                writer.WriteLine(opcode.ToString());
            }
        }
    }
}
