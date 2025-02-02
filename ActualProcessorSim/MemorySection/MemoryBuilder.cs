

using ActualProcessorSim.Assembly;
using ActualProcessorSim.MemorySection;

namespace ActualProcessorSim.Memory;
public class MemoryBuilder
{
    public static readonly Dictionary<OpCodeType, int> InstructionLengthDict = new Dictionary<OpCodeType, int>
    {
        {OpCodeType.MOV, 4},
        {OpCodeType.ADD, 4},
        {OpCodeType.SUB, 4},
        {OpCodeType.MUL, 4},
        {OpCodeType.LS, 4},
        {OpCodeType.RS, 4},
        {OpCodeType.EXIT, 2},
        {OpCodeType.BLA, 3},
        {OpCodeType.BGE, 3},
        {OpCodeType.BGT, 3},
        {OpCodeType.BLE, 3},
        {OpCodeType.BLT, 3},
        {OpCodeType.END, 2},
        {OpCodeType.CMP, 4},
        {OpCodeType.SWI, 4 },
    };

    public void Link(List<InstructionLineInformation> lineInformations, RamMemory memory)
    {
        var jumpData = lineInformations
        .Select((lineInfo, index) => (lineInfo, index))
        .Where(x => x.lineInfo.LineInformation.IsConditionalJump)
        .Select(x =>
        (
            ParsedJumpIndex: int.Parse(x.lineInfo.Instruction.Split(' ')[1]),
            MemoryIndex: x.index 
        ))
        .ToList();

        TransformJmpIndexesIntoMemoryAdress(jumpData, lineInformations, memory);

    } 

    private void TransformJmpIndexesIntoMemoryAdress(
        List<(int ParsedJumpIndex, int MemoryIndex)> jumpData, 
        List<InstructionLineInformation> lineInformations, 
        RamMemory memory
    )
    {
        foreach(var (parsedJumpIndex, memoryIndex) in jumpData)
        {
            var lineInformation = lineInformations[memoryIndex];
            var jmpArgumentPosition = lineInformation.Position + 1;

            var jmpLineInformation = lineInformations[parsedJumpIndex];
            var jmpPosition = jmpLineInformation.Position;
            memory.Content[jmpArgumentPosition] = (byte)jmpPosition;
        }
    }

    private record struct BytesWithLength(byte[] Bytes, int Length);
}
