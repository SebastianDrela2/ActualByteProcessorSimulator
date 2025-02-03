using ActualProcessorSim.Assembly;
using ActualProcessorSim.MemorySection;
using ProcessorSim.Instructions;

namespace ActualProcessorSim.PhysicalComponent
{
    public class Processor
    {
        public readonly IList<Register> Registers;

        public Register ProgramCounter;
        public Register StackPointer;
        public Register CurrentProgramStatus;

        public bool MoveNext;

        public Processor()
        {
            MoveNext = true;
            Registers = [];

            for (var index = 0; index < 8; index++)
            {
                var registerCodeType = Enum.Parse<RegisterCodeType>($"R{index}");
                Registers.Add(new Register(registerCodeType));
            }
            
            Registers.Add(Register.LR);
            Registers.Add(Register.PC);
            Registers.Add(Register.CPSR);
            Registers.Add(Register.SP);

            CurrentProgramStatus = Register.CPSR;

            Register.PC.Value = (int)MemorySectionOffset.TextRegionOffset;
            Register.SP.Value = (int)MemorySectionOffset.EndOffSet;
            
            ProgramCounter = Register.PC;
            StackPointer = Register.SP;
        }
    }
}
