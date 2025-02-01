
namespace ActualProcessorSim.PrimitiveTypes;
public record class ExecuteInformation(ExecuteStatus ExecuteStatus, Exception? Exception, bool JumpedPerformed = false);

