# IL2AVR
This project aims to compile (most) of .NET CIL bytecode to any 8-bit AVR processor.
*Please note that this project is a messy prototype rather than a finished product, and it`ll remain this way for a lifetime.*

Usage: `il2avr <processor> <module> <file> [options...]`
where:
- `<procesor>` is the target AVR processor
- `<module>` is the .NET DLL to take as input, it must:
- => have a entry point method
- => not reference .NET's `System.Private.CoreLib` because it's not compatible with IL2AVR. Instead the module should reference `System` (from here) as the BCL.
- `<file>` is the output file.

## Compiler options
`--define <type> <name> <value>`: Define a compile-time value that can be used by the code, just like `#define`s in C/C++.
`<type>` must be:
- `sbyte` or `System.SByte`: 8-bit signed value
- `short` or `System.Int16`: 16-bit signed value
- `int` or `System.Int32`: 32-bit signed value
- `long` or `System.Int64`: 64-bit signed value
- `byte` or `System.Byte`: 8-bit unsigned value
- `ushort` or `System.UInt16`: 16-bit unsigned value
- `uint` or `System.UInt32`: 32-bit unsigned value
- `ulong` or `System.UInt64`: 64-bit unsigned value
- `float` or `System.Single`: 16-bit floating point number
- `double` or `System.Double`: 32-bit floating-point value
- `char` or `System.Char`: 8-bit character
- `string` or `System.String`: 8-bit string.
- `type` or `System.Type`: A type reference.
- `ctor`: A constructed type instance.
- `method`: A method reference. Only valid for static method references.
- *`T`*`[]`: A single-dimensional array of a `<type>`.
- `object` or `System.Object`: Anything above.
`<name>` should be a static property or static method reference, or a name starting with `#`. If it's a static reference, then the value is type-checked and mapped into the code, meaning that every call to the static member will be replaced with the value. If the name starts with `#`, then the name is anonymous and is only accessible through the `System.Runtime.CompilerServices.IL2AVR.Compiler.GetValue` method.
`<value>` is a compile-time value. If the type is `ctor`, then the first value is the type to construct, then the parameter count and the remaining values are compile-time values to pass to the constructor.

`--inline <mode> [location]`: Sets how the compiler will inline code. `<mode>` can be:
- `off`: Disables any inlining attempts, even from methods that is annotated with `AgressiveInlining`. Note that if `location` is not provided or `location` is a glob expression or `location` is a class reference, the compiler will inline anyways external methods that are only emitted in a inlining context, such as `System.Runtime.CompilerServices.IL2AVR.Assembly` methods. It's an error to disable inlining explicitly of a method that's only emitted in a inlining context.

`--interrupt-exception <mode>`: Changes how the compiler handles unhandled exceptions inside interrupts. `<mode>` can be:
- `abort`: Default, aborts the entire microcontroller execution by halting, as in normal methods.
- `ignore`: Ignores the exception and instead just `reti` from the interrupt.
- `kill`: "Kills" the interrupt by inserting a guard before each interrupt's code to check if an exception had been thrown. If true, it just `reti`s from it.

`--log <level>`: Changes compiler logging. `<level>` can be `debug`, `info`, `warn`, `error` or `fatal`. Can be specified multiple times. If not specified, defaults to `--log fatal --log error --log warn`.

`--no-exceptions`: Disables exception handling code from being emitted.

`--out-asm <file>` or `--output-assembly <file>`: Outputs Assembly code in `<file>`.

`--out <file>` or `--output <file>`: Outputs machine code to `<file>`.

`--out-intel <file>` or `--output-intel <file>`: Outputs Intel code to `<file>`.

`--out-ir <file>` or `--output-ir <file>`: Outputs IL2AVR's IR in `<file>`.

`--runtime-exceptions <mode> [location]`: Sets how the compiler will handle runtime exceptions (`DivisionByZeroException`, `IndexOutOfBoundsException`, `NullReferenceException`). `<mode>` can be:
- `strict`: Force every method to handle possible runtime exceptions by making unhandled runtime exceptions as a compiler error.
- `propagate`: Force every method to handle possible runtime exceptions by propagating them as normal exceptions in the call site.
- `panic`: Call a global panic method registered in the event <<<TODO>>> and halt the program.
- `ignore`: With great powers come great resposibilities!

