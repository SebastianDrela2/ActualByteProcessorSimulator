namespace ActualProcessorSim.MemorySection;

public readonly struct OffsettedMemory(RamMemory ram, int offset)
{
    public byte this[int index] => ram[offset + index];
}

// public class Memory
// {
//     private Dictionary<Address, ReadOnlySubView<byte>> _textRegion = [];
//     public Dictionary<Address, ReadOnlySubView<byte>> DataRegion { get; private set; } = new();
//     public Dictionary<Address, ReadOnlySubView<byte>> BssRegion { get; private set; } = new();
//     public Dictionary<Address, ReadOnlySubView<byte>> StackRegion { get; private set; } = new();

//     public Dictionary<Address, ReadOnlySubView<byte>> TextRegion
//     {
//         get => _textRegion;
//         set
//         {
//             _textRegion = value;
//             PopulateDataRegion();
//             PopulateBssRegion();
//             PopulateStackRegion();
//         }
//     }

//     private void PopulateDataRegion()
//     {
//         DataRegion.Clear();
//         int dataSize = Math.Min(_textRegion.Count / 2, 1024 * 1024); //1MB limit
//         int currentAddress = 0x00600000;

//         foreach (var kvp in _textRegion.Take(dataSize))
//         {
//             DataRegion[new Address(currentAddress)] = kvp.Value;
//             currentAddress++;
//         }
//     }

//     private void PopulateBssRegion()
//     {
//         BssRegion.Clear();
//         int bssSize = Math.Min(_textRegion.Count / 4, 1024 * 1024); // 1MB limit
//         int currentAddress = 0x00700000;

//         for (int i = 0; i < bssSize; i++)
//         {
//             BssRegion[new Address(currentAddress)] = new byte[] { 0x00 };
//             currentAddress++;
//         }
//     }

//     private void PopulateStackRegion()
//     {
//         StackRegion.Clear();
//         int stackSize = 1024 * 1024; // 1MB stack size
//         int currentAddress = 0x00FFFFFF;

//         for (int i = 0; i < stackSize; i++)
//         {
//             StackRegion[new Address(currentAddress)] = new byte[] { 0x00 };
//             currentAddress--;
//         }
//     }
//}
