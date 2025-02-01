using ActualProcessorSim.PrimitiveTypes;
using ProcessorSim.Instructions;

namespace ActualProcessorSim.Intefacess;

public interface IInstruction
{
	OpCodeType OpCode { get; }

	IReadOnlyList<ArgumentType> Arguments { get; }
}
