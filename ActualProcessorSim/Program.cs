using System.Collections.Immutable;
using ActualProcessorSim.Assembly;
using ActualProcessorSim.Memory;
using ActualProcessorSim.MemorySection;
using ActualProcessorSim.PhysicalComponent;
using ActualProcessorSim.Runtime;

namespace ActualProcessorSim;

internal class Program
{
	static void Main()
	{
		var assembler = new Assembler();
        var memoryBuilder = new MemoryBuilder();
		assembler.Read();

		var memory = new RamMemory(assembler.BytesBuilder);	

        memoryBuilder.Link(assembler.LineInformations!, memory);

		var processor = new Processor();
        var computer = new Computer()
		{
			Processor = processor,
			Memory = memory
		};

        computer.Execute();
	}
}
