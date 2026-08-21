using System.Collections.Generic;
namespace IL2AVR.IR;

public sealed class TextSystem(ClosedTypeSystem typeSystem)
{
    private readonly Dictionary<string, Text> _texts = [];
    public Text this[string text] => _texts.GetOrAdd(text, new(typeSystem, text));
}
