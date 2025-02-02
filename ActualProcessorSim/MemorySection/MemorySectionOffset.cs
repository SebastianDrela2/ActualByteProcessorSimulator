namespace ActualProcessorSim.MemorySection;
public enum MemorySectionOffset : int
{
    TextRegionOffset = 0x0,
    DataRegionOffset = 0x00600000,
    BssRegionOffset = 0x00700000,
    StackRegionOffset = 0x00FFFFFF,
}
