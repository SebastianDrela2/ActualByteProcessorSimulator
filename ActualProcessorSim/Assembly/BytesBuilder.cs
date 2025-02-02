using System.Collections;
using System.Collections.Immutable;
using System.Diagnostics.CodeAnalysis;
using ActualProcessorSim.Intefacess;
using ActualProcessorSim.PrimitiveTypes;
using ActualProcessorSim.Types;
using ProcessorSim.Instructions;

namespace ActualProcessorSim.Assembly;

public class BytesBuilder(ImmutableArray<byte> bytes) : IReadOnlyList<byte>
{
    private ImmutableArray<byte>.Builder Bytes { get; } = bytes.ToBuilder();

    public int Position => Bytes.Count;

    public int Count => throw new NotImplementedException();

    public byte this[int index] => Bytes[index];
    public ReadOnlySubView<byte> this[Range range] => Bytes.SubList(range);

    public BytesBuilder()
        : this([]) { }

    public void CopyTo(byte[] array, int index = 0)
    {
        Bytes.CopyTo(array, index);
    }

    public int Write(Instruction instruction, LineInformation lineInformation)
    {
        int sum = Write(lineInformation.OpCode);

        foreach (var (argType, argText) in instruction.Arguments.Zip(lineInformation.SegmentedLines.Skip(1)))
        {
            sum += WriteArgument(argText, argType);
        }

        sum += Write(GetInstructionContext(instruction, lineInformation));
        return sum;
    }

    public int WriteArgument(ReadOnlySpan<char> argText, ArgumentType argType) => argType switch
    {
        ArgumentType.Register => TryWriteRegisterValue(argText),
        ArgumentType.Literal => TryWriteLiteralValue(argText),
        ArgumentType.Address => TryWriteAddressValue(argText),
        _ => throw new ArgumentOutOfRangeException(nameof(argType)),
    };

    public int TryWriteRegisterValue(ReadOnlySpan<char> text)
    {
        if (!TryParse(text, out var parsed)) throw InvalidSyntax($"""Cannot parse "{text}" as {nameof(RegisterCodeType)} enum""");
        return Write(parsed);
    }

    public int TryWriteLiteralValue(ReadOnlySpan<char> text)
    {
        if (text is not ['#', .. var subText]) throw InvalidSyntax($"""Invalid syntax - expected to start with '#'""");
        if (!byte.TryParse(subText, out byte parsed)) throw InvalidSyntax($"""Invalid syntax - unable to parse {subText} as byte""");
        return Write(parsed);
    }
    public int TryWriteAddressValue(ReadOnlySpan<char> argText)
    {
        if (!byte.TryParse(argText, out byte parsed)) throw InvalidSyntax($"""Invalid syntax - unable to parse {argText} as byte""");
        return Write(parsed);

    }
    private InstructionContext GetInstructionContext(Instruction instruction, LineInformation lineInformation) => (instruction, lineInformation) switch
    {
        (IBreakControlFlowInstruction, _) => InstructionContext.BreakControlFlow,
        (IControlFlowInstruction, _) => InstructionContext.ControlFlow,
        (_, { IsValueToValue: true }) => InstructionContext.ValueToValue,
        (_, { IsRegisterToValue: true }) => InstructionContext.RegisterToValue,
        (_, { IsRegisterToRegister: true }) => InstructionContext.RegisterToRegister,
        _ => InstructionContext.None
    };

    public int Write(byte value)
    {
        Bytes.Add(value);
        return 1;
    }

    public int WriteRange(ReadOnlySpan<byte> values)
    {
        Bytes.AddRange(values);
        return values.Length;
    }

    public int Write(OpCodeType value) => Write((byte)value);
    public int Write(RegisterCodeType value) => Write((byte)value);
    public int Write(InstructionContext value) => Write((byte)value);
    public int WriteLabel(string label) => label.Sum(c => Write((byte)c));

    public IEnumerator<byte> GetEnumerator() => Bytes.GetEnumerator();
    IEnumerator IEnumerable.GetEnumerator() => Bytes.GetEnumerator();

    public bool TryParse(ReadOnlySpan<char> register, out RegisterCodeType value) => EnumParser.TryParse(register, out value);
    public class InvalidSyntaxException(string message) : Exception(message)
    {
        public static InvalidSyntaxException Create(string message) => new(message);

        [DoesNotReturn]
        public static void Throw(string message) => throw Create(message);
        public static void ThrowIfFalse([DoesNotReturnIf(false)] bool value, string message)
        {
            if (!value) throw Create(message);
        }
    }
    public Exception InvalidSyntax(string message) => new InvalidOperationException(message);
}
