using ActualProcessorSim.Types;

namespace ActualProcessorSim.Assembly;

public static class InstructionsCr
{
    public static MovInstruction Mov(LineInformation lineInformation) => lineInformation switch
    {
        { IsRegisterToRegister: true } => new MovInstruction(ArgumentType.Register, ArgumentType.Register),
        { IsRegisterToValue: true } => new MovInstruction(ArgumentType.Register, ArgumentType.Literal),
        _ => throw new ArgumentException("Invalid arguements."),
    };

    public static AddInstruction Add(LineInformation lineInformation) => lineInformation switch
    {
        { IsRegisterToValue: true } => new AddInstruction(ArgumentType.Register, ArgumentType.Literal),
        _ => throw new ArgumentException("Invalid arguements."),
    };

    public static SubInstruction Sub(LineInformation lineInformation) => lineInformation switch
    {
        { IsRegisterToValue: true } => new SubInstruction(ArgumentType.Register, ArgumentType.Literal),
        _ => throw new ArgumentException("Invalid arguements."),
    };

    public static MulInstruction Mul(LineInformation lineInformation) => lineInformation switch
    {
        { IsRegisterToValue: true } => new MulInstruction(ArgumentType.Register, ArgumentType.Literal),
        _ => throw new ArgumentException("Invalid arguements."),
    };

    public static LsInstruction Ls(LineInformation lineInformation) => lineInformation switch
    {
        { IsRegisterToValue: true } => new LsInstruction(ArgumentType.Register, ArgumentType.Literal),
        _ => throw new ArgumentException("Invalid arguements."),
    };

    public static RsInstruction Rs(LineInformation lineInformation) => lineInformation switch
    {
        { IsRegisterToValue: true } => new RsInstruction(ArgumentType.Register, ArgumentType.Literal),
        _ => throw new ArgumentException("Invalid arguements."),
    };

    public static ExitInstruction Exit(LineInformation lineInformation) => lineInformation switch
    {
        { IsOpCodeBasedOnly: true } => new ExitInstruction(),
        _ => throw new ArgumentException("Invalid arguements."),
    };

    public static BlaInstruction Bla(LineInformation lineInformation) => lineInformation switch
    {
        { IsConditionalJump: true } => new BlaInstruction(ArgumentType.Address),
        _ => throw new ArgumentException("Invalid arguements."),
    };
    public static BgeInstruction Bge(LineInformation lineInformation) => lineInformation switch
    {
        { IsConditionalJump: true } => new BgeInstruction(ArgumentType.Address),
        _ => throw new ArgumentException("Invalid arguements."),
    };

    public static BgtInstruction Bgt(LineInformation lineInformation) => lineInformation switch
    {
        { IsConditionalJump: true } => new BgtInstruction(ArgumentType.Address),
        _ => throw new ArgumentException("Invalid arguements."),
    };

    public static BleInstruction Ble(LineInformation lineInformation) => lineInformation switch
    {
        { IsConditionalJump: true } => new BleInstruction(ArgumentType.Address),
        _ => throw new ArgumentException("Invalid arguements."),
    };

    public static BltInstruction Blt(LineInformation lineInformation) => lineInformation switch
    {
        { IsConditionalJump: true } => new BltInstruction(ArgumentType.Address),
        _ => throw new ArgumentException("Invalid arguements."),
    };

    public static EndInstruction End(LineInformation lineInformation) => lineInformation switch
    {
        { IsOpCodeBasedOnly: true } => new EndInstruction(),
        _ => throw new ArgumentException("Invalid arguements."),
    };

    public static CmpInstruction Cmp(LineInformation lineInformation) => lineInformation switch
    {
        { IsRegisterToRegister: true } => new CmpInstruction(ArgumentType.Register, ArgumentType.Register),
        { IsRegisterToValue: true } => new CmpInstruction(ArgumentType.Register, ArgumentType.Literal),
        _ => throw new ArgumentException("Invalid arguements."),
    };
}
