using IL2AVR;
using IL2AVR.Processors;
using Mono.Cecil;
using System;
using System.Collections.Generic;

static bool StartsWithAtLeast(ReadOnlySpan<char> x, ReadOnlySpan<char> y, int n = 1)
{
    if (!x[..n].CompareTo(y[..n], StringComparison.OrdinalIgnoreCase))
        return false;

}

// args: module processor
if (args.Length < 2)
    return 1;
ModuleDefinition module = ModuleDefinition.ReadModule(args[0]);
if (module.EntryPoint is null)
{
    Console.Error.WriteLine("Entry point not found!");
    return 1;
}

IProcessor processor;
switch (args[1].ToLowerInvariant())
{
    case "atmega328p":
        processor = new ATMega328P();
        break;
    default:
        return 2;
};

IEmitter? compiler = null;
bool inline = true;
Dictionary<string, CompileTimeValueDefinition> definitions = [];
ReadOnlySpan<string> remainingArgs = new(args, 2, args.Length - 2);
for (int i = 0; i < remainingArgs.Length; ++i)
{
    switch (args[i])
    {
        case "--def":
        case "--define":
            if (i + 3 >= remainingArgs.Length)
                return 1;
            CompileTimeValueType type = args[++i] switch
            {
                "bool" or "Boolean" or "System.Boolean" => CompileTimeValueType.Bool,
                _ => throw new CompilerException("")
            };
            string name = args[++i];
            switch (type)
            {
                case CompileTimeValueType.Bool:
                    ReadOnlySpan<char> value = args[++i].AsSpan().Trim();
                    if (value == "0" || value.CompareTo("n", StringComparison.OrdinalIgnoreCase) || value.CompareTo("no", StringComparison.OrdinalIgnoreCase) || value.CompareTo("f", value.
            }
            break;
        case "--no-inline":
            inline = false;
            break;
    }
}

ICollection<CompiledMethod> methods = compiler.Compile(new(Inline: inline));
return 0;
