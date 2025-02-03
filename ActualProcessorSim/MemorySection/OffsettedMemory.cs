namespace ActualProcessorSim.MemorySection;

public readonly struct OffsettedMemory(RamMemory ram, int offset)
{
    public ref byte this[int index] => ref ram[offset + index];
    
    public ReadOnlyMemory<byte> this[Range range] => ram.Memory[offset..][range];
    public Span<byte> Span => ram.Span[offset..];
}