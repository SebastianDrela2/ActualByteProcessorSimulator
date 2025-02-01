namespace ActualProcessorSim.Assembly;

public struct LineInformation
{
	public OpCodeType OpCode;
	public int Length;
    public int AmountOfAddress;
	public int AmountOfValues;
    public int AmountOfText;
	public int AmountOfRegisters;
	public string[] SegmentedLines;

	public readonly bool IsRegisterToRegister => AmountOfRegisters is 2;
	public readonly bool IsRegisterToValue => AmountOfRegisters is 1 && AmountOfValues is 1;
	public readonly bool IsValueToValue => AmountOfValues is 2;

    public readonly bool IsOpCodeBasedOnly => AmountOfText is 0 && AmountOfRegisters is 0 && AmountOfValues is 0;
    public readonly bool IsConditionalJump => AmountOfAddress is 1;
}
