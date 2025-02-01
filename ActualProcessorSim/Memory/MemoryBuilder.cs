

using System.Collections.Immutable;
using ActualProcessorSim.Assembly;
using ActualProcessorSim.Types;

namespace ActualProcessorSim.Memory;
internal class MemoryBuilder
{
    private readonly Dictionary<OpCodeType, int> _instructionLengthDict = new Dictionary<OpCodeType, int>
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
        {OpCodeType.CMP, -1},
        {OpCodeType.SWI, -1 },
    };

    public Dictionary<Address, byte[]> Build(ImmutableArray<byte>.Builder bytes)
    {
        var adressVal = 0;
        var bytesWithLength = new List<BytesWithLength>();
        var currentLength = 0;

        for (var i = 0; i < bytes.Count; i += currentLength)
        {
            var opCode = (OpCodeType)bytes[i];

           _instructionLengthDict.TryGetValue(opCode, out currentLength);

            var chunkSize = Math.Min(currentLength, bytes.Count - i);
            var chunk = new byte[chunkSize];

            for (var j = 0; j < chunkSize; j++) 
            {
                chunk[j] = bytes[i + j];
            }

            bytesWithLength.Add(new BytesWithLength(chunk, currentLength));
        }

        var dictionary = new Dictionary<Address, byte[]>();

        foreach (var byteWithLength in bytesWithLength)
        {
            var adress = new Address(adressVal);
            dictionary[adress] = byteWithLength.Bytes;
            adressVal += byteWithLength.Length;
        }

        return dictionary;
    }

    public void Link(List<InstructionLineInformation> lineInformations, Dictionary<Address, byte[]> processedMemory)
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

        TransformJmpIndexesIntoMemoryAdress(jumpData, processedMemory);

    } 

    private void TransformJmpIndexesIntoMemoryAdress(List<(int ParsedJumpIndex, int MemoryIndex)> jumpData, Dictionary<Address, byte[]> processedMemory)
    {
        foreach (var (parsedJumpIndex, memoryIndex) in jumpData)
        {
            if (memoryIndex >= 0 && memoryIndex < processedMemory.Count)
            {
                var byteJmpKey = processedMemory.Keys.ElementAt(memoryIndex);
                var jmpAddress = processedMemory.Keys.ElementAt(parsedJumpIndex);
                var bytes = processedMemory[byteJmpKey];
                bytes[1] = jmpAddress;            
            }
        }
    }

    private record struct BytesWithLength(byte[] Bytes, int Length);
}
