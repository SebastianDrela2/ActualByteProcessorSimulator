using System.Runtime.InteropServices;
using ActualProcessorSim.Assembly;
using ActualProcessorSim.Memory;
using ActualProcessorSim.MemorySection;
using ActualProcessorSim.PhysicalComponent;

namespace ActualProcessorSim.Runtime;
public class Computer(InstructionLineInformation lastInfo)
{
    public required Processor Processor { get; init; }
    public required RamMemory Memory { get; init; }

    public OffsettedMemory GetOffsettedProgramMemory() => Memory.WithOffset(Processor.ProgramCounter.Value);
    public OffsettedMemory GetOffsettedStackMemory() => Memory.WithOffset(Processor.StackPointer.Value);

    public void Execute()
    {
        var instructionExecutor = new InstructionExecutor(this);
        Console.WriteLine($"Executed bytes:");

        while (Processor.MoveNext)
        {            
            var opCode = (OpCodeType)Memory[Processor.ProgramCounter.Value];

            MemoryBuilder.InstructionLengthDict.TryGetValue(opCode, out var length);
            DisplayBytesToExecute(length);

            var executeInformation = instructionExecutor.Execute(opCode);

            if (executeInformation.Exception is not null)
            {
                throw executeInformation.Exception;
            }        

            if (!executeInformation.JumpedPerformed)
            {
                Processor.ProgramCounter.Value += (byte)length;
            }
        }
    }

    public void DisplayFullMemory()
    {
        var instructionExecutor = new InstructionExecutor(this);

        var textRange = (int)MemorySectionOffset.TextRegionOffset..(lastInfo.Position + lastInfo.BytesLength);
        var instr = MemoryBuilder.GetInstructions(Memory, textRange);
        Console.WriteLine($"MEMORY: ");

        foreach (var (offset, length) in instr)
        {
            var bytes = Memory.Memory.Slice(offset, length);

            var bytesText = string.Join(" ", MemoryMarshal.ToEnumerable(bytes).Select(x => $"{x:X2}"));
            Console.WriteLine($"{offset:X8} | {bytesText}");
        }
    }

    private void DisplayBytesToExecute(int length)
    {
        var bytes = GetOffsettedProgramMemory();
        var memory = bytes[..length];
        var enumeratedMemory = MemoryMarshal.ToEnumerable(memory);
        var bytesText = string.Join(" ", enumeratedMemory.Select(@byte => $"{@byte:X2}"));
        Console.WriteLine($"{Processor.ProgramCounter.Value:X8} |{bytesText,21}");
    }
}
