using System.Numerics;
namespace System;

public readonly struct SByte : IMinMaxValue<sbyte>
{
    public const sbyte MinValue = -128, MaxValue = 127;
    static sbyte IMinMaxValue<sbyte>.MinValue => MinValue;
    static sbyte IMinMaxValue<sbyte>.MaxValue => MaxValue;
    
}