namespace ActualProcessorSim.Assembly;
public struct InstructionLineInformation
{
    public LineInformation LineInformation { get; }
    public string Instruction { get; }
    public int Bytes { get; }
    public int Position { get; }

    public InstructionLineInformation(LineInformation lineInformation, string instruction, int bytes, int position)
    {
        LineInformation = lineInformation;
        Instruction = instruction;
        Bytes = bytes;
        Position = position;
    }
}
