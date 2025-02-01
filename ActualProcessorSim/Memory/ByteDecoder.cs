﻿using ActualProcessorSim.PhysicalComponent;
using ProcessorSim.Instructions;

namespace ActualProcessorSim.Memory;
public class ByteDecoder(Processor processor)
{
    public Register DecodeRegister(byte chunk)
    {
        var registerCode = (RegisterCodeType)chunk;
        var register = processor.Registers.Single(register => register.RegisterCode == registerCode);

        return register;
    }

    public byte DecodeValue(byte chunk) => chunk;
}
