using ActualProcessorSim.Intefacess;

namespace ActualProcessorSim.Types
{
    public abstract class Instruction(OpCodeType opCode, IReadOnlyList<ArgumentType> arguements) : IInstruction
    {
        public int ArguementsLength => Arguments.Count;

        public OpCodeType OpCode { get; } = opCode;
        public IReadOnlyList<ArgumentType> Arguments { get; } = arguements;
    }
}
