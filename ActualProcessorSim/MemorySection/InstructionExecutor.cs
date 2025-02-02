using ActualProcessorSim.PhysicalComponent;
using ActualProcessorSim.PrimitiveTypes;
using ActualProcessorSim.Runtime;

namespace ActualProcessorSim.MemorySection;

public class InstructionExecutor(Computer computer)
{
    private OffsettedMemory _bytes => computer.GetOffsettedProgramMemory();
    private readonly ByteDecoder _byteDecoder = new ByteDecoder(computer.Processor);

    public ExecuteInformation Execute(OpCodeType opCode)
    {
        return opCode switch
        {
            OpCodeType.MOV => ExecuteMov(),
            OpCodeType.ADD => ExecuteAdd(),
            OpCodeType.SUB => ExecuteSub(),
            OpCodeType.MUL => ExecuteMul(),
            OpCodeType.LS => ExecuteLs(),
            OpCodeType.RS => ExecuteRs(),
            OpCodeType.EXIT => ExecuteExit(),
            OpCodeType.BLA => ExecuteBla(),
            OpCodeType.BGE => ExecuteBge(),
            OpCodeType.BGT => ExecuteBgt(),
            OpCodeType.BLE => ExecuteBle(),
            OpCodeType.BLT => ExecuteBlt(),
            OpCodeType.END => ExecuteEnd(),
            OpCodeType.CMP => ExecuteCmp(),
            _ => throw new NotImplementedException(),
        };
    }

    private ExecuteInformation HandleArithmeticOperation(Action<Register, byte> operation)
    {
        var contextSwitch = (InstructionContext)_bytes[3];

        if (contextSwitch is not InstructionContext.RegisterToValue)
        {
            return InvalidContextResult(contextSwitch);
        }

        var register = _byteDecoder.DecodeRegister(_bytes[1]);
        var value = _byteDecoder.DecodeValue(_bytes[2]);

        operation(register, value);

        return SuccessResult();
    }

    private ExecuteInformation ExecuteAdd() => HandleArithmeticOperation((reg, val) => reg.Value += val);
    private ExecuteInformation ExecuteSub() => HandleArithmeticOperation((reg, val) => reg.Value -= val);
    private ExecuteInformation ExecuteMul() => HandleArithmeticOperation((reg, val) => reg.Value *= val);
    private ExecuteInformation ExecuteLs() => HandleArithmeticOperation((reg, val) => reg.Value <<= val);
    private ExecuteInformation ExecuteRs() => HandleArithmeticOperation((reg, val) => reg.Value >>= val);

    private ExecuteInformation ExecuteBranch(ProgramStatus? expected)
    {
        var contextSwitch = (InstructionContext)_bytes[5];

        if (contextSwitch is not InstructionContext.ControlFlow)
        {
            return InvalidContextResult(contextSwitch);
        }

        if (expected is not null && computer.Processor.CurrentProgramStatus.Value != (byte)expected)
        {
            return SuccessResult();
        }

        var value = _byteDecoder.DecodeValues(_bytes[1..5]);
        computer.Processor.ProgramCounter.Value = value;


        return SuccessJumpedPerformedResult();
    }

    private ExecuteInformation ExecuteBla() => ExecuteBranch(null);
    private ExecuteInformation ExecuteBge() => ExecuteBranch(ProgramStatus.GreaterOrEqual);
    private ExecuteInformation ExecuteBgt() => ExecuteBranch(ProgramStatus.Greater);
    private ExecuteInformation ExecuteBle() => ExecuteBranch(ProgramStatus.LessOrEqual);
    private ExecuteInformation ExecuteBlt() => ExecuteBranch(ProgramStatus.Less);


    private ExecuteInformation ExecuteMov()
    {
        var contextSwitch = (InstructionContext)_bytes[3];
        return contextSwitch switch
        {
            InstructionContext.RegisterToValue => MovRegisterToValue(),
            InstructionContext.RegisterToRegister => MovRegisterToRegister(),
            _ => InvalidContextResult(contextSwitch)
        };
    }

    private ExecuteInformation MovRegisterToValue()
    {
        _byteDecoder.DecodeRegister(_bytes[1]).Value = _byteDecoder.DecodeValue(_bytes[2]);

        return SuccessResult();
    }

    private ExecuteInformation MovRegisterToRegister()
    {
        _byteDecoder.DecodeRegister(_bytes[1]).Value = _byteDecoder.DecodeRegister(_bytes[2]).Value;

        return SuccessResult();

    }

    private ExecuteInformation ExecuteCmp()
    {
        var contextSwitch = (InstructionContext)_bytes[3];

        return contextSwitch switch
        {
            InstructionContext.RegisterToValue => CmpRegisterToValue(),
            InstructionContext.RegisterToRegister => CmpRegisterToRegister(),
            _ => InvalidContextResult(contextSwitch)
        };
    }

    private ExecuteInformation CmpRegisterToValue()
    {
        var reg = _byteDecoder.DecodeRegister(_bytes[1]);
        var value = _byteDecoder.DecodeValue(_bytes[2]);
        UpdateProgramStatus(reg.Value, value);

        return SuccessResult();
    }

    private ExecuteInformation CmpRegisterToRegister()
    {
        var reg1 = _byteDecoder.DecodeRegister(_bytes[1]);
        var reg2 = _byteDecoder.DecodeRegister(_bytes[2]);
        UpdateProgramStatus(reg1.Value, reg2.Value);

        return SuccessResult();
    }

    private void UpdateProgramStatus(int a, int b) => computer.Processor.CurrentProgramStatus.Value = (byte)GetProgramStatus(a, b);

    private ProgramStatus GetProgramStatus(int value1, int value2)
    {
        return (value1, value2) switch
        {
            var (x, y) when x == y => ProgramStatus.Equal,
            var (x, y) when x > y => ProgramStatus.Greater,
            var (x, y) when x < y => ProgramStatus.Less,
            var (x, y) when x >= y => ProgramStatus.GreaterOrEqual,
            var (x, y) when x <= y => ProgramStatus.LessOrEqual,
            _ => throw new InvalidOperationException("Unexpected comparison result.")
        };
    }

    private ExecuteInformation ExecuteExit()
    {
        var contextSwitch = (InstructionContext)_bytes[3];

        if (contextSwitch is not InstructionContext.RegisterToValue)
        {
            return InvalidContextResult(contextSwitch);
        }

        return SuccessResult();
    }

    private ExecuteInformation ExecuteEnd()
    {
        var contextSwitch = (InstructionContext)_bytes[1];

        if (contextSwitch is not InstructionContext.BreakControlFlow)
        {
            return InvalidContextResult(contextSwitch);
        }

        computer.Processor.MoveNext = false;

        return SuccessResult();
    }

    private static ExecuteInformation SuccessResult() => new ExecuteInformation(ExecuteStatus.Success, null);

    private static ExecuteInformation SuccessJumpedPerformedResult() => new ExecuteInformation(ExecuteStatus.Success, null, JumpedPerformed: true);

    private static ExecuteInformation InvalidContextResult(InstructionContext context) =>
        new ExecuteInformation(ExecuteStatus.Failure, new ArgumentException($"Invalid context switch: {context}"));

    // private void Push(byte chunk)
    // {
    //     computer.Memory[computer.Processor.]
    // }

    // private void Pop()
    // {

    // }
}
