namespace ActualProcessorSim.Assembly
{
    internal class InstructionLineResolver
    {
        public LineInformation GetLineInformation(string[] segmentedLines)
        {
            var lineInformation = new LineInformation();
            var opCodeType = GetOpCodeType(segmentedLines);

            lineInformation.OpCode = opCodeType!.Value;

            SetLineinformationFromNonOpCode(ref lineInformation, segmentedLines, 1);
            SetLineinformationFromNonOpCode(ref lineInformation, segmentedLines, 2);
            SetLineinformationFromNonOpCode(ref lineInformation, segmentedLines, 3);

            lineInformation.Length = segmentedLines.Length;
            lineInformation.SegmentedLines = segmentedLines;

            return lineInformation;
        }

        private OpCodeType? GetOpCodeType(string[] segmentedLines)
        {
            if (Enum.TryParse<OpCodeType>(segmentedLines[0], true, out var opCode))
            {
                return opCode;
            }

            return null;
        }

        private int? SetLineinformationFromNonOpCode(ref LineInformation lineInformation, string[] segmentedLines, int index)
        {
            if (segmentedLines.Length > index)
            {
                if (IsRegister(segmentedLines[index]))
                {
                    return lineInformation.AmountOfRegisters++;
                }

                if (IsValue(segmentedLines[index]))
                {
                    return lineInformation.AmountOfValues++;
                }

                if (IsAddress(segmentedLines[index]))
                {
                    return lineInformation.AmountOfAddress++;
                }

                if (IsText(segmentedLines[index]))
                {
                    return lineInformation.AmountOfText++;
                }

                throw new ArgumentException("Unrecognized segment.");
            }

            return null; // it's fine that means line is not as long
        }

        private static bool IsRegister(string segmentedLine)
            => segmentedLine.StartsWith("r") || segmentedLine == "cspr" || segmentedLine == "lr";

        private static bool IsValue(string segmentedLine) => segmentedLine.StartsWith("#");

        private static bool IsText(string segmentedLine)
            => !IsRegister(segmentedLine) && !IsValue(segmentedLine) && !IsAddress(segmentedLine);

        private static bool IsAddress(string segmentedLine) => int.TryParse(segmentedLine, out var _);
    }
}
