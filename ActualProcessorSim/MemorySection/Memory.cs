using System.Collections.Immutable;

namespace ActualProcessorSim.MemorySection;

public class RamMemory
{
    private const int RamLength = 8 << 20;

    public required ImmutableArray<byte>.Builder Content;

    public byte this[int index] => Content[index];
    public OffsettedMemory WithOffset(int offset) => new(this, offset);
}
