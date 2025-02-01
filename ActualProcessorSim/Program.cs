using ActualProcessorSim.Assembly;
using ActualProcessorSim.Memory;
using ActualProcessorSim.PhysicalComponent;

namespace ActualProcessorSim;

internal class Program
{
	static void Main(string[] args)
	{
		var assembler = new Assembler();
        var memoryBuilder = new MemoryBuilder();
		assembler.Read();

		var bytes = assembler.BytesBuilder.Bytes;
        var processedMemory = memoryBuilder.Build(bytes);
        memoryBuilder.Link(assembler.LineInformations, processedMemory);

        var processor = new Processor(processedMemory);
        processor.Execute();
	}
}
