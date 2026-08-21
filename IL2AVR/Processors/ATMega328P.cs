using System;
namespace IL2AVR.Processors;

public class ATMega328P : IProcessor
{
    public byte DDRB => 0x4;
    public byte DDRC => 0x7;
    public byte DDRD => 0xa;

    public bool HasAdiw => true;
    public bool HasBreak => true;
    public bool HasCall => true;
    public bool HasDes => true;
    public bool HasEicall => false;
    public bool HasEijmp => false;
    public bool HasElpm => false;
    public bool HasFmul => true;
    public bool HasFmuls => true;
    public bool HasFmulsu => true;
    public bool HasJmp => true;
    public bool HasLac => true;
    public bool HasLas => true;
    public bool HasLat => true;
    public bool HasLdd => true;
    public bool HasLpm => true;
    public bool HasMovw => true;
    public bool HasMul => true;
    public bool HasMuls => true;
    public bool HasMulsu => true;
    public bool HasSbiw => true;
    public bool HasSpm => true;
    public bool HasStd => true;
    public bool HasXch => true;

    public ushort RamEnd => 0x8ff;
    public ushort RamStart => 0x100;

    public Range RegisterRange => 0 .. 32;

    public byte SPL => 0x3d;
    public byte SPH => 0x3e;

    public Range TotalRegisterRange => 0 .. 32;
}
