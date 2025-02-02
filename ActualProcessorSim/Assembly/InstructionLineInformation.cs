namespace ActualProcessorSim.Assembly;
public struct InstructionLineInformation
{
    public LineInformation LineInformation { get; }
    public string Instruction { get; }
    public int BytesLength { get; }
    public int Position { get; }

    public InstructionLineInformation(LineInformation lineInformation, string instruction, int bytesLength, int position)
    {
        LineInformation = lineInformation;
        Instruction = instruction;
        BytesLength = bytesLength;
        Position = position;
    }
}
