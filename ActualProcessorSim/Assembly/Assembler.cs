using ActualProcessorSim.Types;

namespace ActualProcessorSim.Assembly;

public class Assembler
{
	private const string InstructionsPath = "ActualProcessorSim.InstructionsToExecute.txt";

	private readonly InstructionLineResolver _instructionLineResolver = new();
	public readonly BytesBuilder BytesBuilder = new();

    public List<InstructionLineInformation>? LineInformations;


	public void Read()
	{
		var lineInformations = ProcessResource(InstructionsPath)
			.Select(ReadLine)
            .ToList();
        
        LineInformations = lineInformations;
    }

	private LineInformation GetLineInformation(string[] parts) => _instructionLineResolver.GetLineInformation(parts);

	private InstructionLineInformation ReadLine(string instructionLine)
	{
		var segmentedLines = instructionLine.Split(' ');
		var lineInformation = GetLineInformation(segmentedLines);

		Instruction instruction = lineInformation.OpCode switch
		{
			OpCodeType.MOV => InstructionsCr.Mov(lineInformation),
			OpCodeType.ADD => InstructionsCr.Add(lineInformation),
			OpCodeType.SUB => InstructionsCr.Sub(lineInformation),
            OpCodeType.MUL => InstructionsCr.Mul(lineInformation),
            OpCodeType.LS => InstructionsCr.Ls(lineInformation),
            OpCodeType.RS => InstructionsCr.Rs(lineInformation),
            OpCodeType.EXIT => InstructionsCr.Exit(lineInformation),
            OpCodeType.BLA => InstructionsCr.Bla(lineInformation),
            OpCodeType.BGE => InstructionsCr.Bge(lineInformation),
            OpCodeType.BGT => InstructionsCr.Bgt(lineInformation),
            OpCodeType.BLE => InstructionsCr.Ble(lineInformation),
            OpCodeType.BLT => InstructionsCr.Blt(lineInformation),
            OpCodeType.END => InstructionsCr.End(lineInformation),
            OpCodeType.CMP => InstructionsCr.Cmp(lineInformation),
            var x => throw new NotImplementedException($"Unrecognized instruction: {x}"),
		};

		var postion = BytesBuilder.Position;
		var bytes = BytesBuilder.Write(instruction, lineInformation);

		return new InstructionLineInformation(lineInformation, instructionLine, bytes, postion);
	}

	private static IEnumerable<string> ProcessResource(string resourceName)
	{
		var assemblyInstructions = new List<string>();
		var stream = System.Reflection.Assembly.GetExecutingAssembly().GetManifestResourceStream(resourceName)!;
        var instructions = GetInstructionsBeforeSymbolResolution(stream);

        instructions = ProcessLabels(instructions);

		return instructions;
	}

    private static List<string> ProcessLabels(List<string> instructions)
    {
        var labels = instructions.Where(x => x.Contains(':'));

        var labelMap = labels.Select(label =>
        {
            var index = instructions.FindIndex(x => x == label);
            return (Name : label.TrimEnd(':'), Index : index);
        })
        .ToDictionary(selector => selector.Name, selector => selector.Index);
        
        instructions.RemoveAll(instructions => instructions[instructions.Length - 1] == ':');
        
        var updatedInstructions = instructions
        .Select(instruction =>
        {
            var splitInstruction = instruction.Split(' ');

            if (splitInstruction.Length > 0)
            {
                foreach (var possibleLabel in splitInstruction)
                {
                    var hasLabel = labelMap.TryGetValue(possibleLabel, out var address);

                    if (address is not 0)
                    {
                        return instruction.Replace(possibleLabel, address.ToString());
                    }
                }
            }

            return instruction;
        }).ToList();


        return updatedInstructions;
    }

    private static List<string> GetInstructionsBeforeSymbolResolution(Stream stream)
    {
        var instructions = new List<string>();
        using var reader = new StreamReader(stream);

        while (!reader.EndOfStream)
        {
            var line = reader.ReadLine();

            if (!string.IsNullOrEmpty(line)) 
            {
                instructions.Add(line);
            }
        }

        return instructions;
    }
}
