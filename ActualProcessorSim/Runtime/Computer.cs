using System.Runtime.InteropServices;
using ActualProcessorSim.Assembly;
using ActualProcessorSim.Memory;
using ActualProcessorSim.MemorySection;
using ActualProcessorSim.PhysicalComponent;

namespace ActualProcessorSim.Runtime;
public class Computer(InstructionLineInformation lastInfo)
{
    private readonly DisplayDiagnostic DisplayDiagnostic = new();
    public required Processor Processor { get; init; }
    public required RamMemory Memory { get; init; }

    public OffsettedMemory GetOffsettedProgramMemory() => Memory.WithOffset(Processor.ProgramCounter.Value);
    public OffsettedMemory GetOffsettedStackMemory() => Memory.WithOffset(Processor.StackPointer.Value);

    public void Execute()
    {
        var instructionExecutor = new InstructionExecutor(this);
        DisplayDiagnostic.ExecutedBytes.Add($"EXECUTED:");

        while (Processor.MoveNext)
        {            
            var opCode = (OpCodeType)Memory[Processor.ProgramCounter.Value];

            MemoryBuilder.InstructionLengthDict.TryGetValue(opCode, out var length);
            SetBytesToExecuteDisplay(length);

            var executeInformation = instructionExecutor.Execute(opCode);
            
            if (executeInformation.Exception is not null)
            {
                throw executeInformation.Exception;
            }

            SetStackDisplay();

            if (!executeInformation.JumpedPerformed)
            {
                Processor.ProgramCounter.Value += (byte)length;
            }

            DisplayDiagnostic.DisplayBytes();
        }
    }

    public void SetFullMemoryDisplay()
    {
        var instructionExecutor = new InstructionExecutor(this);

        var textRange = (int)MemorySectionOffset.TextRegionOffset..(lastInfo.Position + lastInfo.BytesLength);
        var instr = MemoryBuilder.GetInstructions(Memory, textRange);

        DisplayDiagnostic.Memory.Add($"MEMORY: ");

        foreach (var (offset, length) in instr)
        {
            var bytes = Memory.Memory.Slice(offset, length);

            var bytesText = string.Join(" ", MemoryMarshal.ToEnumerable(bytes).Select(x => $"{x:X2}"));
            DisplayDiagnostic.Memory.Add($"{offset:X8} | {bytesText}");
        }

        DisplayDiagnostic.Memory.Add(string.Empty);
    }

    private void SetStackDisplay()
    {
        DisplayDiagnostic.Stack.Clear();
        DisplayDiagnostic.Stack.Add(string.Empty);
        DisplayDiagnostic.Stack.Add("STACK: ");

        var (start, end) = (Processor.StackPointer.Value, (int)MemorySectionOffset.EndOffSet);

        for (var index = start; index < end; index++)
        {
            var chunk = Memory[index];

            DisplayDiagnostic.Stack.Add($"{index:X8} | {chunk:X2}");

        }
    }

    private void SetBytesToExecuteDisplay(int length)
    {
        var bytes = GetOffsettedProgramMemory();
        var memory = bytes[..length];
        var enumeratedMemory = MemoryMarshal.ToEnumerable(memory);
        var bytesText = string.Join(" ", enumeratedMemory.Select(@byte => $"{@byte:X2}"));
        DisplayDiagnostic.ExecutedBytes.Add($"{Processor.ProgramCounter.Value:X8} | {bytesText}");
    }
}