# Example usage
Follow this C# code:
```cs
// QUESTION: Is porting System.Device.Gpio actually beneficial?
// This Arduino library doesn't allocate memory... And it works thanks to extension methods.
using Arduino;
using System;
using System.Threading;

Pin.D2.Output(); // Pin.Output(Pin.D2)
bool led = false;
while (true)
{
    Pin.D2.Write(led); // Pin.Write(Pin.D2)
    led = !led;
    Thread.Sleep(1000);
}
```
First, you have to compile this code without .NET's default BCL, instead replacing it by the BCL inside 'System' folder.
After that, you can invoke the compiler on the DLL file!
`$ IL2AVR atmega328p Module.dll`
`$ `
After that, `avrdude` can be used to flash it.

# Internals
IL2AVR is a very complex project, ranging to 3 parts:
- Compiler: The compiler itself, which it's subdivided into a lot of parts.
- BCL: The standard .NET's BCL is not usable, so the project also contains a minimal, but familiar, implementation of the BCL that is AVR-friendly.
- Arduino: An example library using the BCL that aims to make it easy for Arduino programmers to code.

# Definitions
Most definitions on this document will be explained in C# terms for ease of reading and for its proximity with .NET CIL, unless told otherwise.
Let "simple getter" be a method/property that just returns a field's value directly.
Let "read only member" be a `readonly` field or a `private` field that isn't mutated except at construction time and that is optionally exposed only from a simple getter.
Let "immutable type" be: `bool`, `byte`, `char`, `decimal`, `double`, `float`, `int`, `long`, `nint`, `nuint`, `sbyte`, `short`, `string`, `uint`, `ulong`, `ushort` or any type that contains only read only members of immutable types.
Let "pointer type" be `void*`, `T*`, `ref T` or `ref readonly T`.
Let "fat pointer" be a pointer that, when dereferenced, emits `LD` instructions if the address is within `RAMSTART .. RAMEND`, or else emits `LPM`.
Let "pointerish type" be a pointer type, or a type that contains one or more pointer fields.
Let "compile-time representable type" be a type that can be instantiated at compile-time, that is, every C# primitive, or a type that contains every field as a compile-time representable type, doesn't call `extern` methods, and every instance method of itself calls only compile-time representable functions, enabling more optimizations.
Let "compile-time representable function" be a function that can be emulated at compile-time without depending on a real AVR runtime.
Let "function hints" be identity functions that only serve as a hint to the compiler.
Let "flash-promotable type" be a type that can't be inlined by the compiler, that is read-only and also compile-time representable, or a pointer type.
Let "flash pointer" be a pointer that emits `LPM` instructions when dereferenced.
Let "memory pointer" be a pointer that emits `LD` instructions when dereferenced.

## Compiler - Front end
The compiler itself is very big, with a lot of responsibilities. It always starts analyzing from a DLL module with a `Main()` method and it does some processes in a particular order.

