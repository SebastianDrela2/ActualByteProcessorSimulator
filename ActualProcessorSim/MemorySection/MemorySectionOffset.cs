namespace ActualProcessorSim.MemorySection;
public enum MemorySectionOffset : int
{
    TextRegionOffset = 0x0,
    DataRegionOffset = 0x00200000,
    BssRegionOffset = 0x00400000,
    StackRegionOffset = 0x00600000,
    EndOffSet = RamMemory.RamLength
}
