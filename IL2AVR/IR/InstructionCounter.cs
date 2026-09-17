using System.Collections.Generic;
namespace IL2AVR.IR;

internal sealed class InstructionCounter
{
    private readonly List<Wrap<int>> _instructions = [];

    public Wrap<int> GetInstruction(int at = -1)
    {
        Wrap<int> v;
        if (at < 0 || at >= _instructions.Count)
            _instructions.Add(v = new(_instructions.Count));
        else
        {
            _instructions.Insert(at, v = new(at));
            for (int i = at + 1; i < _instructions.Count; ++i)
                ++_instructions[i].Value;
        }
        return v;
    }

    public void RemoveInstruction(int at = -1)
    {
        if (at < 0)
            at = _instructions.Count - 1;
        _instructions.RemoveAt(at);
        for (int i = at; i < _instructions.Count; ++i)
            --_instructions[i].Value;
    }
}
