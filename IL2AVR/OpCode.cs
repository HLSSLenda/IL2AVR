namespace IL2AVR.IR;

public readonly record struct OpCode(OpCodeId Id, IValue? LeftOperand = null, IValue? RightOperand = null) : IValue
{
    ClosedType IValue.Type => null!;
    public static int Size(OpCodeId id) => id switch
    {
        OpCodeId.ExtCall or OpCodeId.ExtJmp or OpCodeId.Lds or OpCodeId.Sts => 4,
        _ => 2
    };
    public override string ToString()
    {
        string name = Id.ToString().ToUpperInvariant();
        if (name.StartsWith("EXT"))
            name = name[4 ..];
        switch (name)
        {
            // no operands
            case "BREAK":
            case "CLC":
            case "CLH":
            case "CLI":
            case "CLN":
            case "CLS":
            case "CLT":
            case "CLV":
            case "CLZ":
            case "EICALL":
            case "EIJMP":
            case "ICALL":
            case "IJMP":
            case "NOP":
            case "RET":
            case "RETI":
            case "SEC":
            case "SEH":
            case "SEI":
            case "SEN":
            case "SES":
            case "SET":
            case "SEV":
            case "SEZ":
            case "SLEEP":
            case "WDR":
                return name.ToString();
            // single const operand
            case "BCLR":
            case "BSET":
            case "DES":
                return $"{name} {((ValueOperand)LeftOperand!).Value}";
            // single register operand
            case "ASR":
            case "CLR":
            case "COM":
            case "DEC":
            case "INC":
            case "LSL":
            case "LSR":
            case "NEG":
            case "POP":
            case "PUSH":
            case "ROL":
            case "ROR":
            case "SER":
            case "SWAP":
            case "TST":
                return $"{name} {((ValueOperand)LeftOperand!).Value:r}";
            // single address operand
            case "BRCC":
            case "BRCS":
            case "BREQ":
            case "BRGT":
            case "BRHC":
            case "BRHS":
            case "BRID":
            case "BRIE":
            case "BRLT":
            case "BRLO":
            case "BRMI":
            case "BRNE":
            case "BRPL":
            case "BRSH":
            case "BRTC":
            case "BRTS":
            case "BRVC":
            case "BRVS":
            case "CALL":
            case "JMP":
            case "RCALL":
            case "RJMP":
                return $"{name} {((AddressOperand)LeftOperand!).Address:X4}";
            default:
                throw new WtfException();
        };
    }
}