### Type analysis + type trimming
Firstly, it enters the `Main` method and analyzes the code for type, properties and field references. For every type references it finds, it checks for a static constructor. If it exists, analysis follows as such:
1. It its body is empty, it simply ignores emitting any static constructor.
2. It checks the body for simple static field assignment instructions (stsfld with a simple expression such as a constant, another static field's contents respecting the invariants below or a constructor call that is simple on itself) against all these invariants, sooner or later:
- The type of the field being immutable and the type is small enough (fewer than 8 bytes);
- The field itself not being accessed by its address (ldsflda)
=> If the invariants are satisfied, then the compiler marks the field as a cloneable instance by every reference of this static field, meaning that the compiler copies the field's contents at every ldsfld and it doesn't assign an address to it.
=> If the invariants aren't satisfied, then the compiler stores the field contents into a unique address at the flash (if the type is immutable) or to the SRAM (if the type is mutable).
3. If the body doesn't have only simple assignments, then the static constructor gets transformed as a method, and the compiler marks to execute this static method before any of the Main code runs.
Do note that static constructors behaves as private methods, that means that they can't be called.

### Class and interface structure
Each class/interface has its own vtable stored in flash memory, at an address chosen by the compiler itself.
For `System.Object`, the real object layout would look like (note that `class`, `ref` and `virtual` are just for demonstration purposes):
```cs
namespace System;
public class Object
{
    internal readonly unsafe struct vtable // occupies 10 bytes
    {
        private static readonly bool ImplEquals(object self, object? other)
        {
            // ...
        }
        private static readonly void ImplFinalize(object self)
        {
            // ...
        }
        private static readonly string ImplToString(object self)
        {
            // ...
        }
        private readonly nint baseClass= 0; // 0xdeadbeef
        private readonly nint interfaceTable = 0; // 0xdeadbef1
        public readonly new delegate*<object, object?, bool> Equals = &ImplEquals; // 0xdeadbef3
        public readonly new delegate*<object, void> Finalize = &ImplFinalize; // 0xdeadbef5
        public readonly new delegate*<object, string> ToString = &ImplToString; // 0xdeadbef7
    }
    protected static unsafe vtable vtable
    {
        get
        {
            Span<byte> rawTable = stackalloc byte[sizeof(vtable)];
            LPM.Read(0xdeadbeef, rawTable);
            return MemoryMarshal.Read<vtable>(rawTable);
        }
    }
    protected virtual unsafe void Finalize() => vtable.Finalize(this);
    public virtual unsafe bool Equals(object? obj) => vtable.Equals(this, obj);
    public Type GetType()
    {
        ...
    }
    public virtual unsafe string ToString() => vtable.ToString(this);
}
```
For subclasses (such as `System.String`), the vtable contains the overriden methods in the class, and every reference to the base class is resolved directly via taking the base vtable's address. For example (shortened for brevity):
```cs
namespace System;

public sealed class String
{
    private readonly unsafe struct vtable
    {
        public static readonly bool ImplEquals(string self, object? obj)
        {
            // ...
        }
        public static readonly string ImplToString(string self)
        {
            // ...
        }
        public readonly nint baseClass = 0xdeadbeef;
        public readonly nint interfaceTable = 0;
        public readonly new delegate*<string, object?, bool> ImplEquals = &ImplEquals;
        public readonly new delegate*<string, void> Finalize = (delegate*<string, void>)&object.vtable.ImplFinalize;
        public readonly new delegate*<object, string> ToString = &ImplToString;
    }
    protected static unsafe new vtable vtable
    {
        get
        {
            Span<byte> rawTable = stackalloc byte[sizeof(vtable)];
            LPM.Read(0xdeadbef9, rawTable);
            return MemoryMarshal.Read<vtable>(rawTable);
        }
    }
    private readonly ushort length;
    private readonly char firstChar;
    public override unsafe bool Equals(object? obj) => vtable.Equals(this, obj);
    public override unsafe bool Finalize() => object.vtable.Finalize(this);
    public override unsafe string ToString() => vtable.ToString(this);
}
```

For interfaces, each interface will receive an unique ID, as classes, but it'll not have a vtable

For subclasses with interfaces (such as `System.String` again), each interface also contains its own vtable, though the vtable references the implementations directly in the class, or the default interface implementation if the interface itself contains a default implementation and the class doesn't override it. For example:
```cs
namespace System;

public interface IComparable<T> where T : allows ref struct
{
    internal const nint id = 1;
    int CompareTo(T other);
}
public interface IEquatable<T> where T : allows ref struct
{
    internal const nint id = 2;
    bool Equals(T other);
}

public sealed class String : IComparable<string>, IEquatable<string>
{
    private readonly unsafe struct vtable
    {
        public static readonly int ImplCompareTo(string self, string other)
        {
            // ...
        }
        public static readonly bool ImplEquals(object self, string other)
        {
            // ...
        }
        public static readonly bool ImplEquals(string self, string other)
        {
            // ...
        }
        public static readonly string ImplToString(string self)
        {
            // ...
        }
        public readonly nint baseClass = 0xdeadbeef;
        public readonly nint interfaceTable = 0xf0000000;
        public readonly new delegate*<string, object?, bool> Equals = &ImplEquals;
        public readonly new delegate*<string, void> Finalize = (delegate*<string, void>)&object.vtable.ImplFinalize;
        public readonly new delegate*<object, string> ToString = &ImplToString;
        // new methods!
    }
    private readonly unsafe struct System_IComparable__System_String__vtable
    {
        public readonly unsafe delegate*<string, string, int> CompareTo = string.vtable.CompareTo;
    }
    private readonly unsafe struct System_IEquatable__System_String__vtable
    {
        public readonly unsafe delegate*<string, string, bool> Equals = string.vtable.Equals;
    }
    private readonly struct InterfaceTable  // 0xf0000000
    {
        public readonly nint System_IComparable__System_String = 1;
        public readonly nint Impl1 = 1;
        public readonly nint System_IEquatable__System_String = 2;
        public readonly nint Impl2 = 2;
        public readonly nint end = 0;
    }
    protected static new vtable vtable
    {
        get
        {
            Span<byte> rawTable = stackalloc byte[sizeof(vtable)];
            LPM.Read(0xdeadbef9, rawTable);
            return MemoryMarshal.Read<vtable>(rawTable);
        }
    }
    private readonly ushort length;
    private readonly char firstChar;
    public override bool Equals(object? obj) => vtable.Equals(this, obj);
    public override bool Finalize() => object.vtable.Finalize(this);
    public override string ToString() => vtable.ToString(this);
    public int CompareTo(string other)
    {
        // ...
    }
    public bool Equals(string other)
    {
        // ...
    }
}
```

### `StructLayout` attribute
In .NET, both `class`es and `struct`s can have a custom layout.
| `StructLayout` | Behaviour |
| --- | --- |
| `StructLayout.Auto` (default) | Unused fields are deleted from the type. |
| `StructLayout.Sequential` | All fields are preserved in the order they are declared. |
| `StructLayout.Explicit` | All fields are preserved in the order they are mandated. |

### Type checking, vtable dispatching and generics
Each class has a distinct type, identified by their vtable's flash address. An `isinst` instruction works like the following (pseudo-C#):
```cs
static TDst isinst<TSrc, TDst>(ref TSrc obj) where TSrc : class where TDst : class
{
    if (typeof(TSrc) == typeof(TDst))
        return Unsafe.As<TSrc, TDst>(obj);
    // TODO
}
```
Virtual table dispatching will happen in downcast scenarios: Base class to derived class, (un)boxing and interface to implementor.


Templates are monomorphized, meaning that for a set of instantiations of `C<T>`, the compiler will generate the code for each instantiation, regardless of covariance. Be wise!

### Member and type trimming
IL2AVR performs an extensive assembly trimming process, that trims unused types, methods, properties and even fields. The rules to delete unused members follows as such:
- For types, it checks if the type is referenced in any way:
- 1. 

### Compile-time type instantiating
An instance of a compile-time instantiable type can be instantiated at compile-time from a `static` read-only member, or a field that is already instantiated inside a compile-time instantiable type, and the instance must not be sent/manipulated through runtime instructions.
- Calls to constructors of compile-time instances will not emit anything at runtime.
- Calls to methods that receive a compile-time instance but don't modify it in any way can be implicitly monomorphized into a method that doesn't read a real instance of the type, but instead a specialized version where data is available at compile-time, enabling compile-time optimizations. Monomorphizations will always happen for inline functions.

### Pointer reads and writes
AVR boards follows the Harvard architecture, that describes that code and other constant read-only data, stored in the Flash memory, is physically separated from the Static Random Access Memory. This means that the code have two ways of reading data: from flash memory or from volatile memory. Both uses different instructions (`LPM` and `LD` respectivel6) to load. In `avr-gcc` and `avr-g++`, immutable data are always copied from the flash to the SRAM before `main` actually starts, to conform with C semantics where data points to memory. To ignore this copying step, programmers use the `__flash` type qualifier (specific to `avr-gcc`) or they use the `__attribute__((progmem))` data attribute to force the variable to reside in flash only, and they read the data using `pgm_read_*` functions to fetch the data. While this is a basic solution, this creates API cluttering, where most functions have two versions: one for memory data and one for flash data (example: `strlen` and `strlen_P`).
IL2AVR avoids API cluttering by trying to infer as best as possible how the data is used.
- With flash-promotable types that is stored inside a `static readonly` field, a compile-time representable read-only type or a local variable that isn't a parameter and that isn't written, the type can be safely promoted to flash, and every direct read will correctly emit `LPM`.
- With pointer types that are: (TODO)
- Functions that receives pointer types as parameters can receive a flash-adequate version.
-> If the function is inlined, then the compiler will emit read instructions based on what the caller gave.
-> If

### Bytecode to 3AC maping
The compiler has to convert method declarations into a 3AC (Three-Address Code) format that is a superset of ECNA-335 Common Intermediate Language. The following method types is handled by the compiler as follows:
- Method with CIL implementation without inlining: The compiler checks if a method body already exists. If it doesn't exist, then it gets generated on demand.
- Method with CIL implementation with inlining: The compiler checks if the method is parameterless. If it's parameterless, then the compiler generates the method body and pastes this body in every caller that calls it. If the method has parameters, then the compiler generates the body on-the-fly inside at each caller.
- Extern method without inlining: It does nothing but 

### Basic inlining


### Thunk generation


### Exception handling
For each `try`, `catch` and optionally a `finally` clause, it gets transformed into a sequence of conditioning and branching. For each `throw` or for a method that throws an exception unhandled or propagated, the method's return type is transformed into an union between the exception and the result type, or the exception type itself if the return type of the method is void.
For `catch` of expressions that returns a value, the implicit union type is tagged by the `T` SREG of the AVR chip. Example:
```cs
void F() => throw new Exception();
int F(bool v) => v ? 1 : throw new Exception();
void G()
{
    try
    {
        F();
    }
    catch (Exception ex)
    {
        throw new NotImplementedException();
    }
    finally
    {
        Console.WriteLine("uh oh");
    }
}
```
gets transformed into:
```cs
Exception F() => new Exception();
__Result<int, Exception> F(bool v) => v ? 1 : new Exception();
NotImplementedException? G()
{
    Exception? __f = F();
    Console.WriteLine("uh oh");
    if (__f is null)
        return new NotImplementedException();
    else
        return null;
}
```
(where `__Result<T : allows ref struct, E : Exception>` is a meta-type)
The option `--noexcept` enforces that methods can't throw. The BCL itself never throws.

For callbacks, the exception specification depends on the `unmanaged` calling convention existing in the function pointer, or with the `System.Runtime.IL2AVR.ThrowsAttribute` in the delegate. `System.Func<...>` isn't supposed to throw.

### System runtime exceptions
For system exceptions, such as `System.ArithmeticException`, `System.DivisionByZeroException`, `System.NullReferenceException`, these exceptions are generated as per ECMA's spec and the method's return type is implicitly transformed as per above, but some checks can omit the implicit transformation:
- `x != null`/`x is not null`/`!object.ReferenceEquals(x, null)`/`!Unsafe.AreSame(ref x, ref Unsafe.NullRef<T>())`: Being `x` an expression that yields a reference type, if the code explicitly checks for the non-nullability of `x`, then the check is omitted, since the exception is thrown when `x` is, in fact, null.
`if (d != 0) x = n / d`: A division by zero error is pretty rare, but if left unchecked, exceptions can happen.

### Method compilation
Every method is compiled per (generic instantiation) and usage. The ABI is defined as follows:
`X`: `this`, in case of instance methods
`Y`: Reserved for virtual calls.
`Z`: Reserved for general usage.
`r0` .. `r12`: Callee-saved registers.
`r13` .. `r25`: Caller-saved
Intrinsics doesn't need to follow the ABI strictly, apart from not corrupting the caller's stack frame.

### Intrinsic methods
Intrinsic methods are defined inside the compiier itself or inside a compilation call, if the compiler is being used as a library. It can behave like a normal method and it can do everything a normal C# method can do, but it's not implemented in pure C#, instead it's defined in assembly.
When the compiler is traversing through calls, intrinsic methods are called as a C# function inside the compiler, allowing the implementor of the intrinsic method to intercept the compile/emission pipeline. If the method is always inlined (referenced through C# side), then the method is called at each instantiation, with the caller's frame instead of a new frame.

## Compiler diagnostics
The diagnostics are handled 


## BCL
### General overview
The BCL is designed to have a similar feel to the standard .NET's BCL. Some things were removed because it is simply impossible to implement it in an agnostic manner (example: most I/O functionality).

### Differences from standard .NET BCL
For starters, `char` takes 1 byte instead of 2 bytes (`Unsafe.SizeOf<char>() == Unsafe.SizeOf<byte>()`). As a consequence, strings are 8-bit character arrays.
`char` and `int` has an implicit conversion to `byte` due to size issues, as using `int` everywhere is conventional but every `int` takes 4 bytes.
Some APIs that takes `ReadOnlySpan<char>` takes `ReadOnlySpan<byte>` for consistency.


### Aditions specific to AVR
Some new namespaces were added to support AVR-specific things, and compiler annotation attributes for IL2AVR.

`void System.Console.ReadLine(Span<char>)`: A shortcut to `System.Console.Input.ReadLine(Span<char>)`.

`System.Span<T>` and `System.ReadOnlySpan<T>`: Already exists in .NET, but because in AVR "managed pointer" and "unmanaged pointer"s are the same thing, this essentially allows `Span`s to also store a `ref struct`, meaning that `Span<Span<char>>` is a valid construct.

`System.Diagnostics.CodeAnalysis.Hints`: A static class containing some hints that can be useful to the optimizer. They are no-ops in behaviour.
- `bool Likely(bool condition)`: Marks the condition as a likely candidate.
- `bool Unlikely(bool condition)`: Marks the condition as an unlikely candidate.

`System.Diagnostics.Contracts.Hints`: A static class containing some hints that can be useful to the optimizer and that can change code behaviour.
- `T[] ArraySize<T>(T[] array, int length)`: Tells the optimizer that the array has the length specified, allowing it to omit bound checking.
- *`P`*` PointerFrom(`*`P`*` ptr, DataLocation location)`: Tells the optimizer that the pointer comes from the SRAM or from the flash.
    where `P` is any pointer type.

`System.Diagnostics.Contracts.DataLocationAttribute`: Tells the optimizer that the field/method/parameter pointer comes from the SRAM or from the flash.

`System.Math.Internal`: A static class where it contains AVR's math procedures for adding, subtracting, multiplying and dividing floats and doubles.
TODO

`System.Runtime.IL2AVR.InterruptAttribute(InterruptId id)`: An attribute that marks a static parameterless method as an interrupt.

`System.Runtime.CompilerServices.IL2AVR.Assembly`: A static class where it allows the programmer to directly emit assembly code inside the method.
- `Asm(OpCodeId opcode)`: One-instruction version.
- `Asm(OpCodeId opcode, X value1)`: Two-instruction version.
- `Asm(OpCodeId opcode, X value1, Y value2)` Three-instruction version.
    where `X` and `Y` can be `byte`, `short` or a out/ref for some existing local.

`System.Runtime.CompilerServices.IL2AVR.NakedAttribute`: A dangerous attribute that makes the compiler omit prologue and epilogue code from being generated.

`System.Runtime.CompilerServices.InlineArray`*`N`*`<T>` (where *`N`* is from 0 to 16): Just a struct of *`N`* elements.

## Arduino
This project includes a basic library for Arduino programmers, with support for making a port as `Input`, `Output`. You can `Read`, `Write` or `Toggle` ports. You can either use the `D`*n* notation, `A`*n* notation or use directly the `Port`*NN* notation. PWM is not yet implemented.

## Compiler library
The compiler can be called as a library. Every CLI argument can be mapped into the following:

`--define` => `IL2AVR.CompilerOptions.Definitions`: a `IDictionary<string, IL2AVR.CompileTimeValueDefinition>` that maps names to values defined at compile-time.

`--no-exceptions`: `IL2AVR.CompilerOptions.NoExceptions`

## `Mono.Cecil` limitations and the `IClosedType` interface
The compiler uses `Mono.Cecil` to operate with CIL code, but there are a lot of processes that the compiler must do, such as generic monomorphization, type monomorphization, method inlining and so on, things that Mono.Cecil doesn't help much alone because it doesn't monomorphize generic types, methods and parameters automatically. That led to the creation of the `IClosedType` interface, a type similar to the `TypeReference` class, but with additional properties and structure that makes it possible for the compiler to monomorphize code as necessary.
| Mono.Cecil entity | IL2AVR translation | Notes |
| --- | --- | --- |
| `TypeReference`/`TypeDefinition` | `ClosedType` | ... |

### CompilerBag
a

# C# pitfalls
## Primitives
`ìnt` should not be used sparingly, because its size is fixed at 32 bits. Even if we changed `ìnt` to mean 16 or 8 bits, C# still alias `ìnt` as `System.Int32`, and having `System.Int32` to be 16 bytes would make things really awkward.

## `sizeof()`
Thanks to the pitfall above, `sizeof()` is the most unsafe thing on the language because it straight off lies to you (since it assumes that the code runs on a 32/64 bit computer). You should use `Unsafe.SizeOf<T>()` instead, because the IL2AVR compiler guarantees the right size.

# ROADMAP
## CIL instructions

# Comprehensive CIL Instruction Checklist
| Status | Opcode | Instruction |
| --- | --- | --- |
| X | `0x00` | `nop` |
| X | `0x01` | `break` |
| O | `0x02` | `ldarg.0` |
| O | `0x03` | `ldarg.1` |
| O | `0x04` | `ldarg.2` |
| O | `0x05` | `ldarg.3` |
|   | `0x06` | `ldloc.0` |
|   | `0x07` | `ldloc.1` |
|   | `0x08` | `ldloc.2` |
|   | `0x09` | `ldloc.3` |
|   | `0x0A` | `stloc.0` |
|   | `0x0B` | `stloc.1` |
|   | `0x0C` | `stloc.2` |
|   | `0x0D` | `stloc.3` |
| O | `0x0E` | `ldarg.s` |
|   | `0x0F` | `ldarga.s` |
|   | `0x10` | `starg.s` |
|   | `0x11` | `ldloc.s` |
|   | `0x12` | `ldloca.s` |
|   | `0x13` | `stloc.s` |
| O | `0x14` | `ldnull` |
| O | `0x15` | `ldc.i4.m1` |
| O | `0x16` | `ldc.i4.0` |
| O | `0x17` | `ldc.i4.1` |
| O | `0x18` | `ldc.i4.2` |
| O | `0x19` | `ldc.i4.3` |
| O | `0x1A` | `ldc.i4.4` |
| O | `0x1B` | `ldc.i4.5` |
| O | `0x1C` | `ldc.i4.6` |
| O | `0x1D` | `ldc.i4.7` |
| O | `0x1E` | `ldc.i4.8` |
| O | `0x1F` | `ldc.i4.s` |
| O | `0x20` | `ldc.i4` |
| O | `0x21` | `ldc.i8` |
| O | `0x22` | `ldc.r4` |
| O | `0x23` | `ldc.r8` |
| O | `0x25` | `dup` |
| O | `0x26` | `pop` |
|   | `0x27` | `jmp` |
|   | `0x28` | `call` |
|   | `0x29` | `calli` |
|   | `0x2A` | `ret` |
|   | `0x2B` | `br.s` |
|   | `0x2C` | `brfalse.s` |
|   | `0x2D` | `brtrue.s` |
|   | `0x2E` | `beq.s` |
|   | `0x2F` | `bge.s` |
|   | `0x30` | `bgt.s` |
|   | `0x31` | `ble.s` |
|   | `0x32` | `blt.s` |
|   | `0x33` | `bne.un.s` |
|   | `0x34` | `bge.un.s` |
|   | `0x35` | `bgt.un.s` |
|   | `0x36` | `ble.un.s` |
|   | `0x37` | `blt.un.s` |
|   | `0x38` | `br` |
|   | `0x39` | `brfalse` |
|   | `0x3A` | `brtrue` |
|   | `0x3B` | `beq` |
|   | `0x3C` | `bge` |
|   | `0x3D` | `bgt` |
|   | `0x3E` | `ble` |
|   | `0x3F` | `blt` |
|   | `0x40` | `bne.un` |
|   | `0x41` | `bge.un` |
|   | `0x42` | `bgt.un` |
|   | `0x43` | `ble.un` |
|   | `0x44` | `blt.un` |
|   | `0x45` | `switch` |
|   | `0x46` | `ldind.i1` |
|   | `0x47` | `ldind.u1` |
|   | `0x48` | `ldind.i2` |
|   | `0x49` | `ldind.u2` |
|   | `0x4A` | `ldind.i4` |
|   | `0x4B` | `ldind.u4` |
|   | `0x4C` | `ldind.i8` |
|   | `0x4D` | `ldind.i` |
|   | `0x4E` | `ldind.r4` |
|   | `0x4F` | `ldind.r8` |
|   | `0x50` | `ldind.ref` |
|   | `0x51` | `stind.ref` |
|   | `0x52` | `stind.i1` |
|   | `0x53` | `stind.i2` |
|   | `0x54` | `stind.i4` |
|   | `0x55` | `stind.i8` |
|   | `0x56` | `stind.r4` |
|   | `0x57` | `stind.r8` |
|   | `0x58` | `add` |
|   | `0x59` | `sub` |
| # | `0x5A` | `mul` |
| # | `0x5B` | `div` |
| # | `0x5C` | `div.un` |
| # | `0x5D` | `rem` |
| # | `0x5E` | `rem.un` |
|   | `0x5F` | `and` |
|   | `0x60` | `or` |
|   | `0x61` | `xor` |
|   | `0x62` | `shl` |
|   | `0x63` | `shr` |
|   | `0x64` | `shr.un` |
|   | `0x65` | `neg` |
|   | `0x66` | `not` |
|   | `0x67` | `conv.i1` |
|   | `0x68` | `conv.i2` |
|   | `0x69` | `conv.i4` |
|   | `0x6A` | `conv.i8` |
|   | `0x6B` | `conv.r4` |
|   | `0x6C` | `conv.r8` |
|   | `0x6D` | `conv.u4` |
|   | `0x6E` | `conv.u8` |
|   | `0x6F` | `callvirt` |
|   | `0x70` | `cpobj` |
|   | `0x71` | `ldobj` |
| O | `0x72` | `ldstr` |
|   | `0x73` | `newobj` |
|   | `0x74` | `castclass` |
|   | `0x75` | `isinst` |
|   | `0x76` | `conv.r.un` |
|   | `0x79` | `unbox` |
|   | `0x7A` | `throw` |
|   | `0x7B` | `ldfld` |
|   | `0x7C` | `ldflda` |
|   | `0x7D` | `stfld` |
|   | `0x7E` | `ldsfld` |
|   | `0x7F` | `ldsflda` |
|   | `0x80` | `stsfld` |
|   | `0x81` | `stobj` |
|   | `0x82` | `conv.ovf.i1.un` |
|   | `0x83` | `conv.ovf.i2.un` |
|   | `0x84` | `conv.ovf.i4.un` |
|   | `0x85` | `conv.ovf.i8.un` |
|   | `0x86` | `conv.ovf.u1.un` |
|   | `0x87` | `conv.ovf.u2.un` |
|   | `0x88` | `conv.ovf.u4.un` |
|   | `0x89` | `conv.ovf.u8.un` |
|   | `0x8A` | `conv.ovf.i.un` |
|   | `0x8B` | `conv.ovf.u.un` |
|   | `0x8C` | `box` |
| # | `0x8D` | `newarr` |
|   | `0x8E` | `ldlen` |
|   | `0x8F` | `ldelema` |
|   | `0x90` | `ldelem.i1` |
|   | `0x91` | `ldelem.u1` |
|   | `0x92` | `ldelem.i2` |
|   | `0x93` | `ldelem.u2` |
|   | `0x94` | `ldelem.i4` |
|   | `0x95` | `ldelem.u4` |
|   | `0x96` | `ldelem.i8` |
|   | `0x97` | `ldelem.i` |
|   | `0x98` | `ldelem.r4` |
|   | `0x99` | `ldelem.r8` |
|   | `0x9A` | `ldelem.ref` |
|   | `0x9B` | `stelem.i` |
|   | `0x9C` | `stelem.i1` |
|   | `0x9D` | `stelem.i2` |
|   | `0x9E` | `stelem.i4` |
|   | `0x9F` | `stelem.i8` |
|   | `0xA0` | `stelem.r4` |
|   | `0xA1` | `stelem.r8` |
|   | `0xA2` | `stelem.ref` |
|   | `0xA3` | `ldelem` |
|   | `0xA4` | `stelem` |
|   | `0xA5` | `unbox.any` |
|   | `0xB3` | `conv.ovf.i1` |
|   | `0xB4` | `conv.ovf.u1` |
|   | `0xB5` | `conv.ovf.i2` |
|   | `0xB6` | `conv.ovf.u2` |
|   | `0xB7` | `conv.ovf.i4` |
|   | `0xB8` | `conv.ovf.u4` |
|   | `0xB9` | `conv.ovf.i8` |
|   | `0xBA` | `conv.ovf.u8` |
|   | `0xC2` | `refanyval` |
|   | `0xC3` | `ckfinite` |
|   | `0xC6` | `mkrefany` |
| O | `0xD0` | `ldtoken` |
|   | `0xD1` | `conv.u2` |
|   | `0xD2` | `conv.u1` |
|   | `0xD3` | `conv.i` |
|   | `0xD4` | `conv.ovf.i` |
|   | `0xD5` | `conv.ovf.u` |
|   | `0xD6` | `add.ovf` |
|   | `0xD7` | `add.ovf.un` |
| # | `0xD8` | `mul.ovf` |
| # | `0xD9` | `mul.ovf.un` |
|   | `0xDA` | `sub.ovf` |
|   | `0xDB` | `sub.ovf.un` |
|   | `0xDC` | `endfinally` |
|   | `0xDD` | `leave` |
|   | `0xDE` | `leave.s` |
|   | `0xDF` | `stind.i` |
|   | `0xE0` | `conv.u` |
|   | `0xFE 0x00` | `arglist` |
|   | `0xFE 0x01` | `ceq` |
|   | `0xFE 0x02` | `cgt` |
|   | `0xFE 0x03` | `cgt.un` |
|   | `0xFE 0x04` | `clt` |
|   | `0xFE 0x05` | `clt.un` |
|   | `0xFE 0x06` | `ldftn` |
|   | `0xFE 0x07` | `ldvirtftn` |
|   | `0xFE 0x09` | `ldarg` |
|   | `0xFE 0x0A` | `ldarga` |
|   | `0xFE 0x0B` | `starg` |
|   | `0xFE 0x0C` | `ldloc` |
|   | `0xFE 0x0D` | `ldloca` |
|   | `0xFE 0x0E` | `stloc` |
|   | `0xFE 0x0F` | `localloc` |
|   | `0xFE 0x11` | `endfilter` |
| ! | `0xFE 0x12` | `unaligned.` |
|   | `0xFE 0x13` | `volatile.` |
|   | `0xFE 0x14` | `tail.` |
|   | `0xFE 0x15` | `initobj` |
|   | `0xFE 0x16` | `constrained.` |
| # | `0xFE 0x17` | `cpblk` |
| # | `0xFE 0x18` | `initblk` |
|   | `0xFE 0x19` | `no.` |
|   | `0xFE 0x1A` | `rethrow` |
| O | `0xFE 0x1C` | `sizeof` |
|   | `0xFE 0x1D` | `refanytype` |
|   | `0xFE 0x1E` | `readonly.` |

`X`: Implemented.

`#`: Call forwarded to userspace.

`O`: It's an CIL instruction that just manipulates the stack without doing any useful computation.

`!`: No-operation.

# Thanks
Thanks for reading this!
