namespace ActualProcessorSim.Instructions;

public enum OpCodeType : byte
{
    MOV,
    ADD,
    SUB,
    MUL,
    LS,
    RS,
    EXIT,
    BLA,
    BGE,
    BGT,
    BLE,
    BLT,
    END,
    CMP,
    SWI
}
