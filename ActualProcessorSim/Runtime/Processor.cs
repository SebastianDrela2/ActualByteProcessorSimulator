using ActualProcessorSim.Assembly;
using ActualProcessorSim.MemorySection;
using ProcessorSim.Instructions;

namespace ActualProcessorSim.PhysicalComponent
{
    public class Processor
    {
        public readonly IList<Register> Registers;

        public Register ProgramCounter;
        public Register CurrentProgramStatus;

        public bool MoveNext;

        public Processor()
        {
            MoveNext = true;
            Registers = [];

            for (var index = 0; index < 8; index++)
            {
                var registerCodeType = Enum.Parse<RegisterCodeType>($"R{index}");
                Registers.Add(new Register(registerCodeType, 0));
            }
            var programCounter = new Register(RegisterCodeType.PC, 0);
            var currentProgramStatus = new Register(RegisterCodeType.CPSR, 0);

            Registers.Add(new Register(RegisterCodeType.LR, 0));

            Registers.Add(programCounter);
            Registers.Add(currentProgramStatus);

            CurrentProgramStatus = currentProgramStatus;

            programCounter.Value = (byte)MemorySectionOffset.TextRegionOffset;
            ProgramCounter = programCounter;
        }
    }
}
