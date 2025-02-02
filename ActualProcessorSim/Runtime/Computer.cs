using System.Collections.Immutable;
using ActualProcessorSim.Memory;
using ActualProcessorSim.MemorySection;
using ActualProcessorSim.PhysicalComponent;

namespace ActualProcessorSim.Runtime;
public class Computer()
{
    public required Processor Processor { get; init; }
    public required RamMemory Memory { get; init; }

    public OffsettedMemory GetOffsettedProgramMemory() => Memory.WithOffset(ProgramCounterValue);
    public int ProgramCounterValue => Processor.ProgramCounter.Value;

    public void Execute()
    {
        var instructionExecutor = new InstructionExecutor(this);
        Console.WriteLine($"Executed bytes:");          

        while (Processor.MoveNext)
        {
            var opCode = (OpCodeType)Memory[Processor.ProgramCounter.Value];         
            var executeInformation = instructionExecutor.Execute(opCode);

            if (executeInformation.Exception is not null)
            {
                throw executeInformation.Exception;
            }

            MemoryBuilder.InstructionLengthDict.TryGetValue(opCode, out var length);
            
            DisplayExecutedBytes(length);

            if (!executeInformation.JumpedPerformed)
            {
                 Processor.ProgramCounter.Value += (byte)length;
            }

        }
    }

    private void DisplayExecutedBytes(int length)
    {
        var bytes = GetOffsettedProgramMemory();
        var displayBytes = new byte[length];

        for(var index = 0 ; index < length ; index ++)
        {
            displayBytes[index] = bytes[index];
        }

        var bytesText = string.Join(" ", displayBytes.Select(@byte => $"{@byte:X2}"));
        Console.WriteLine($"{Processor.ProgramCounter.Value:X8} |{bytesText, 12}");
    }
}
