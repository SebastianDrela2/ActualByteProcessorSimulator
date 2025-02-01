namespace ActualProcessorSim.Types;

public struct Address
{
    public int Value { get; }

    public Address(int value)
    {
        Value = value;
    }

    public static implicit operator byte(Address address) => (byte)address.Value;

    public override string ToString()
    {
        return $"{Value:X2}";
    }
}
