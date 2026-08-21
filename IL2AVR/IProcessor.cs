using System;
namespace IL2AVR;

public interface IProcessor
{
    byte DDRB { get; }
    byte DDRC { get; }
    byte DDRD { get; }
    bool HasAdiw { get; }
    bool HasBreak { get; }
    bool HasCall { get; }
    bool HasDes { get; }
    bool HasEicall { get; }
    bool HasEijmp { get; }
    bool HasElpm { get; }
    bool HasFmul { get; }
    bool HasFmuls { get; }
    bool HasFmulsu { get; }
    bool HasJmp { get; }
    bool HasLac { get; }
    bool HasLas { get; }
    bool HasLat { get; }
    bool HasLdd { get; }
    bool HasLpm { get; }
    bool HasMovw { get; }
    bool HasMul { get; }
    bool HasMuls { get; }
    bool HasMulsu { get; }
    bool HasSbiw { get; }
    bool HasSpm { get; }
    bool HasStd { get; }
    bool HasXch { get; }
    ushort RamEnd { get; }
    ushort RamStart { get; }
    Range RegisterRange { get; }
    byte SPL { get; }
    byte SPH { get; }
    Range TotalRegisterRange { get; }
    const byte X = 26, XL = 26, XH = 27, Y = 28, YL = 28, YH = 29, Z = 30, ZL = 30, ZH = 30;
}