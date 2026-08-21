using System;
namespace IL2AVR;

public enum InlineMode
{
    DisableInlining = -1,
    NoInlining,
    BasicMethodInlining = 1,
    ReadonlyFieldInlining = 2,
    AgressiveInlining = BasicMethodInlining | ReadonlyFieldInlining
}
