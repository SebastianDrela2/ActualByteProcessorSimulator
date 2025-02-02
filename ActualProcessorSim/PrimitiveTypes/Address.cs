namespace ActualProcessorSim.Types;

public struct Address
{
    public int Value { get; }

    public Address(int value)
    {
        Value = value;
    }
    
    public override string ToString()
    {
        return $"{Value:X2}";
    }
}
