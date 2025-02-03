using ActualProcessorSim.Assembly;

namespace ActualProcessorSim.MemorySection;

public class RamMemory
{
    public const int RamLength = 8 << 20;

    public byte[] Content;

    public int Count => RamLength;

    public ref byte this[int index] => ref Content[index];

    public ReadOnlyMemory<byte> Memory => Content.AsMemory();
    public Span<byte> Span => Content.AsSpan();

    public RamMemory(BytesBuilder builder)
    {
        Content = new byte[RamLength];
        builder.CopyTo(Content);
    }

    public OffsettedMemory WithOffset(int offset) => new(this, offset);
}
