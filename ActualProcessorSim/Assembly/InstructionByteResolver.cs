using ActualProcessorSim.Intefacess;
using ActualProcessorSim.PrimitiveTypes;
using ActualProcessorSim.Types;
using ProcessorSim.Instructions;

namespace ActualProcessorSim.Assembly;

public class InstructionByteResolver
{
	public byte[] GetBytesFromInstruction(Instruction instruction, LineInformation lineInformation)
	{
		var bytes = new List<byte>
		{
			(byte)lineInformation.OpCode
		};

		if (instruction is IArithmeticInstruction)
		{
			AddBytesForArithmeticInstruction(lineInformation, bytes);			
		}
		else if (instruction is IControlFlowInstruction && instruction is not IBreakControlFlowInstruction)
		{
			var address = lineInformation.SegmentedLines[1];
			var byteAdress = GetBytesFromLabel(address);

			bytes.AddRange(byteAdress);
		}

		bytes.Add((byte)GetInstructionContext(instruction, lineInformation));

		return [.. bytes];
	}

	private void AddBytesForArithmeticInstruction(LineInformation lineInformation, List<byte> bytes)
	{
		if (lineInformation.IsValueToValue)
		{
			var immiediateValueOne = GetByteFromImmediateValue(lineInformation.SegmentedLines[1]);
			var immiediateValueTwo = GetByteFromImmediateValue(lineInformation.SegmentedLines[2]);

			bytes.Add(immiediateValueOne);
			bytes.Add(immiediateValueTwo);
		}

		if (lineInformation.IsRegisterToValue)
		{
			var register = GetRegisterCodeType(lineInformation.SegmentedLines[1]);
			var immiediateValue = GetByteFromImmediateValue(lineInformation.SegmentedLines[2]);

			bytes.Add((byte)register);
			bytes.Add(immiediateValue);
		}
	}

	private RegisterCodeType GetRegisterCodeType(string register) => register.ToLower() switch
	{
		"r0" => RegisterCodeType.R0,
		"r1" => RegisterCodeType.R1,
		"r2" => RegisterCodeType.R2,
		"r3" => RegisterCodeType.R3,
		"r4" => RegisterCodeType.R4,
		"r5" => RegisterCodeType.R5,
		"r6" => RegisterCodeType.R6,
		"r7" => RegisterCodeType.R7,		
		"r9" => RegisterCodeType.LR,
		"lr" => RegisterCodeType.LR,
		"cpsr" => RegisterCodeType.CPSR,
		_ => throw new ArgumentException($"Invalid register: {register}"),
	};

	private List<byte> GetBytesFromLabel(string label)
	{
		var bytes = new List<byte>();

		foreach(var c in label)
		{
			bytes.Add((byte)c);
		}

		return bytes;
	}

	private byte GetByteFromImmediateValue(string immiediateValueSharp) => byte.Parse(immiediateValueSharp.Replace("#", ""));

	private InstructionContext GetInstructionContext(Instruction instruction, LineInformation lineInformation) => (instruction, lineInformation) switch
	{
		(IBreakControlFlowInstruction, _) => InstructionContext.BreakControlFlow,
		(IControlFlowInstruction, _) => InstructionContext.ControlFlow,
		(_,  { IsValueToValue: true }) => InstructionContext.ValueToValue,
		(_,  { IsRegisterToValue: true }) => InstructionContext.RegisterToValue,
		(_,  { IsRegisterToRegister: true }) => InstructionContext.RegisterToRegister,
		_ => InstructionContext.None
	};
}
