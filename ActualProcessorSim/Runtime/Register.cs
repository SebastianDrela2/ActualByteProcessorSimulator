using ProcessorSim.Instructions;

namespace ActualProcessorSim.PhysicalComponent;

public class Register
{
    public RegisterCodeType RegisterCode;
    public int Value { get; set; }

    public static readonly Register LR = new(RegisterCodeType.LR);
    public static readonly Register CPSR = new(RegisterCodeType.CPSR);
    public static readonly Register PC = new(RegisterCodeType.PC);
    public static readonly Register SP = new(RegisterCodeType.SP);

    public Register(RegisterCodeType registerCodeType, int value = 0)
    {
        RegisterCode = registerCodeType;
        Value = value;
    }
}
