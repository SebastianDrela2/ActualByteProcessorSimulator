
namespace ActualProcessorSim;
public class DisplayDiagnostic
{
    public List<string> Memory = [];
    public List<string> ExecutedBytes = [];
    public List<string> Stack = [];

    public void DisplayBytes()
    {
        Console.Clear();

        foreach (var memory in Memory)
        {
            Console.WriteLine(memory);
        }

        foreach (var memory in ExecutedBytes)
        {
            Console.WriteLine(memory);
        }

        foreach (var memory in Stack)
        {
            Console.WriteLine(memory);
        }
    }
}
