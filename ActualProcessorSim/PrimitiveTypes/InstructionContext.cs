namespace ActualProcessorSim.PrimitiveTypes;

public enum InstructionContext : byte
{
	None = 0,
	BreakControlFlow = 1,
	ControlFlow = 2,
	ValueToValue = 3,
	RegisterToValue = 4,
	RegisterToRegister = 5,
	Label = 6
}
