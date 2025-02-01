using ActualProcessorSim.Memory;
using ActualProcessorSim.Types;
using ProcessorSim.Instructions;

namespace ActualProcessorSim.PhysicalComponent
{
	public class Processor
	{
        public readonly InstructionExecutor _instructionExecutor;
        public readonly Dictionary<Address, byte[]> ProcessorMemory;
        public readonly IList<Register> Registers;

        public Register ProgramCounter;
        public Register CurrentProgramStatus;

        public bool MoveNext;

        public Processor(Dictionary<Address, byte[]> builtMemory) 
		{
            _instructionExecutor = new InstructionExecutor(this);
            MoveNext = true;
			ProcessorMemory = builtMemory;
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
            ProgramCounter = programCounter;
        }

        public void Execute()
        {
            Console.WriteLine($"Executed bytes:");

            while (MoveNext)
            {
                var instructionBytes = ProcessorMemory[new Address(ProgramCounter.Value)];             
                var executeInformation = _instructionExecutor.Execute(instructionBytes);

                if (executeInformation.Exception is not null)
                {
                    throw executeInformation.Exception;
                }

                var bytesText = string.Join(" ", instructionBytes.Select(@byte => $"{@byte:X2}"));
                Console.WriteLine($"{ProgramCounter.Value:X8} |{bytesText, 12}");

                if (!executeInformation.JumpedPerformed)
                {
                    ProgramCounter.Value += (byte)instructionBytes.Length;
                }

            }
        }
	}
}
