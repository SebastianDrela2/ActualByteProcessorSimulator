using ActualProcessorSim.Assembly;

namespace ActualProcessorSim.MemorySection;

public class RamMemory
{
    private const int RamLength = 8 << 20;

    public byte[] Content;

    public byte this[int index] => Content[index];

    public RamMemory(BytesBuilder builder)
    {
        Content = new byte[RamLength];
        builder.CopyTo(Content);
    }

    public OffsettedMemory WithOffset(int offset) => new(this, offset);
}
