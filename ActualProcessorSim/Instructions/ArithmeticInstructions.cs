using ActualProcessorSim.Intefacess;

namespace ActualProcessorSim.Types;

public class MovInstruction(ArgumentType argDest, ArgumentType argSrc) 
	: Instruction(OpCodeType.MOV, [argDest, argSrc]), IArithmeticInstruction
{ }

public class AddInstruction(ArgumentType argDest, ArgumentType argSrc)
	: Instruction(OpCodeType.ADD, [argDest, argSrc]), IArithmeticInstruction
{ }

public class SubInstruction(ArgumentType argDest, ArgumentType argSrc)
	: Instruction(OpCodeType.SUB, [argDest, argSrc]), IArithmeticInstruction
{ }

public class MulInstruction(ArgumentType argDest, ArgumentType argSrc)
	: Instruction(OpCodeType.MUL, [argDest, argSrc]), IArithmeticInstruction
{ }

public class LsInstruction(ArgumentType argDest, ArgumentType argSrc)
	: Instruction(OpCodeType.LS, [argDest, argSrc]), IArithmeticInstruction
{ }

public class RsInstruction(ArgumentType argDest, ArgumentType argSrc)
	: Instruction(OpCodeType.RS, [argDest, argSrc]), IArithmeticInstruction
{ }

public class CmpInstruction(ArgumentType argDest, ArgumentType argSrc)
    : Instruction(OpCodeType.CMP, [argDest, argSrc]), IArithmeticInstruction
{ }


