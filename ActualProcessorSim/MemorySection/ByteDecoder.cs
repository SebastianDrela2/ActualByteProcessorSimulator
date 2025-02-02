using System.Buffers.Binary;
using ActualProcessorSim.PhysicalComponent;
using ProcessorSim.Instructions;

namespace ActualProcessorSim.MemorySection;
public class ByteDecoder(Processor processor)
{
    public Register DecodeRegister(byte chunk)
    {
        var registerCode = (RegisterCodeType)chunk;
        var register = processor.Registers.Single(register => register.RegisterCode == registerCode);

        return register;
    }

    public byte DecodeValue(byte chunk) => chunk;

    public int DecodeValues(ReadOnlyMemory<byte> bytes)
    {
        BinaryPrimitives.TryReadInt32LittleEndian(bytes.Span, out var result);

        return result;
    }
}
