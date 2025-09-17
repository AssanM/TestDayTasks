using MemoryPack;

namespace testDay4.contracts.Requests;

[MemoryPackable]
public partial class GetObjecstInAreaRequest
{
    public int X1 { get; set; }
    public int Y1 { get; set; }
    public int X2 { get; set; }
    public int Y2 { get; set; }
}
