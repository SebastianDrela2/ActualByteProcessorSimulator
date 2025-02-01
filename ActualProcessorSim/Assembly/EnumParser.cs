using ProcessorSim.Instructions;

namespace ActualProcessorSim.Assembly;
public static class EnumParser
{
    public static readonly Dictionary<string, RegisterCodeType> RegisterCodeTextToValue = new(StringComparer.OrdinalIgnoreCase) {
        { "r0", RegisterCodeType.R0 },
        { "r1", RegisterCodeType.R1 },
        { "r2", RegisterCodeType.R2 },
        { "r3", RegisterCodeType.R3 },
        { "r4", RegisterCodeType.R4 },
        { "r5", RegisterCodeType.R5 },
        { "r6", RegisterCodeType.R6 },
        { "r7", RegisterCodeType.R7 },
        { "r9", RegisterCodeType.LR },
        { "lr", RegisterCodeType.LR },
        { "cpsr", RegisterCodeType.CPSR },
    };
    public static readonly Dictionary<string, RegisterCodeType>.AlternateLookup<ReadOnlySpan<char>> RegisterCodeTextToValueAlt
        = RegisterCodeTextToValue.GetAlternateLookup<ReadOnlySpan<char>>();

    public static bool TryParse(ReadOnlySpan<char> register, out RegisterCodeType value)
    {
        return RegisterCodeTextToValueAlt.TryGetValue(register, out value);
    }
}
