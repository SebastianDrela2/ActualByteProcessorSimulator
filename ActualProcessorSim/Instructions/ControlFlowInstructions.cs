using ActualProcessorSim.Intefacess;

namespace ActualProcessorSim.Types;

public class BlaInstruction(ArgumentType address) 
	: Instruction(OpCodeType.BLA, [address]), IControlFlowInstruction
{ }

public class BgeInstruction(ArgumentType address) 
	: Instruction(OpCodeType.BGE, [address]), IControlFlowInstruction
{ }

public class BgtInstruction(ArgumentType address) 
	: Instruction(OpCodeType.BGT, [address]), IControlFlowInstruction
{ }

public class BleInstruction(ArgumentType address) 
	: Instruction(OpCodeType.BLE, [address]), IControlFlowInstruction
{ }

public class BltInstruction(ArgumentType address)
    : Instruction(OpCodeType.BLT, [address]), IControlFlowInstruction
{ }

public class ExitInstruction() 
	: Instruction(OpCodeType.EXIT, []), IBreakControlFlowInstruction
{ }

public class EndInstruction() 
	: Instruction(OpCodeType.END, []), IBreakControlFlowInstruction
{ }
