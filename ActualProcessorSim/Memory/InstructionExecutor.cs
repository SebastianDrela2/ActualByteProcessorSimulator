using ActualProcessorSim.PhysicalComponent;
using ActualProcessorSim.PrimitiveTypes;

namespace ActualProcessorSim.Memory;

public class InstructionExecutor(Processor processor)
{
    private readonly ByteDecoder _byteDecoder = new ByteDecoder(processor);

    public ExecuteInformation Execute(byte[] bytes)
    {
        var opCode = (OpCodeType)bytes[0];

        return opCode switch
        {
            OpCodeType.MOV => ExecuteMov(bytes),
            OpCodeType.ADD => ExecuteAdd(bytes),
            OpCodeType.SUB => ExecuteSub(bytes),
            OpCodeType.MUL => ExecuteMul(bytes),
            OpCodeType.LS => ExecuteLs(bytes),
            OpCodeType.RS => ExecuteRs(bytes),
            OpCodeType.EXIT => ExecuteExit(bytes),
            OpCodeType.BLA => ExecuteBla(bytes),
            OpCodeType.BGE => ExecuteBge(bytes),
            OpCodeType.BGT => ExecuteBgt(bytes),
            OpCodeType.BLE => ExecuteBle(bytes),
            OpCodeType.BLT => ExecuteBlt(bytes),
            OpCodeType.END => ExecuteEnd(bytes),
            OpCodeType.CMP => ExecuteCmp(bytes),
            _ => throw new NotImplementedException(),
        };
    }

    private ExecuteInformation HandleArithmeticOperation(byte[] bytes, Action<Register, byte> operation)
    {
        var contextSwitch = (InstructionContext)bytes[3];

        if (contextSwitch is not InstructionContext.RegisterToValue)
        {
            return InvalidContextResult(contextSwitch);
        }

        var register = _byteDecoder.DecodeRegister(bytes[1]);
        var value = _byteDecoder.DecodeValue(bytes[2]);

        operation(register, value);

        return SuccessResult();
    }

    private ExecuteInformation ExecuteAdd(byte[] bytes) => HandleArithmeticOperation(bytes, (reg, val) => reg.Value += val);
    private ExecuteInformation ExecuteSub(byte[] bytes) => HandleArithmeticOperation(bytes, (reg, val) => reg.Value -= val);
    private ExecuteInformation ExecuteMul(byte[] bytes) => HandleArithmeticOperation(bytes, (reg, val) => reg.Value *= val);
    private ExecuteInformation ExecuteLs(byte[] bytes) => HandleArithmeticOperation(bytes, (reg, val) => reg.Value <<= val);
    private ExecuteInformation ExecuteRs(byte[] bytes) => HandleArithmeticOperation(bytes, (reg, val) => reg.Value >>= val);

    private ExecuteInformation ExecuteBranch(byte[] bytes)
    {
        var contextSwitch = (InstructionContext)bytes[2];

        if (contextSwitch is not InstructionContext.ControlFlow)
        {
            return InvalidContextResult(contextSwitch);
        }

        var value = _byteDecoder.DecodeValue(bytes[1]);
        processor.ProgramCounter.Value = value;

        return SuccessJumpedPerformedResult();
    }

    private ExecuteInformation ExecuteBla(byte[] bytes) => ExecuteBranch(bytes);
    private ExecuteInformation ExecuteBge(byte[] bytes) => ExecuteBranch(bytes);
    private ExecuteInformation ExecuteBgt(byte[] bytes) => ExecuteBranch(bytes);
    private ExecuteInformation ExecuteBle(byte[] bytes) => ExecuteBranch(bytes);
    private ExecuteInformation ExecuteBlt(byte[] bytes) => ExecuteBranch(bytes);


    private ExecuteInformation ExecuteMov(byte[] bytes)
    {
        var contextSwitch = (InstructionContext)bytes[3];
        return contextSwitch switch
        {
            InstructionContext.RegisterToValue => MovRegisterToValue(bytes),
            InstructionContext.RegisterToRegister => MovRegisterToRegister(bytes),
            _ => InvalidContextResult(contextSwitch)
        };
    }

    private ExecuteInformation MovRegisterToValue(byte[] bytes)
    {
        _byteDecoder.DecodeRegister(bytes[1]).Value = _byteDecoder.DecodeValue(bytes[2]);

        return SuccessResult();
    }

    private ExecuteInformation MovRegisterToRegister(byte[] bytes)
    {
        _byteDecoder.DecodeRegister(bytes[1]).Value = _byteDecoder.DecodeRegister(bytes[2]).Value;

        return SuccessResult();

    }

    private ExecuteInformation ExecuteCmp(byte[] bytes)
    {
        var contextSwitch = (InstructionContext)bytes[3];

        return contextSwitch switch
        {
            InstructionContext.RegisterToValue => CmpRegisterToValue(bytes),
            InstructionContext.RegisterToRegister => CmpRegisterToRegister(bytes),
            _ => InvalidContextResult(contextSwitch)
        };
    }

    private ExecuteInformation CmpRegisterToValue(byte[] bytes)
    {
        var reg = _byteDecoder.DecodeRegister(bytes[1]);
        var value = _byteDecoder.DecodeValue(bytes[2]);
        UpdateProgramStatus(reg.Value, value);

        return SuccessResult();
    }

    private ExecuteInformation CmpRegisterToRegister(byte[] bytes)
    {
        var reg1 = _byteDecoder.DecodeRegister(bytes[1]);
        var reg2 = _byteDecoder.DecodeRegister(bytes[2]);
        UpdateProgramStatus(reg1.Value, reg2.Value);

        return SuccessResult();
    }

    private void UpdateProgramStatus(byte a, byte b) => processor.CurrentProgramStatus.Value = (byte)GetProgramStatus(a, b);

    private ProgramStatus GetProgramStatus(byte value1, byte value2)
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

    private ExecuteInformation ExecuteExit(byte[] bytes)
    {
        var contextSwitch = (InstructionContext)bytes[3];

        if (contextSwitch is not InstructionContext.RegisterToValue)
        {
            return InvalidContextResult(contextSwitch);
        }

        return SuccessResult();
    }

    private ExecuteInformation ExecuteEnd(byte[] bytes)
    {
        var contextSwitch = (InstructionContext)bytes[1];

        if (contextSwitch is not InstructionContext.BreakControlFlow)
        {
            return InvalidContextResult(contextSwitch);
        }

        processor.MoveNext = false;

        return SuccessResult();
    }

    private static ExecuteInformation SuccessResult() => new ExecuteInformation(ExecuteStatus.Success, null);

    private static ExecuteInformation SuccessJumpedPerformedResult() => new ExecuteInformation(ExecuteStatus.Success, null, JumpedPerformed: true);

    private static ExecuteInformation InvalidContextResult(InstructionContext context) =>
        new ExecuteInformation(ExecuteStatus.Failure,  new ArgumentException($"Invalid context switch: {context}"));
}
