using ProcessorSim.Instructions;

namespace ActualProcessorSim.PhysicalComponent;

public class Register
{
    public RegisterCodeType RegisterCode;
    public byte Value { get; set; }
    public Register(RegisterCodeType registerCodeType, byte value)
    {
        RegisterCode = registerCodeType;
        Value = value;
    }
}
