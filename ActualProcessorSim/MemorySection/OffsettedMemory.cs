namespace ActualProcessorSim.MemorySection;

public readonly struct OffsettedMemory(RamMemory ram, int offset)
{
    public byte this[int index] => ram[offset + index];

    public ReadOnlyMemory<byte> this[Range range] => ram.Memory[offset..][range];
}