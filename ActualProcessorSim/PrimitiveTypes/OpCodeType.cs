namespace ActualProcessorSim.Instructions;

public enum OpCodeType : byte
{
    MOV = 0,
    ADD = 1,
    SUB = 2,
    MUL = 3,
    LS = 4,
    RS = 5,
    RET = 6,
    BLA = 7,
    BGE = 8,
    BGT = 9,
    BLE = 10,
    BLT = 11,
    END = 12,
    CMP = 13,
    SWI = 14
}
